using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using edu.stanford.nlp.tagger.maxent;
using java.io;
using LemmaSharp;


namespace BibReader.Analysis
{
    public class TextAnalysis
    {
        private static Dictionary<string, string> TagsPos { get; } = new Dictionary<string, string>
        {
            {"CC", "Союз"},
            {"IN", "Предлог"},
            {"JJ", "Прилагательное"},
            {"JJR", "Прилагательное"},
            {"JJS", "Прилагательное"},
            {"EX", "Глагол"},
            {"NN", "Существительное"},
            {"NNS", "Существительное"},
            {"NNP", "Существительное"},
            {"NNPS", "Существительное"},
            {"RB", "Наречие"},
            {"RBR", "Наречие"},
            {"RBS", "Наречие"},
            {"VB", "Глагол"},
            {"VBD", "Глагол"},
            {"VBG", "Глагол"},
            {"VBN", "Глагол"},
            {"VBP", "Глагол"},
        };

        public static Dictionary<string, int> Concepts { get; private set; } = new Dictionary<string, int>();

        public static List<string> Persons { get; private set; } = new List<string>();

        public static List<string> Organizations { get; private set; } = new List<string>();

        public static Dictionary<string, string> POS = new Dictionary<string, string>();

        public static string Names;

        public static void getAnalysis(string text)
        {
            Concepts = new Dictionary<string, int>();
            Persons = new List<string>();
            Organizations = new List<string>();
            POS = new Dictionary<string, string>();

            SetConceptsAnalysis(text);

            Names = WorkOpenNLP.GetNameEntity(text);

            SetNamesAnalysis();
            SetOrganizationAnalysis();

        }

        public static int getMaxVvalueConcepts()
        {
            return Concepts.Values.Max();
        }

        public static int getMinVvalueConcepts()
        {
            return Concepts.Values.Min();
        }

        public static double getAverageValueConcepts()
        {
           return Concepts.Values.Average();
        }

        private static bool IsRussian(string word)
        {
            return !Regex.IsMatch(word, @"\P{IsCyrillic}");
        }

        private static bool IsEnglish(string word)
        {
           return !Regex.IsMatch(word, @"\P{IsBasicLatin}");
        }

        private static void SetConceptsAnalysis(string text)
        {
            string[] tokens = WorkOpenNLP.Tokenisation(text);

            ILemmatizer lmtz_en = new LemmatizerPrebuiltCompact(LanguagePrebuilt.English);
            ILemmatizer lmtz_ru = new LemmatizerPrebuiltCompact(LanguagePrebuilt.Russian);

            dynamic tagger = PartsOfSpeech.Initialize();

            foreach (string token in tokens)
            {
                string tok = token.ToLower();
                if (tok.Length >= 3 && char.IsLetter(tok, tok.Length - 1))
                {
                    if (IsRussian(tok))
                    {
                        if (
                             !Stopwords.particles_ru.Contains(tok) &&
                             !Stopwords.pronouns_ru.Contains(tok) &&
                             !Stopwords.unions_ru.Contains(tok) &&
                             !Stopwords.otherStopwords_ru.Contains(tok)
                             )
                        {
                            tok = lmtz_ru.Lemmatize(tok);
                        }
                        else continue;
                    }
                    else if (IsEnglish(tok))
                    {
                        if (
                           !Stopwords.particles_en.Contains(tok) &&
                           !Stopwords.pronouns_en.Contains(tok) &&
                           !Stopwords.unions_en.Contains(tok) &&
                           !Stopwords.otherStopwords_en.Contains(tok)
                           )
                            tok = lmtz_en.Lemmatize(tok);
                        else continue;
                    } else
                    {
                        continue;
                    }

                    var sentences = MaxentTagger.tokenizeText(
                        new StringReader(Regex.Replace(text, @"[^\w\d\s]", ""))
                        ).toArray();
                    var taggedSentence = tagger.tagSentence(sentences).toArray();

                    if (Concepts.ContainsKey(tok))
                    {
                        Concepts[tok]++;
                    }
                    else
                    {
                        string tag = taggedSentence[0].tag();
                        POS.Add(tok, TagsPos[tag]);
                        Concepts.Add(tok, 1);
                    }
                }
            }
        }

        public static string GetParseTree(string text)
        {
            return WorkOpenNLP.Parse_tree(text).Show().ToString();
        }

        private static void SetNamesAnalysis()
        {
            
            Regex regex = new Regex(@"(?<=<person>)([\s\S]+?)(?=</person>)");

            MatchCollection names = regex.Matches(Names);

            foreach (Match name in names)
            {
                GroupCollection groups = name.Groups;
                string person = groups[0].Value;
                if (person.Length > 1 && !Persons.Contains(person))
                    Persons.Add(groups[0].Value);          
            }
        }

        private static void SetOrganizationAnalysis()
        {
            Regex regex = new Regex(@"(?<=<organization>)([\s\S]+?)(?=</organization>)");

            MatchCollection organizations = regex.Matches(Names);

            foreach (Match organization in organizations)
            {
                GroupCollection groups = organization.Groups;
                string org = groups[0].Value;
                if (org.Length > 1 && !Organizations.Contains(org))
                    Organizations.Add(groups[0].Value);
            }
        }
    }
}
