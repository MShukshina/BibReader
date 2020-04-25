using System;
using System.Collections.Generic;
using System.Reflection;

namespace BibReader.Publications
{
    public class LibItem
    {
        public string Authors { get; set; }
        public string Doi {get; set; }
        public string Year {get; set; }
        public string Title { get; set; }
        public string JournalName {get; set; }
        public string Volume {get; set; }
        public string Pages {get; set; }
        public string Url {get; set; }
        public string Affiliation {get; set; }
        public string Abstract {get; set; }
        public string Keywords {get; set; }
        public string Publisher {get; set; }
        public string Sourсe {get; set; }
        public string Number {get; set; }
        public string OriginalTitle { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }

        public LibItem(string authors, string doi, string year, string booktitle, string title, string journalName, 
            string volume, string pages, string url, string affiliation, string @abstract, string keywords, string publisher, 
            string sourсe, string number, string originalTitle, string type, string address)
        {
            Authors = authors;
            Doi = doi;
            Year = year;
            Title = title;
            JournalName = journalName;
            Volume = volume;
            Pages = pages;
            Url = url;
            Affiliation = affiliation;
            Abstract = @abstract;
            Keywords = keywords;
            Publisher = publisher;
            Sourсe = sourсe;
            Number = number;
            OriginalTitle = originalTitle;
            Type = type;
            Address = address;
        }

        public LibItem(Dictionary<string, string> dict)
        {
            Authors = dict["authors"];
            Doi = dict["doi"];
            Year = dict["year"];
            Title = dict["title"];
            JournalName = dict["journal"];
            Volume = dict["volume"];
            Pages = dict["pages"];
            Url = dict["url"];
            Affiliation = dict["affiliation"];
            Abstract = dict["abstract"];
            Keywords = dict["keywords"];
            Publisher = dict["publisher"];
            Sourсe = dict["source"];
            Number = dict["number"];
            OriginalTitle = dict["originalTitle"];
            Type = dict["type"];
            Address = dict["address"];
        }

        public bool AbstractIsEmpty => Abstract == string.Empty ? true : false;
        public bool KeywordsIsEmpty => Keywords == string.Empty ? true : false;
        public bool AffiliationIsEmpty => Affiliation == string.Empty ? true : false;

        public PropertyInfo GetProperty(string name)
        {
            return GetType().GetProperty(name.Substring(2));
        }
    }
}
