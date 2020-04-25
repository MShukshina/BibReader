using BibReader.Publications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BibReader.Readers
{
    public class UniversalBibReader
    {
        private string FindSource(string str)
        {
            switch(str.Substring(0, "title".Length))
            {
                case " titl":
                    return "ACM DL";
                case "title":
                    return (str[5] != ' ') ? "IEEE" : "Science Direct";
                case "Title":
                    return "Web of Science";
            }
            return "";
        }

        private void SetSource(string key, string value, string tagString)
        {
            if (key == "title" || key == "Title" || key == "source")
            {
                if (Tags.TagValues["source"] == "")
                    Tags.TagValues["source"] = FindSource(tagString);
                if (key == "source")
                    Tags.TagValues["source"] = value;
            }
        }

        private string PretreatTitle(string title)
        {
            string[] codesForRemove = new string[] {
                @"\&\#38;",
                "amp;#x2014;",
                "{''}"
            };

            foreach (var code in codesForRemove)
            {
                if (title.Contains(code))
                {
                    var index = title.IndexOf(code);
                    title = title.Remove(index, code.Length);
                    if (code == "{''}")
                        title = title.Insert(index, "”");
                }
            }
            return RemoveOriginalTitle(title);
        }

        private bool SetOriginalTitle(string title)
        {
            var template = @".*\[(.+)\].*";
            var regex = new Regex(template);

            Tags.TagValues["originalTitle"] = regex.Match(title).Groups[1].Value;
            return Tags.TagValues["originalTitle"] != "";
        }

        private string RemoveOriginalTitle(string title)
        {
            if (SetOriginalTitle(title))
                title = 
                    title
                    .Remove(
                        title.IndexOf(Tags.TagValues["originalTitle"]) - 1,
                        Tags.TagValues["originalTitle"].Length + 2
                    );
            return title;
        }

        private void FixScienceDirect(string str)
        {
            var template = @"(.+?)\s=\s""(.+?)""";
            var regex = new Regex(template);
           
            var key = regex.Match(str).Groups[1].Value;
            var value = regex.Match(str).Groups[2].Value;
            if (Tags.TagRework.ContainsKey(key) && Tags.TagValues.ContainsKey(Tags.TagRework[key]))
                Tags.TagValues[Tags.TagRework[key]] = value;
        }

        private void SetType(string str) =>
            Tags.TagValues["type"] =
                Tags.TagRework
                [
                    str
                    .Substring(
                        1,
                        str.IndexOf('{') - 1
                    )
                    .ToLower()
                ];

        private bool IsEndOfTag(string currstr) =>
            currstr.Length >= 3 &&
            (currstr.Substring(currstr.Length - 2, 2) == "}," ||
            currstr.Substring(currstr.Length - 3, 3) == "}, " ||
            currstr.Substring(currstr.Length - 2, 2) == "\",");

        private bool IsEndOfLibItem(string currstr) => 
            currstr.Length > 2 && currstr != "}" 
            && currstr.Substring(currstr.Length - 2, 2) != ",}";

        private List<LibItem> GetLibItems(StreamReader reader)
        {
            List<LibItem> Items = new List<LibItem>();
            var template = @"\s?(.+?)\s?=\s?(""|{{|{)(.+?)(""|}}|}),";
            var regex = new Regex(template);
            string tagString = "", newLine = "";
            Tags.NewTags();

            if (reader == null)
                return Items;
            newLine = reader.ReadLine();
            while (newLine == null || newLine == "" || newLine[0] != '@')
            {
                if (newLine == null)
                    return Items;
                newLine = reader.ReadLine();
            }
            SetType(newLine);
            while (!reader.EndOfStream)
            {
                newLine = reader.ReadLine();
                while (newLine == "" || IsEndOfLibItem(newLine))
                {
                    if (newLine != "")
                    {
                        if (newLine[0] != '@')
                            tagString += newLine;
                        else
                            SetType(newLine);

                        if (IsEndOfTag(newLine))
                        {
                            var key = regex.Match(tagString).Groups[1].Value;
                            var value = ClearValue(regex.Match(tagString).Groups[3].Value, key);
                            SetSource(key, value, tagString);
                            if (Tags.TagRework.ContainsKey(key))
                                Tags.TagValues[Tags.TagRework[key]] = value;
                            tagString = "";
                        }
                    }
                    newLine = reader.ReadLine();
                    if (newLine == null)
                        break;
                }
                if (tagString != string.Empty)
                    FixScienceDirect(tagString);
                if (newLine == null)
                    break;
                tagString = string.Empty;
                var newItem = new LibItem(Tags.TagValues);
                Items.Add(newItem);
                Tags.NewTags();
            }
            reader.Close();
            return Items;
        }

        public static string ClearValue(string value, string key)
        {
            key.ToLower();
            if (value != "" && ( key == "author"
                                || key == " author"  
                                || key == "title" 
                                || key == " title" 
                                || key == "pages" 
                                || key == " pages" 
                                || key == "journal" 
                                || key == " journal"
                                || key == "booktitle"
                                || key == " booktitle")
                ) {
                string[] symbolsForReplace = new string[] {
               @"{", @"}", @"\&\#38;", "�" , "®",  @"’", @"“", @"”", @"–", @" ", @"—", @"--",  @"{''}", @"\""{a}", @"\""{o}", @"{\'e}",
                @"\`{o", @"\'{\i}", @"\""{O}", @"\'{u}", @"\'{o}", @"\""{u}", @"\~{n}", @"\^{o}", @"\&", @"\""", @"\'", @"\~", @"\^", @"\`"
            };

                string[] newSymbols = new string[] {
                "", "", "", "", "",
                @"'", @"""", @"""", @"-", @" ", @"-", @"-", @"""", @"a", @"o", @"e",
                @"o", @"i", @"o", @"u", @"o", @"u", @"n", @"o", " ", "", "", "", "", ""
            };

                for (int i = 0; i < symbolsForReplace.Length; i++)
                {
                    value = value.Replace(symbolsForReplace[i], newSymbols[i]);
                }
            }
            return value;
        }

        public List<LibItem> Read(StreamReader[] readers)
        {
            var Items = new List<LibItem>();
            if (readers != null)
                foreach (var reader in readers)
                    Items.AddRange(GetLibItems(reader));
            return Items;
        }
    }
}
