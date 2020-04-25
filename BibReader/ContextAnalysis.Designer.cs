namespace BibReader
{
    partial class ContextAnalysis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControlForAnalisys = new System.Windows.Forms.TabControl();
            this.tpSynopsis = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chSynopsis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lvSynopsis = new System.Windows.Forms.ListView();
            this.tbContext = new System.Windows.Forms.TextBox();
            this.tpConcepts = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvConcepts = new System.Windows.Forms.ListView();
            this.tbThesaurus = new System.Windows.Forms.TabPage();
            this.lvThesaurus = new System.Windows.Forms.ListView();
            this.tbNames = new System.Windows.Forms.TabPage();
            this.lvOrganization = new System.Windows.Forms.ListView();
            this.lvPerson = new System.Windows.Forms.ListView();
            this.pGraphic = new System.Windows.Forms.Panel();
            this.tabControlForAnalisys.SuspendLayout();
            this.tpSynopsis.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chSynopsis)).BeginInit();
            this.tpConcepts.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tbThesaurus.SuspendLayout();
            this.tbNames.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlForAnalisys
            // 
            this.tabControlForAnalisys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlForAnalisys.Controls.Add(this.tpSynopsis);
            this.tabControlForAnalisys.Controls.Add(this.tpConcepts);
            this.tabControlForAnalisys.Controls.Add(this.tbThesaurus);
            this.tabControlForAnalisys.Controls.Add(this.tbNames);
            this.tabControlForAnalisys.Location = new System.Drawing.Point(686, 3);
            this.tabControlForAnalisys.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlForAnalisys.Name = "tabControlForAnalisys";
            this.tabControlForAnalisys.SelectedIndex = 0;
            this.tabControlForAnalisys.Size = new System.Drawing.Size(521, 677);
            this.tabControlForAnalisys.TabIndex = 5;
            // 
            // tpSynopsis
            // 
            this.tpSynopsis.Controls.Add(this.panel1);
            this.tpSynopsis.Controls.Add(this.tbContext);
            this.tpSynopsis.Location = new System.Drawing.Point(4, 22);
            this.tpSynopsis.Margin = new System.Windows.Forms.Padding(2);
            this.tpSynopsis.Name = "tpSynopsis";
            this.tpSynopsis.Padding = new System.Windows.Forms.Padding(2);
            this.tpSynopsis.Size = new System.Drawing.Size(513, 651);
            this.tpSynopsis.TabIndex = 0;
            this.tpSynopsis.Text = "Краткий обзор";
            this.tpSynopsis.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chSynopsis);
            this.panel1.Controls.Add(this.lvSynopsis);
            this.panel1.Location = new System.Drawing.Point(4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 206);
            this.panel1.TabIndex = 7;
            // 
            // chSynopsis
            // 
            this.chSynopsis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chSynopsis.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chSynopsis.BackImageTransparentColor = System.Drawing.Color.White;
            this.chSynopsis.BackSecondaryColor = System.Drawing.Color.White;
            chartArea3.Name = "ChartArea1";
            this.chSynopsis.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chSynopsis.Legends.Add(legend3);
            this.chSynopsis.Location = new System.Drawing.Point(155, 13);
            this.chSynopsis.Name = "chSynopsis";
            this.chSynopsis.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chSynopsis.Series.Add(series3);
            this.chSynopsis.Size = new System.Drawing.Size(343, 201);
            this.chSynopsis.TabIndex = 2;
            this.chSynopsis.Text = "Понятия";
            // 
            // lvSynopsis
            // 
            this.lvSynopsis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSynopsis.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvSynopsis.HideSelection = false;
            this.lvSynopsis.Location = new System.Drawing.Point(2, 3);
            this.lvSynopsis.Margin = new System.Windows.Forms.Padding(2);
            this.lvSynopsis.Name = "lvSynopsis";
            this.lvSynopsis.Size = new System.Drawing.Size(148, 198);
            this.lvSynopsis.TabIndex = 0;
            this.lvSynopsis.UseCompatibleStateImageBehavior = false;
            this.lvSynopsis.View = System.Windows.Forms.View.Details;
            this.lvSynopsis.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvSynopsis_DrawColumnHeader);
            this.lvSynopsis.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lvSynopsis_DrawSubItem);
            // 
            // tbContext
            // 
            this.tbContext.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbContext.Location = new System.Drawing.Point(4, 216);
            this.tbContext.Margin = new System.Windows.Forms.Padding(2);
            this.tbContext.Multiline = true;
            this.tbContext.Name = "tbContext";
            this.tbContext.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbContext.Size = new System.Drawing.Size(503, 431);
            this.tbContext.TabIndex = 6;
            // 
            // tpConcepts
            // 
            this.tpConcepts.Controls.Add(this.panel2);
            this.tpConcepts.Location = new System.Drawing.Point(4, 22);
            this.tpConcepts.Margin = new System.Windows.Forms.Padding(2);
            this.tpConcepts.Name = "tpConcepts";
            this.tpConcepts.Padding = new System.Windows.Forms.Padding(2);
            this.tpConcepts.Size = new System.Drawing.Size(513, 651);
            this.tpConcepts.TabIndex = 1;
            this.tpConcepts.Text = "Общее представление";
            this.tpConcepts.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lvConcepts);
            this.panel2.Location = new System.Drawing.Point(4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(505, 473);
            this.panel2.TabIndex = 8;
            // 
            // lvConcepts
            // 
            this.lvConcepts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lvConcepts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvConcepts.HideSelection = false;
            this.lvConcepts.Location = new System.Drawing.Point(2, 3);
            this.lvConcepts.Margin = new System.Windows.Forms.Padding(2);
            this.lvConcepts.Name = "lvConcepts";
            this.lvConcepts.Size = new System.Drawing.Size(499, 465);
            this.lvConcepts.TabIndex = 6;
            this.lvConcepts.UseCompatibleStateImageBehavior = false;
            this.lvConcepts.View = System.Windows.Forms.View.Details;
            this.lvConcepts.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvConcepts_DrawColumnHeader);
            this.lvConcepts.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lvConcepts_DrawSubItem);
            // 
            // tbThesaurus
            // 
            this.tbThesaurus.Controls.Add(this.lvThesaurus);
            this.tbThesaurus.Location = new System.Drawing.Point(4, 22);
            this.tbThesaurus.Margin = new System.Windows.Forms.Padding(2);
            this.tbThesaurus.Name = "tbThesaurus";
            this.tbThesaurus.Padding = new System.Windows.Forms.Padding(2);
            this.tbThesaurus.Size = new System.Drawing.Size(513, 651);
            this.tbThesaurus.TabIndex = 2;
            this.tbThesaurus.Text = "Тезаурус";
            this.tbThesaurus.UseVisualStyleBackColor = true;
            // 
            // lvThesaurus
            // 
            this.lvThesaurus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvThesaurus.HideSelection = false;
            this.lvThesaurus.Location = new System.Drawing.Point(4, 5);
            this.lvThesaurus.Margin = new System.Windows.Forms.Padding(2);
            this.lvThesaurus.Name = "lvThesaurus";
            this.lvThesaurus.Size = new System.Drawing.Size(505, 642);
            this.lvThesaurus.TabIndex = 3;
            this.lvThesaurus.UseCompatibleStateImageBehavior = false;
            this.lvThesaurus.View = System.Windows.Forms.View.Details;
            // 
            // tbNames
            // 
            this.tbNames.Controls.Add(this.lvOrganization);
            this.tbNames.Controls.Add(this.lvPerson);
            this.tbNames.Location = new System.Drawing.Point(4, 22);
            this.tbNames.Name = "tbNames";
            this.tbNames.Size = new System.Drawing.Size(513, 651);
            this.tbNames.TabIndex = 3;
            this.tbNames.Text = "Имена";
            this.tbNames.UseVisualStyleBackColor = true;
            // 
            // lvOrganization
            // 
            this.lvOrganization.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvOrganization.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvOrganization.HideSelection = false;
            this.lvOrganization.Location = new System.Drawing.Point(254, 5);
            this.lvOrganization.Margin = new System.Windows.Forms.Padding(2);
            this.lvOrganization.Name = "lvOrganization";
            this.lvOrganization.Size = new System.Drawing.Size(253, 644);
            this.lvOrganization.TabIndex = 2;
            this.lvOrganization.UseCompatibleStateImageBehavior = false;
            this.lvOrganization.View = System.Windows.Forms.View.Details;
            // 
            // lvPerson
            // 
            this.lvPerson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPerson.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvPerson.HideSelection = false;
            this.lvPerson.Location = new System.Drawing.Point(4, 5);
            this.lvPerson.Margin = new System.Windows.Forms.Padding(2);
            this.lvPerson.Name = "lvPerson";
            this.lvPerson.Size = new System.Drawing.Size(246, 644);
            this.lvPerson.TabIndex = 1;
            this.lvPerson.UseCompatibleStateImageBehavior = false;
            this.lvPerson.View = System.Windows.Forms.View.Details;
            // 
            // pGraphic
            // 
            this.pGraphic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pGraphic.BackColor = System.Drawing.Color.White;
            this.pGraphic.Location = new System.Drawing.Point(3, 3);
            this.pGraphic.Name = "pGraphic";
            this.pGraphic.Size = new System.Drawing.Size(678, 677);
            this.pGraphic.TabIndex = 7;
            // 
            // ContextAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1208, 687);
            this.Controls.Add(this.pGraphic);
            this.Controls.Add(this.tabControlForAnalisys);
            this.MaximumSize = new System.Drawing.Size(1224, 726);
            this.MinimumSize = new System.Drawing.Size(1224, 726);
            this.Name = "ContextAnalysis";
            this.Text = "Контекстный анализ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ContextAnalysis_FormClosed);
            this.Load += new System.EventHandler(this.ContextAnalysis_Load);
            this.Shown += new System.EventHandler(this.ContextAnalysis_Shown);
            this.tabControlForAnalisys.ResumeLayout(false);
            this.tpSynopsis.ResumeLayout(false);
            this.tpSynopsis.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chSynopsis)).EndInit();
            this.tpConcepts.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tbThesaurus.ResumeLayout(false);
            this.tbNames.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControlForAnalisys;
        private System.Windows.Forms.TabPage tpSynopsis;
        private System.Windows.Forms.ListView lvSynopsis;
        private System.Windows.Forms.TabPage tpConcepts;
        private System.Windows.Forms.TabPage tbThesaurus;
        private System.Windows.Forms.ListView lvThesaurus;
        private System.Windows.Forms.TextBox tbContext;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView lvConcepts;
        private System.Windows.Forms.Panel pGraphic;
        private System.Windows.Forms.TabPage tbNames;
        private System.Windows.Forms.ListView lvPerson;
        private System.Windows.Forms.DataVisualization.Charting.Chart chSynopsis;
        private System.Windows.Forms.ListView lvOrganization;
    }
}