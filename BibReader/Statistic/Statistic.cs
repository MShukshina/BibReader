using BibReader.Publications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibReader.Statistic
{
    public class Stat
    {
        public static Dictionary<string, int> Sources            { get; private set; } = new Dictionary<string, int>();
        public static Dictionary<string, int> SourcesUnique      { get; private set; } = new Dictionary<string, int>();
        public static Dictionary<string, int> SourcesRelevance   { get; private set; } = new Dictionary<string, int>();
        public static Dictionary<string, int> Years              { get; private set; } = new Dictionary<string, int>();
        public static Dictionary<string, int> Types              { get; private set; } = new Dictionary<string, int>();
        public static Dictionary<string, int> Journal            { get; private set; } = new Dictionary<string, int>();
        public static Dictionary<string, int> Conference         { get; private set; } = new Dictionary<string, int>();
        public static Dictionary<string, int> Geography          { get; private set; } = new Dictionary<string, int>();

        public enum Corpus
        {
            First,
            Unique,
            Relevance
        }

        public static void CalculateStatistic(List<LibItem> libItems, Corpus corpus)
        {
            if (corpus == Corpus.First)
            {
                Sources = new Dictionary<string, int>();
                SourcesUnique = new Dictionary<string, int>();
                SourcesRelevance = new Dictionary<string, int>();
            }
            Years = new Dictionary<string, int>();
            Types = new Dictionary<string, int>();
            Journal = new Dictionary<string, int>();
            Conference = new Dictionary<string, int>();
            Geography = new Dictionary<string, int>();

            foreach (var item in libItems)
            {
                SetYearStatistic(item);
                SetTypesStatistic(item);
                if (corpus == Corpus.First)
                    SetSourseStatictic(item);
                if (corpus == Corpus.Unique)
                    SetSourseUniqueStatictic(item);
                if (corpus == Corpus.Relevance)
                    SetSourseRelevanceStatictic(item);
                SetJournalStatistic(item);
                SetConferenceStatistic(item);
                SetGeographyStatistic(item);
            }
        }


        private static void SetGeographyStatistic(LibItem libItem)
        {
            if (libItem.Affiliation != string.Empty)
            {
                List<string> affs = new List<string>();
                if(libItem.Sourсe == "Web of Science")
                {
                    affs = libItem.Affiliation
                        .Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(text => text[text.Length-2] != ' ')
                        .ToList();
                }
                else
                    affs = libItem.Affiliation.Split(';').ToList();
                foreach (var aff in affs)
                {
                    var infoArray = aff.Split(',');

                    if (Geography.ContainsKey(infoArray.Last()))
                        Geography[infoArray.Last()]++;
                    else
                        Geography.Add(infoArray.Last(), 1);
                }
            }
        }

        private static void SetYearStatistic(LibItem libItem)
        {
            if (Years.ContainsKey(libItem.Year))
                Years[libItem.Year]++;
            else
                Years.Add(libItem.Year, 1);
        }

        private static void SetJournalStatistic(LibItem libItem)
        {
            if (libItem.Type == "journal" && libItem.JournalName != string.Empty)
            if (Journal.ContainsKey(libItem.JournalName))
                Journal[libItem.JournalName]++;
            else
                Journal.Add(libItem.JournalName, 1);
        }

        private static void SetConferenceStatistic(LibItem libItem)
        {
            if (libItem.Type == "conference")
            {
                var title = libItem.JournalName;
                if (Conference.ContainsKey(title))
                    Conference[title]++;
                else
                    Conference.Add(title, 1);
            }
        }

        private static void SetSourseStatictic(LibItem libItem)
        {
            if (Sources.ContainsKey(libItem.Sourсe))
                Sources[libItem.Sourсe]++;
            else
            {
                Sources.Add(libItem.Sourсe, 1);
                SourcesUnique.Add(libItem.Sourсe, 0);
                SourcesRelevance.Add(libItem.Sourсe, 0);
            }
        }

        private static void SetSourseUniqueStatictic(LibItem libItem)
        {
            if (SourcesUnique.ContainsKey(libItem.Sourсe))
                SourcesUnique[libItem.Sourсe]++;
            else
                SourcesUnique.Add(libItem.Sourсe, 1);
        }

        private static void SetSourseRelevanceStatictic(LibItem libItem)
        {
            if (SourcesRelevance.ContainsKey(libItem.Sourсe))
                SourcesRelevance[libItem.Sourсe]++;
            else
                SourcesRelevance.Add(libItem.Sourсe, 1);
        }

        private static void SetTypesStatistic(LibItem libItem)
        {
            if (Types.ContainsKey(libItem.Type))
                Types[libItem.Type]++;
            else
                Types.Add(libItem.Type, 1);
        }
    }
}
