using BibReader.ColumnSorting;
using BibReader.Statistic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibReader
{
    public partial class FiltersForm : Form
    {
        public List<string> Years { get; set; } = new List<string>();
        public List<string> Conference { get; set; } = new List<string>();
        public List<string> Types { get; set; } = new List<string>();
        public List<string> Sources { get; set; } = new List<string>();
        public List<string> Journals { get; set; } = new List<string>();
        public List<string> Geography { get; set; } = new List<string>();

        public FiltersForm()
        {
            InitializeComponent();
            InitListViews();
            InitListViewEvent();
        }

        private void InitListViews()
        {
            foreach (TabPage tp in tabControlForStatistic.TabPages)
            {
                tp.Controls.OfType<ListView>().First().CheckBoxes = true;
                tp.Controls.OfType<ListView>().First().Columns.Add("Значения");
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            Filter.Source = lvSourceStat.CheckedItems.Cast<ListViewItem>().Select(item => item.SubItems[0].Text).ToList();
            Filter.Conferences = lvConferenceStat.CheckedItems.Cast<ListViewItem>().Select(item => item.SubItems[0].Text).ToList();
            Filter.Journals = lvJournalStat.CheckedItems.Cast<ListViewItem>().Select(item => item.SubItems[0].Text).ToList();
            Filter.Types = lvTypeStat.CheckedItems.Cast<ListViewItem>().Select(item => item.SubItems[0].Text).ToList();
            Filter.Years = lvYearStat.CheckedItems.Cast<ListViewItem>().Select(item => item.SubItems[0].Text).ToList();
            Filter.Geography = lvGeographyStat.CheckedItems.Cast<ListViewItem>().Select(item => item.SubItems[0].Text).ToList();
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FiltersForm_Load(object sender, EventArgs e)
        {
            lvYearStat.Items.AddRange(Years.Select(item => new ListViewItem(item)).ToArray());
            lvConferenceStat.Items.AddRange(Conference.Select(item => new ListViewItem(item)).ToArray());
            lvTypeStat.Items.AddRange(Types.Select(item => new ListViewItem(item)).ToArray());
            lvSourceStat.Items.AddRange(Sources.Select(item => new ListViewItem(item)).ToArray());
            lvGeographyStat.Items.AddRange(Geography.Select(item => new ListViewItem(item)).ToArray());
            lvJournalStat.Items.AddRange(Journals.Select(item => new ListViewItem(item)).ToArray());

            foreach (TabPage tp in tabControlForStatistic.TabPages)
            {
                tp.Controls.OfType<ListView>().First().AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                tp.Controls.OfType<ListView>().First().Items.Cast<ListViewItem>().ToList().ForEach(
                    item =>
                    item.Checked = Contains(item.ListView.Name, item.SubItems[0].Text)
                );
            }
            
        }

        private void InitListViewEvent()
        {
            tabControlForStatistic.TabPages.Cast<TabPage>()
                .Select(tp => tp.Controls.OfType<ListView>().First())
                .ToList()
                .ForEach(
                    listView => 
                    listView.ColumnClick += new ColumnClickEventHandler(
                        (sender, e) => Sorting.SortingByColumn((ListView)sender, e.Column)
                    )
                );
        }

        private bool Contains(string name, string text)
        {
            switch(name)
            {
                case "lvConferenceStat":
                    return Filter.Conferences.Contains(text);
                case "lvGeographyStat":
                    return Filter.Geography.Contains(text);
                case "lvJournalStat":
                    return Filter.Journals.Contains(text);
                case "lvSourceStat":
                    return Filter.Source.Contains(text);
                case "lvTypeStat":
                    return Filter.Types.Contains(text);
                case "lvYearStat":
                    return Filter.Years.Contains(text);
                default:
                    return false;
            }
        }
    }
}
