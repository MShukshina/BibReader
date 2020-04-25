﻿using BibReader.Publications;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibReader.BibReference.TypesOfSourse
{
    public class Book
    {
        string Title;
        string Name;
        string[] Authors;
        string City;
        string Publisher;
        string Pages;
        string Link;
        DateTime Date;
        int Volume;
        int Year;
        string Source;

        Font f = new Font(SystemFonts.DefaultFont, FontStyle.Italic);
        const string Point = ".";
        const string Page = "p. ";
        const string PPage = "pp. ";
        const string CommaSpace = ", ";
        const string Space = " ";
        const string Access = "Accessed on: ";
        const string Num = "no. ";
        const string Vol = "vol. ";
        const string PointSpace = ". ";
        const string DoublePointSpace = ": ";
        const string DoublePoint = ":";
        const string DoubleSlash = "//";
        const string Slash = "/";
        const string URL = "URL: ";
        const string Lparenthesis = "(";
        const string Rparenthesis = ")";
        const string Avaliable = "Avaliable at: ";
        const string DateRus = "дата обращения";
        const string Lpar = "[";
        const string Rpar = "]";
        const string Retrieved = "Retrieved ";
        const string From = "from ";
        const string EtAl = " et. al. ";
        const string In = "in ";
        const string IN = "In";

        public Book(string[] authors, string title, string name, string city, string publisher, int year, int volume, string pages, string link, DateTime date, string source)
        {
            Authors = authors.ToArray();
            Title = title;
            Name = name;
            City = city;
            Publisher = publisher;
            Year = year;
            Pages = source == "Springer Link" ? "XXX-XXX" : pages;
            Volume = volume;
            Link = link;
            Date = date;
            Source = source;
        }

        public Book(LibItem libItem)
        {
            Int32.TryParse(libItem.Year, out int year);
            Int32.TryParse(libItem.Volume, out int volume);

            Authors = AuthorsParser.ParseAuthors(libItem.Authors, libItem.Sourсe);
            Title = libItem.Title;
            Name = libItem.JournalName;
            City = string.Empty;
            Publisher = libItem.Publisher;
            Year = year;
            Pages = libItem.Sourсe == "Springer Link" ? "XXX-XXX" : libItem.Pages;
            Volume = volume;
            Link = string.Empty;
            Date = DateTime.Parse(DateTime.Now.ToShortDateString());
            Source = libItem.Sourсe;
        }

        public void MakeGOST(RichTextBox rtb)
        {
            string result = string.Empty;
            if (Authors.Length < 4)
            {
                result += AuthorsParser.MakeAuthorsForGOST(Authors);
                result += Space;
                result += Title;
            }
            else
            {
                result += Title;
                result += Space + Slash + Space;
                result += AuthorsParser.MakeAuthorsForGOST(Authors);
            }
            result += Space + DoubleSlash + Space;
            result += Name + PointSpace;
            // TODO 
            if (City != string.Empty)
                result += City + DoublePointSpace;
            result += Publisher + CommaSpace;
            result += Year + PointSpace;
            if (Volume != 0)
                result += Space + Vol + Volume + Point;
            result += PPage + Pages + Point;

            if (Link != "")
                result += Space + URL + Link + Space + Lparenthesis + DateRus + DoublePointSpace + Date.ToString("dd.MM.yyyy") + Rparenthesis + Point;
            rtb.Text += result + "\n\n";
        }

        public void MakeHarvard(RichTextBox rtb)
        {
            rtb.Select(rtb.TextLength, 0);
            rtb.SelectedText = AuthorsParser.MakeAuthorsForHarvard(Authors);
            rtb.SelectedText = Space;
            rtb.SelectedText = Lparenthesis + Year + Rparenthesis + PointSpace;
            rtb.SelectedText = Title + PointSpace;
            rtb.Select(rtb.TextLength, 0); rtb.SelectionFont = f;
            rtb.SelectedText = IN + DoublePointSpace + Name + PointSpace;
            rtb.Select(rtb.TextLength, 0); rtb.SelectionFont = SystemFonts.DefaultFont;
            //if (Volume > 0)
            //    rtb.SelectedText += Volume + Point; 
            if (City != string.Empty)
                rtb.SelectedText = City + DoublePoint + Space;
            rtb.SelectedText = Publisher + CommaSpace;
            rtb.SelectedText = Int32.TryParse(Pages, out int a) ? Page : PPage;
            rtb.SelectedText = Pages + Point;
            if (Link != "")
                rtb.SelectedText = Space + Avaliable + Link + Space + Lpar + DateRus + Space + Date.ToString("dd MMM yyyy") + Rpar + Point;
            rtb.SelectedText = "\n\n";
        }

        public void MakeAPA(RichTextBox rtb)
        {
            rtb.Select(rtb.TextLength, 0);
            rtb.SelectedText = AuthorsParser.MakeAuthorsForAPA(Authors);
            rtb.SelectedText = Space;
            rtb.SelectedText = Lparenthesis + Year + Rparenthesis + PointSpace;
            if (Title != string.Empty)
                rtb.SelectedText = Title + PointSpace;
            rtb.Select(rtb.TextLength, 0); rtb.SelectionFont = f;
            rtb.SelectedText = Name + Space;
            rtb.Select(rtb.TextLength, 0); rtb.SelectionFont = SystemFonts.DefaultFont;
            rtb.SelectedText = Lparenthesis;
            if (Volume != 0)
                rtb.SelectedText += Vol + Volume + CommaSpace;
            rtb.SelectedText = Int32.TryParse(Pages, out int a) ? Page : PPage;
            rtb.SelectedText = Pages + Rparenthesis + PointSpace;
            if (City != string.Empty)
                rtb.SelectedText = City + DoublePoint;
            rtb.SelectedText = Publisher + Point;
            if (Link != "")
                rtb.SelectedText = Space + Retrieved + Date.ToString("dd MMMM yyyy") + CommaSpace + From + Link;
            rtb.SelectedText = "\n\n";
        }

        public void MakeIEEE(RichTextBox rtb)
        {
            rtb.Select(rtb.TextLength, 0);
            rtb.SelectedText = AuthorsParser.MakeAuthorsForIEEE(Authors) + CommaSpace;
            rtb.SelectedText = "“" + Title + "”" + CommaSpace + In;
            rtb.Select(rtb.TextLength, 0); rtb.SelectionFont = f;
            rtb.SelectedText = Name + CommaSpace;
            rtb.Select(rtb.TextLength, 0); rtb.SelectionFont = SystemFonts.DefaultFont;
            if (Volume != 0)
                rtb.SelectedText = Vol + Volume + CommaSpace;
            if (City != string.Empty)
                rtb.SelectedText = City + CommaSpace;
            rtb.SelectedText = Publisher + CommaSpace + Year;
           
            if (Pages != "")
            {
                rtb.SelectedText = CommaSpace;
                rtb.SelectedText = Int32.TryParse(Pages, out int a) ? Page : PPage;
                rtb.SelectedText = Pages + Point;
            }
            else
                rtb.SelectedText = Point;
            if (Link != "")
                rtb.SelectedText = Space + Avaliable + Link + Point + Space + Access + Date.ToString("MMM. dd, yyyy.");
            rtb.SelectedText = "\n\n";
        }
    }
}
