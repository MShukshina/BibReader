﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using BibReader.Corpuses;
using BibReader.Statistic;
using BibReader.Saver;
using BibReader.ColumnSorting;
using BibReader.Readers;
using BibReader.Publications;
using BibReader.BibReference.TypesOfSourse;
using System.Reflection;
using Collection = BibReader.Finder.Collection.Collection;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using BibReader.Readers.ELibraryScraper;
using BibReader.Analysis;

namespace BibReader
{
    public partial class MainForm : Form
    {
        List<LibItem> deletedLibItems = new List<LibItem>();
        List<LibItem> libItems = new List<LibItem>();
        string lastOpenedFileName = string.Empty;
        Finder.Finder finder = new Finder.Finder();
        Log.Log log = new Log.Log();

        private StreamReader[] GetStreamReaders()
        {
            using (var opd = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Все файлы (*.bib; *.csv; *.html; *.htm; *.txt;)|*.bib; *.csv; *.html; *.htm; *.txt;|Файлы bib (*.bib)|*.bib;|Файлы csv (*.csv)|*.csv;|Файлы html, htm (*.html; *.htm)|*.html; *.htm;|Файлы txt (*.txt)|*.txt;"
            })
            {
                if (opd.ShowDialog() == DialogResult.OK)
                {
                    var streamReaders = new StreamReader[opd.FileNames.Length];
                    for (var i = 0; i < opd.FileNames.Length; i++)
                    {
                        if (opd.FileNames[i].Substring(opd.FileNames[i].LastIndexOf(".") + 1) == "csv")
                        {
                            var cv = new CsvConverter();
                            cv.convertFile(@opd.FileNames[i], @"ConverterFile" + i + ".bib");
                            var reader = new StreamReader(@"ConverterFile" + i + ".bib");
                            streamReaders[i] = reader;
                        }
                        else if (opd.FileNames[i].Substring(opd.FileNames[i].LastIndexOf(".") + 1) == "txt")
                        {
                            @opd.FileNames[i] = Path.ChangeExtension(@opd.FileNames[i], "bin");
                            var reader = new StreamReader(@opd.FileNames[i]);
                            streamReaders[i] = reader;
                        }
                        else if (opd.FileNames[i].Substring(opd.FileNames[i].LastIndexOf(".") + 1) == "html" || 
                                 opd.FileNames[i].Substring(opd.FileNames[i].LastIndexOf(".") + 1) == "htm")
                        {
                            var scraper = new Scraper();
                            scraper.readFile(@opd.FileNames[i], @"ConverterFile" + i + ".bib");
                            var reader = new StreamReader(@"ConverterFile" + i + ".bib");
                            streamReaders[i] = reader;
                        }
                        else
                        {
                            var reader = new StreamReader(opd.FileNames[i]);
                            streamReaders[i] = reader;
                        }
                    }
                    lastOpenedFileName = opd.FileNames.Last();
                    return streamReaders;

                }
                return null;
            }
        }

        private void InitListViewItems()
        {
            lvLibItems.Columns.Add("Название");
            lvLibItems.Columns.Add("Авторы");
            lvLibItems.Columns[0].Width = lvLibItems.Width / 2;
            lvLibItems.Columns[1].Width = lvLibItems.Width / 2;
        }

        private void InitListViewEvent()
        {
            var lists = Controls.OfType<ListView>();
            var tps = tabControlForStatistic.TabPages;
            var listOfTables = new List<ListView>();
            foreach (TabPage tp in tps)
                listOfTables.Add(tp.Controls.OfType<ListView>().First());
                listOfTables.AddRange(lists);
                listOfTables.ForEach(listView => listView.ColumnClick += new ColumnClickEventHandler(
                    (sender, e) => Sorting.SortingByColumn((ListView)sender, e.Column))
                    );
        }

        private void InitTextBoxTextChangedEvent()
        {
            var textBoxes = tabControl.TabPages["tpData"].Controls.OfType<TextBox>();
            foreach (var tb in textBoxes)
                tb.TextChanged += TextBoxTextChanged;
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            PropertyInfo info = null;
            if (lvLibItems.Items.Count != 0)
                info = ((LibItem)lvLibItems.SelectedItems[0].Tag).GetProperty(((TextBox)sender).Name);
            if (info != null)
                info.SetValue((LibItem)lvLibItems.SelectedItems[0].Tag, ((TextBox)sender).Text);
        }

        public MainForm()
        {
            InitializeComponent();
            InitListViewEvent();
            InitTextBoxTextChangedEvent();
            InitListViewItems();
            btFirst.Enabled = false;
            btUnique.Enabled = false;
            btRelevance.Enabled = false;
            cbBibStyles.SelectedIndex = 0;
            cbSearchCriterion.SelectedIndex = 0;
            enabledEditProperties(false);
        }

        private void AddLibItemsInLvItems()
        {
            lvLibItems.Items.Clear();
            foreach (var item in Filter.FilterOut(libItems))
            {
                var lvItem = new ListViewItem(new string[]
                {
                    item.Title,
                    item.Authors,
                });

                lvItem.Tag = item;
                lvLibItems.Items.Add(lvItem);
            }
            lvLibItems.Sorting = SortOrder.Ascending;
            lvLibItems.Sort();
            if (lvLibItems.Items.Count != 0)
            {
                lvLibItems.Items[0].Selected = true;
                lbCurrSelectedItem.Text = $"1/{lvLibItems.Items.Count}";
            }
        }

        private void UniqueTitles()
        {
            var libItemsCount = lvLibItems.Items.Count;
            double step = libItemsCount / 100;
            pbLoadUniqueData.Step = (int)step;

            var time = DateTime.Now;
            log.Write($"{ time.ToString() }");
            log.Write($"> Find unique where libItems count = {lvLibItems.Items.Count} ");
          
            var unique = new Unique(libItems);

            var uniqueItems = unique.GetUnique();
            deletedLibItems.AddRange(libItems.Except(uniqueItems).ToList());
            libItems = uniqueItems;
            LoadLibItems();

            log.Write($"{ (DateTime.Now - time).TotalSeconds.ToString() } sec.");
            log.Write("____________________");

            pbLoadUniqueData.Value = 100;
            MessageBox.Show("Готово!");
            pbLoadUniqueData.Value = 0;
        }

        private void RelevanceData()
        {
            var libItemsCount = lvLibItems.Items.Count;
            double step = libItemsCount / 100;

            var time = DateTime.Now;
            log.Write($"{ time.ToString() }");
            log.Write($"> Find relevance where libItems count = {lvLibItems.Items.Count} ");

            var relevance = new Relevance(libItems);
            var relevanceItems = relevance.GetRelevance();
            deletedLibItems.AddRange(libItems.Except(relevanceItems).ToList());
            libItems = relevanceItems;
            LoadLibItems();

            log.Write($"{ (DateTime.Now - time).TotalSeconds.ToString() } sec.");
            log.Write("____________________");

            pbLoadUniqueData.Value = 100;
            MessageBox.Show("Готово!");
            pbLoadUniqueData.Value = 0;
        }

        private void LoadLibItems()
        {
            lvLibItems.Items.Clear();
            AddLibItemsInLvItems();
        }

        private void enabledEditProperties(bool enabled)
        {
            btEditAuthors.Enabled = enabled;
            btEditTitle.Enabled = enabled;
            btEditAbstract.Enabled = enabled;
            btEditJournal.Enabled = enabled;
            btEditYear.Enabled = enabled;
            btEditVolume.Enabled = enabled;
            btEditPublisher.Enabled = enabled;
            btEditNumber.Enabled = enabled;
            btEditPages.Enabled = enabled;
            btEditDoi.Enabled = enabled;
            btEditUrl.Enabled = enabled;
            btEditAffiliation.Enabled = enabled;
            btEditKeywords.Enabled = enabled;
            btEditSource.Enabled = enabled;
             

        }

        private void lvItems_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                var item = (LibItem)((ListView)sender).SelectedItems[0].Tag;
                tbAbstract.Text = item.Abstract;
                tbAffiliation.Text = item.Affiliation;
                tbAuthors.Text = item.Authors;
                tbDoi.Text = item.Doi;
                tbJournalName.Text = item.JournalName;
                tbKeywords.Text = item.Keywords;
                tbNumber.Text = item.Number;
                tbPages.Text = item.Pages;
                tbPublisher.Text = item.Publisher;
                tbSourсe.Text = item.Sourсe;
                tbTitle.Text = item.Title;
                tbUrl.Text = item.Url;
                tbVolume.Text = item.Volume;
                tbYear.Text = item.Year;
                lbCurrSelectedItem.Text = $"{lvLibItems.SelectedIndices[0] + 1}/{lvLibItems.Items.Count}";
                enabledEditProperties(true);
            } else
            {
                enabledEditProperties(false);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var univReader = new UniversalBibReader();
            var readers = GetStreamReaders();
            if (readers != null)
            {
                libItems.Clear();
                deletedLibItems.Clear();
                libItems.AddRange(univReader.Read(readers));
                LoadFilters();
                var time = DateTime.Now;
                log.Write($"{ time.ToString() }");
                log.Write($"> Open file");
                finder = new Finder.Finder();
                LoadLibItems();
                log.Write($"> Add new LibItem(s): count = { lvLibItems.Items.Count }");
                log.Write($"{ (DateTime.Now - time).TotalSeconds.ToString() } sec.");
                log.Write("____________________");

                toolStripStatusLabel1.Text = "Last opened file name: " + lastOpenedFileName;
                labelFindedItemsCount.Text = string.Empty;
                btFirst.Enabled = false;
                btUnique.Enabled = true;
                btRelevance.Enabled = false;
                добавитьToolStripMenuItem.Enabled = true;
                UpdateUI();
            }
        }

        private void LoadFilters()
        {
            UpdateStatistic();
            Filter.Clear();
            Filter.Conferences.AddRange(Stat.Conference.Keys.Select(key => key));
            Filter.Years.AddRange(Stat.Years.Keys.Select(key => key));
            Filter.Geography.AddRange(Stat.Geography.Keys.Select(key => key));
            Filter.Journals.AddRange(Stat.Journal.Keys.Select(key => key));
            Filter.Types.AddRange(Stat.Types.Keys.Select(key => key));
            Filter.Source.AddRange(Stat.Sources.Keys.Select(key => key));
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var univReader = new UniversalBibReader();
            var reader = GetStreamReaders();
            if (reader != null)
            {
                List<LibItem> newitems;
                var time = DateTime.Now;
                log.Write($"{ time.ToString() }");
                log.Write($"> Add new LibItem(s): count = { (newitems = univReader.Read(reader)).Count }");
                libItems.AddRange(newitems);
                LoadFilters();
                AddLibItemsInLvItems();
                log.Write($"{ (DateTime.Now - time).TotalSeconds.ToString() } sec.");
                log.Write("____________________");

                labelFindedItemsCount.Text = string.Empty;
                btFirst.Enabled = false;
                btUnique.Enabled = true;
                btRelevance.Enabled = false;
                UpdateUI();
            }
        }

        private void MakeBibRef()
        {
            foreach (ListViewItem item in lvLibItems.Items)
            {
                var libItem = (LibItem)item.Tag;
                switch (((LibItem)item.Tag).Type)
                {
                    case "conference":
                        var conference = new Conference(libItem);
                        if (cbBibStyles.Text == "APA")
                            conference.MakeAPA(rtbBib);
                        else if (cbBibStyles.Text == "Harvard")
                            conference.MakeHarvard(rtbBib);
                        else if (cbBibStyles.Text == "IEEE")
                            conference.MakeIEEE(rtbBib);
                        else
                            conference.MakeGOST(rtbBib);
                        break;

                    case "book":
                        {
                            var book = new Book(libItem);
                            if (cbBibStyles.Text == "APA")
                                book.MakeAPA(rtbBib);
                            else if (cbBibStyles.Text == "Harvard")
                                book.MakeHarvard(rtbBib);
                            else if (cbBibStyles.Text == "IEEE")
                                book.MakeIEEE(rtbBib);
                            else
                                book.MakeGOST(rtbBib);
                            break;
                        }

                    case "journal":
                        var journal = new Journal(libItem);
                        if (cbBibStyles.Text == "APA")
                            journal.MakeAPA(rtbBib);
                        else if (cbBibStyles.Text == "Harvard")
                            journal.MakeHarvard(rtbBib);
                        else if (cbBibStyles.Text == "IEEE")
                            journal.MakeIEEE(rtbBib);
                        else
                            journal.MakeGOST(rtbBib);
                        break;
                }
            }
        }

        private void btFirst_Click(object sender, EventArgs e)
        {
            libItems.AddRange(deletedLibItems);// = lvLibItems.Items.Cast<ListViewItem>().Select(item => (LibItem)item.Tag).ToList();
            deletedLibItems.Clear();
            LoadFilters();
            LoadLibItems();
            btUnique.Enabled = true;
            btFirst.Enabled = false;
            добавитьToolStripMenuItem.Enabled = true;
            UpdateUI();
        }

        private void btUnique_Click(object sender, EventArgs e)
        {
            UniqueTitles();
            btRelevance.Enabled = true;
            btUnique.Enabled = false;
            добавитьToolStripMenuItem.Enabled = false;
            UpdateUI();
        }

        private void btRelevance_Click(object sender, EventArgs e)
        {
            RelevanceData();
            btFirst.Enabled = true;
            btRelevance.Enabled = false;
            UpdateUI();
        }

        private void UpdateStatistic()
        {
            Stat.Corpus corpus = Stat.Corpus.First;
            if (btRelevance.Enabled)
                corpus = Stat.Corpus.Unique;
            if (btFirst.Enabled)
                corpus = Stat.Corpus.Relevance;

            Stat.CalculateStatistic(libItems, corpus);
            FormStatistic.LoadSourseStatistic(lvSourceStatistic);
            FormStatistic.LoadYearStatistic(lvYearStatistic);
            FormStatistic.LoadTypeStatistic(lvTypeOfDoc);
            FormStatistic.LoadJournalStatistic(lvJournalStat);
            FormStatistic.LoadGeographyStatistic(lvGeography);
            FormStatistic.LoadConferenceStatistic(lvConferenceStat);
        }

        private void UpdateUI()
        {
            UpdateStatistic();
            UpdateBibReference();
            SelectFstLibItem();
        }

        private void SelectFstLibItem()
        {
            if (lvLibItems.Items.Count != 0)
            {
                lvLibItems.Items[0].Selected = true;
                lbCurrSelectedItem.Text = $"1/{lvLibItems.Items.Count}";
            }
            else
            {
                var textBoxes = tabControl.TabPages["tpData"].Controls.OfType<TextBox>();
                foreach (var tb in textBoxes)
                    tb.Text = string.Empty;
                lbCurrSelectedItem.Text = $"0/{lvLibItems.Items.Count}";
            }
        }

        private void UpdateBibReference()
        {
            rtbBib.Text = string.Empty;
            try
            {
                MakeBibRef();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvItems_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (lvLibItems.FocusedItem.Bounds.Contains(e.Location) == true)
                {

                }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            lvLibItems.SelectedItems[0].Remove();
            SelectFstLibItem();
        }

        private void tbTitle_TextChanged(object sender, EventArgs e)
        {
            if (lvLibItems.Items.Count != 0)
            {
                ((LibItem)lvLibItems.SelectedItems[0].Tag).Title = tbTitle.Text;
                lvLibItems.SelectedItems[0].SubItems[0].Text = tbTitle.Text;
            }
        }

        private void tbAuthors_TextChanged(object sender, EventArgs e)
        {
            if (lvLibItems.Items.Count != 0)
            {
                ((LibItem)lvLibItems.SelectedItems[0].Tag).Authors = tbAuthors.Text;
                lvLibItems.SelectedItems[0].SubItems[1].Text = tbAuthors.Text;
            }
        }

        private int GetNextIndex(Func<List<int>, int, int> func)
        {
            List<int> indexes;
            switch (cbSearchCriterion.SelectedIndex)
            {
                case 0:
                    indexes = Collection.MakeListOfIndexes(tbFind.Text, lvLibItems, 0);
                    break;
                case 1:
                    indexes = Collection.MakeListOfIndexes(
                            tbFind.Text,
                            lvLibItems.Items.Cast<ListViewItem>().Select(item => ((LibItem)item.Tag).Abstract).ToList()
                    );
                    break;
                case 2:
                    indexes = Collection.MakeListOfIndexes(tbFind.Text, lvLibItems, 1);
                    break;
                case 3:
                    indexes = Collection.MakeListOfIndexes(
                            tbFind.Text,
                            lvLibItems.Items.Cast<ListViewItem>().Select(item => ((LibItem)item.Tag).Publisher).ToList()
                    );
                    break;
                case 4:
                    indexes = Collection.MakeListOfIndexes(
                            tbFind.Text,
                            lvLibItems.Items.Cast<ListViewItem>().Select(item => ((LibItem)item.Tag).Keywords).ToList()
                    );
                    break;
                case 5:
                    indexes = Collection.MakeListOfIndexes(
                            tbFind.Text,
                            lvLibItems.Items.Cast<ListViewItem>().Select(item => ((LibItem)item.Tag).Sourсe).ToList()
                    );
                    break;
                default:
                    return 0;
            }
            labelFindedItemsCount.Text = indexes.Count.ToString();
            return finder.GetIndex(indexes, func);
        }

        private void btNextFindedLibItem_Click(object sender, EventArgs e) =>
            Collection.SelectItem(lvLibItems, GetNextIndex(Finder.Functions.Next));

        private void btPrevFindedLibItem_Click(object sender, EventArgs e) =>
            Collection.SelectItem(lvLibItems, GetNextIndex(Finder.Functions.Prev));

        private void btPrintBib_Click(object sender, EventArgs e)
        {
            rtbBib.Text = string.Empty;
            MakeBibRef();
        }

        private void названияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string titles = string.Join("\r\n",
                libItems
                .Where(item => item.Title != string.Empty)
                .Select(item => item.Title)
                );
            var form = new ClassificationForm() { Info = titles };
            form.Show();
        }

        private void ключевыеСловаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string keywords = string.Join("\r\n", 
                libItems
                .Where(item => item.Keywords != string.Empty)
                .Select(item => item.Keywords)
                );
            var form = new ClassificationForm() { Info = keywords };
            form.Show();
        }

        private void аннотацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string abstract_ = string.Join("\r\n", 
                libItems
                .Where(item => item.Abstract != string.Empty)
                .Select(item => item.Abstract)
                );
            var form = new ClassificationForm() { Info = abstract_ };
            form.Show();
        }

        private void cbSearchCriterion_SelectedIndexChanged(object sender, EventArgs e) => finder = new Finder.Finder();

        private void btSaveStatistic_Click(object sender, EventArgs e) => ExcelSaver.Save(GetStatisticListViews());

        private void btSaveBibRef_Click(object sender, EventArgs e) => DocSaver.Save(rtbBib);

        private void корпусДокументовToolStripMenuItem_Click(object sender, EventArgs e) => MyBibFormat.Save(libItems);

        private void библОписанияToolStripMenuItem_Click(object sender, EventArgs e) => DocSaver.Save(rtbBib);

        private void статистикуToolStripMenuItem_Click(object sender, EventArgs e) => ExcelSaver.Save(GetStatisticListViews());

        private List<ListView> GetStatisticListViews()
        {
            var listOfTables = new List<ListView>();
            foreach (TabPage tp in tabControlForStatistic.TabPages)
                listOfTables.Add(tp.Controls.OfType<ListView>().First());
            return listOfTables;
        }

        private void фильтрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FiltersForm()
            {
                Geography = Stat.Geography.Keys.ToList(),
                Sources = Stat.Sources.Keys.ToList(),
                Types = Stat.Types.Keys.ToList(),
                Journals = Stat.Journal.Keys.ToList(),
                Years = Stat.Years.Keys.ToList(),
                Conference = Stat.Conference.Keys.ToList()
            })
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadLibItems();
                }
            }
        }

        private void OpenDiagram(Chart chart, string[] xValues, int[] yValues, ListView lv)
        {
            chart.Series.Clear();

            if (!chart.Visible)
            {
                chart.Visible = true;
                lv.Visible = false;
                chart.BackColor = Color.White;

                chart.BorderlineDashStyle = ChartDashStyle.Solid;
                chart.BorderlineColor = Color.White;

                chart.Series.Add(new Series("ColumnSeries")
                {
                    ChartType = SeriesChartType.Pyramid
                });

                chart.Series["ColumnSeries"].Points.DataBindXY(xValues, yValues);
                chart.Series["ColumnSeries"].IsValueShownAsLabel = true;
                chart.ChartAreas[0].Area3DStyle.Enable3D = true;
                btSaveDiagram.Enabled = true;
                rbPyramid.Checked = true;
                gbTypeDiagram.Enabled = true;
            }
            else
            {
                lv.Visible = true;
                chart.Visible = false;
                rbPyramid.Checked = true;
                btSaveDiagram.Enabled = false;
                gbTypeDiagram.Enabled = false;
            }
        }

        private Chart getChart()
        {
            switch (tabControlForStatistic.SelectedIndex)
            {
                case 0: return chYear;
                case 1: return chSource;
                case 2: return chTypeDocument;
                case 3: return chJournal;
                case 4: return chGeography;
                case 5: return chConference;
                default: return null;
            }
        }

        private ListView getStatistic()
        {
            switch (tabControlForStatistic.SelectedIndex)
            {
                case 0: return lvYearStatistic;
                case 1: return lvSourceStatistic;
                case 2: return lvTypeOfDoc;
                case 3: return lvJournalStat;
                case 4: return lvGeography;
                case 5: return lvConferenceStat;
                default: return null;
            }
        }

        private int[] orderByKeyValue(Dictionary <string, int> statistic)
        {
            return statistic.OrderBy(i => i.Key).Select(item => item.Value).ToArray();
        }

        private string[] orderByKeyKey(Dictionary<string, int> statistic)
        {
            return statistic.OrderBy(i => i.Key).Select(item => item.Key).ToArray();
        }

        private int[] getYValue()
        {
            switch (tabControlForStatistic.SelectedIndex)
            {
                case 0: return orderByKeyValue(Stat.Years);
                case 1: return orderByKeyValue(Stat.Sources);
                case 2: return orderByKeyValue(Stat.Types);
                case 3: return orderByKeyValue(Stat.Journal);
                case 4: return orderByKeyValue(Stat.Geography);
                case 5: return orderByKeyValue(Stat.Conference);
                default: return null;
            }
        }

        private string[] getXValue()
        {
            switch (tabControlForStatistic.SelectedIndex)
            {
                case 0: return orderByKeyKey(Stat.Years);
                case 1: return orderByKeyKey(Stat.Sources);
                case 2: return orderByKeyKey(Stat.Types);
                case 3: return orderByKeyKey(Stat.Journal);
                case 4: return orderByKeyKey(Stat.Geography);
                case 5: return orderByKeyKey(Stat.Conference);
                default: return null;
            }
        }

        private void btOpenDiagram_Click(object sender, EventArgs e)
        {
            int[] yValues = getYValue();
            string[] xValues = getXValue();

            Chart chart = getChart();
            ListView lvStatistic = getStatistic();
            if (chart != null && lvStatistic != null)
            {
                OpenDiagram(chart, xValues, yValues, lvStatistic);
            }
        }

        private void setTypeDiagram(SeriesChartType type)
        {
            switch (type)
            {
                case SeriesChartType.Pie:
                    {
                        rbPie.Checked = true;
                        break;
                    }
                case SeriesChartType.Bar:
                    {
                        rbBar.Checked = true;
                        break;
                    }
                case SeriesChartType.Pyramid:
                    {
                        rbPyramid.Checked = true;
                        break;
                    }
                case SeriesChartType.Doughnut:
                    {
                        rbDoughnut.Checked = true;
                        break;
                    }
            }
        }

        private void tabControlForStatistic_SelectedIndexChanged(Object sender, EventArgs e)
        {
            Chart chart = getChart();
            if (chart != null)
            {
                if (chart.Visible)
                {
                    btSaveDiagram.Enabled = true;
                    gbTypeDiagram.Enabled = true;
                    setTypeDiagram(chart.Series[0].ChartType);
                }
                else
                {
                    rbPyramid.Checked = true;
                    btSaveDiagram.Enabled = false;
                    gbTypeDiagram.Enabled = false;
                }
            }
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                открытьToolStripMenuItem_Click(sender, e);
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                добавитьToolStripMenuItem_Click(sender, e);
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                e.SuppressKeyPress = true;
            }
        }

        private void saveDiagram(Chart chart)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "png files (*.png)|*.png|all files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                chart.SaveImage(saveFileDialog.FileName, ChartImageFormat.Png);
            }          
        }

        private void btSaveDiagram_Click(object sender, EventArgs e)
        {
            Chart chart = getChart();
            if (chart != null)
            {
                saveDiagram(chYear);
            }
        }

        private void rbPie_CheckedChanged(object sender, EventArgs e)
        {
            Chart chart = getChart();
            if (chart != null)
            {
                chart.Series[0].ChartType = SeriesChartType.Pie;
            }
        }

        private void rbDoughnut_CheckedChanged(object sender, EventArgs e)
        {
            Chart chart = getChart();
            if (chart != null)
            {
                chart.Series[0].ChartType = SeriesChartType.Doughnut;
            }
        }

        private void rbBar_CheckedChanged(object sender, EventArgs e)
        {
            Chart chart = getChart();
            if (chart != null)
            {
                chart.Series[0].ChartType = SeriesChartType.Bar;
            }
        }

        private void rbPyramid_CheckedChanged(object sender, EventArgs e)
        {
            Chart chart = getChart();
            if (chart != null)
            {
                chart.Series[0].ChartType = SeriesChartType.Pyramid;
            }
        }

        private void аннотацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string annotations = string.Join(" \r\n",
               libItems
               .Where(item => item.Abstract != string.Empty)
               .Select(item => item.Abstract)
               );
            TextAnalysis.SetConceptsAnalysis(annotations);
            var form = new ContextAnalysis() { Info = annotations };
            form.Show();
        }

        private void названияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string titles = string.Join(" \r\n",
                libItems
                .Where(item => item.Title != string.Empty)
                .Select(item => item.Title)
                );
            TextAnalysis.SetConceptsAnalysis(titles);
            var form = new ContextAnalysis() { Info = titles};
            form.Show();
        }

        private void ключевыеСловаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string keywords = string.Join(" \r\n",
               libItems
               .Where(item => item.Keywords != string.Empty)
               .Select(item => item.Keywords)
               );
            TextAnalysis.SetConceptsAnalysis(keywords);
            var form = new ContextAnalysis() { Info = keywords };
            form.Show();
        }

        private string GetText(StreamReader reader)
        {
            string text = "";
            while (!reader.EndOfStream)
            {
               text += reader.ReadLine();
            }
            return text;
        }

        private void открытьДокументToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "";
            var readers = GetStreamReaders();

            if (readers != null)
            { 
                if (readers != null)
                {
                    foreach (var reader in readers)
                        text += GetText(reader);
                }
            }
            var form = new ContextAnalysis() { Info = text };
            form.Show();
        }

        private void setReadOnlyTextBox(TextBox textBox, bool readOnly)
        {
            if (readOnly)
            {
                textBox.ReadOnly = true;
            }
            else
            {
                textBox.ReadOnly = false;
            }
        }

        private void setVisibleButton(Button button, bool visible)
        {
            if (visible)
            {
                button.Visible = true;
            }
            else
            {
                button.Visible = false;
            }
        }

        private void btEditAuthor_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbAuthors, false);
            setVisibleButton(btEditAuthors, false);
            setVisibleButton(btSaveAuthors, true);
        }

        private void btEditTitle_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbTitle, false);
            setVisibleButton(btEditTitle, false);
            setVisibleButton(btSaveTitle, true);
        }

        private void btEditAbstract_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbAbstract, false);
            setVisibleButton(btEditAbstract, false);
            setVisibleButton(btSaveAbstract, true);
        }

        private void btEditJournal_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbJournalName, false);
            setVisibleButton(btEditJournal, false);
            setVisibleButton(btSaveJournal, true);
        }

        private void btEditYear_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbYear, false);
            setVisibleButton(btEditYear, false);
            setVisibleButton(btSaveYear, true);
        }

        private void btEditVolume_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbVolume, false);
            setVisibleButton(btEditVolume, false);
            setVisibleButton(btSaveVolume, true);
        }

        private void btEditPublisher_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbPublisher, false);
            setVisibleButton(btEditPublisher, false);
            setVisibleButton(btSavePublisher, true);
        }

        private void btEditNumber_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbNumber, false);
            setVisibleButton(btEditAuthors, false);
            setVisibleButton(btSaveAuthors, true);
        }

        private void btEditPages_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbPages, false);
            setVisibleButton(btEditPages, false);
            setVisibleButton(btSavePages, true);
        }

        private void btEditDoi_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbDoi, false);
            setVisibleButton(btEditDoi, false);
            setVisibleButton(btSaveDoi, true);
        }

        private void btEditUrl_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbUrl, false);
            setVisibleButton(btEditUrl, false);
            setVisibleButton(btSaveUrl, true);
        }

        private void btEditAffiliation_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbAffiliation, false);
            setVisibleButton(btEditAffiliation, false);
            setVisibleButton(btSaveAffiliation, true);
        }

        private void btEditKeywords_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbKeywords, false);
            setVisibleButton(btEditKeywords, false);
            setVisibleButton(btSaveKeywords, true);
        }

        private void btEditSource_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbSourсe, false);
            setVisibleButton(btEditSource, false);
            setVisibleButton(btSaveSource, true);
        }

        private void btSaveAuthors_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbAuthors, true);
            setVisibleButton(btSaveAuthors, false);
            setVisibleButton(btEditAuthors, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Authors = tbAuthors.Text;
        }

        private void btSaveTitle_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbTitle, true);
            setVisibleButton(btSaveTitle, false);
            setVisibleButton(btEditTitle, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Title = tbTitle.Text;
        }

        private void btSaveAbstract_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbAbstract, true);
            setVisibleButton(btSaveAbstract, false);
            setVisibleButton(btEditAbstract, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Abstract = tbAbstract.Text;
        }

        private void btSaveJournal_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbJournalName, true);
            setVisibleButton(btSaveJournal, false);
            setVisibleButton(btEditJournal, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).JournalName = tbJournalName.Text;
        }

        private void btSaveYear_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbYear, true);
            setVisibleButton(btSaveYear, false);
            setVisibleButton(btEditYear, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Year = tbYear.Text;
        }

        private void btSaveVolume_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbVolume, true);
            setVisibleButton(btSaveVolume, false);
            setVisibleButton(btEditVolume, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Volume = tbVolume.Text;
        }

        private void btSavePublisher_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbPublisher, true);
            setVisibleButton(btSavePublisher, false);
            setVisibleButton(btEditPublisher, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Publisher = tbPublisher.Text;
        }

        private void btSaveNumber_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbNumber, true);
            setVisibleButton(btSaveNumber, false);
            setVisibleButton(btEditNumber, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Number = tbNumber.Text;
        }

        private void btSavePages_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbPages, true);
            setVisibleButton(btSavePages, false);
            setVisibleButton(btEditPages, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Pages = tbPages.Text;
        }

        private void btSaveDoi_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbDoi, true);
            setVisibleButton(btSaveDoi, false);
            setVisibleButton(btEditDoi, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Doi = tbDoi.Text;
        }

        private void btSaveUrl_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbUrl, true);
            setVisibleButton(btSaveUrl, false);
            setVisibleButton(btEditUrl, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Url = tbUrl.Text;
        }

        private void btSaveAffiliation_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbAffiliation, true);
            setVisibleButton(btSaveAffiliation, false);
            setVisibleButton(btEditKeywords, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Affiliation = tbAffiliation.Text;
        }

        private void btSaveKeywords_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbKeywords, true);
            setVisibleButton(btSaveKeywords, false);
            setVisibleButton(btEditKeywords, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Keywords = tbKeywords.Text;
        }

        private void btSaveSource_Click(object sender, EventArgs e)
        {
            setReadOnlyTextBox(tbSourсe, true);
            setVisibleButton(btSaveSource, false);
            setVisibleButton(btEditSource, true);
            ((LibItem)lvLibItems.SelectedItems[0].Tag).Sourсe = tbSourсe.Text;
        }
    }
}
