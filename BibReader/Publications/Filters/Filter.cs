using BibReader.Publications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibReader
{
    class Filter
    {
        public static List<string> Years = new List<string>();
        public static List<string> Types = new List<string>();
        public static List<string> Journals = new List<string>();
        public static List<string> Conferences = new List<string>();
        public static List<string> Geography = new List<string>();
        public static List<string> Source = new List<string>();

        public static List<LibItem> FilterOut(List<LibItem> libItems)
        {
            return libItems
                .Where(item => 
                    Years.Contains(item.Year) 
                    && Types.Contains(item.Type) 
                    && Source.Contains(item.Sourсe) 
                    && (item.Type == "journal" && Journals.Contains(item.JournalName) ||
                    item.Type == "conference" && Conferences.Contains(item.JournalName) ||
                    item.Type == "book")
                     && (Geography.Count == 0 || Geography.Contains(item.Affiliation, new GeographyComparer()))
                )
                .ToList();
        }

        public static void Clear()
        {
            Years = new List<string>();
            Types = new List<string>();
            Journals = new List<string>();
            Conferences = new List<string>();
            Geography = new List<string>();
            Source = new List<string>();
        }

        private class GeographyComparer : IEqualityComparer<string>
        {
            private bool IsWoS;

            public bool Equals(string x, string y)
            {
                string affiliation = y;
                if (affiliation != string.Empty)
                {
                    IsWoS = affiliation.Last() == '.';
                    var affs = !IsWoS 
                            ? affiliation.Split(';').ToList()
                            : affiliation
                                .Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(text => text[text.Length - 2] != ' ')
                                .ToList();
                    foreach (var aff in affs)
                    {
                        var infoArray = aff.Split(',');
                        if (x == infoArray.Last())
                            return true;
                    }
                    return false;
                }
                return true;
            }

            public int GetHashCode(string obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
