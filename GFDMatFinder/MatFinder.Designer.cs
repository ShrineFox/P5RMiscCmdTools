
namespace GFDMatFinder
{
    partial class MatFinder
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            chk_IncludeOnlySelectedVertFlags = new CheckBox();
            chk_IncludeOnlySelectedGeoFlags = new CheckBox();
            checkedListBox_VertFlags = new CheckedListBox();
            checkedListBox_GeoFlags = new CheckedListBox();
            checkedListBox_MatFlags = new CheckedListBox();
            textBox_MatsList = new TextBox();
            btn_FindMats = new Button();
            chk_IncludeOnlySelectedMatFlags = new CheckBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(chk_IncludeOnlySelectedVertFlags, 2, 1);
            tableLayoutPanel1.Controls.Add(chk_IncludeOnlySelectedGeoFlags, 1, 1);
            tableLayoutPanel1.Controls.Add(checkedListBox_VertFlags, 2, 0);
            tableLayoutPanel1.Controls.Add(checkedListBox_GeoFlags, 1, 0);
            tableLayoutPanel1.Controls.Add(checkedListBox_MatFlags, 0, 0);
            tableLayoutPanel1.Controls.Add(textBox_MatsList, 0, 2);
            tableLayoutPanel1.Controls.Add(btn_FindMats, 2, 2);
            tableLayoutPanel1.Controls.Add(chk_IncludeOnlySelectedMatFlags, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // chk_IncludeOnlySelectedVertFlags
            // 
            chk_IncludeOnlySelectedVertFlags.AutoSize = true;
            chk_IncludeOnlySelectedVertFlags.Location = new Point(535, 273);
            chk_IncludeOnlySelectedVertFlags.Name = "chk_IncludeOnlySelectedVertFlags";
            chk_IncludeOnlySelectedVertFlags.Size = new Size(174, 24);
            chk_IncludeOnlySelectedVertFlags.TabIndex = 7;
            chk_IncludeOnlySelectedVertFlags.Text = "Include Only Selected";
            chk_IncludeOnlySelectedVertFlags.UseVisualStyleBackColor = true;
            // 
            // chk_IncludeOnlySelectedGeoFlags
            // 
            chk_IncludeOnlySelectedGeoFlags.AutoSize = true;
            chk_IncludeOnlySelectedGeoFlags.Location = new Point(269, 273);
            chk_IncludeOnlySelectedGeoFlags.Name = "chk_IncludeOnlySelectedGeoFlags";
            chk_IncludeOnlySelectedGeoFlags.Size = new Size(174, 24);
            chk_IncludeOnlySelectedGeoFlags.TabIndex = 6;
            chk_IncludeOnlySelectedGeoFlags.Text = "Include Only Selected";
            chk_IncludeOnlySelectedGeoFlags.UseVisualStyleBackColor = true;
            // 
            // checkedListBox_VertFlags
            // 
            checkedListBox_VertFlags.Dock = DockStyle.Fill;
            checkedListBox_VertFlags.FormattingEnabled = true;
            checkedListBox_VertFlags.Location = new Point(535, 3);
            checkedListBox_VertFlags.Name = "checkedListBox_VertFlags";
            checkedListBox_VertFlags.Size = new Size(262, 264);
            checkedListBox_VertFlags.TabIndex = 2;
            // 
            // checkedListBox_GeoFlags
            // 
            checkedListBox_GeoFlags.Dock = DockStyle.Fill;
            checkedListBox_GeoFlags.FormattingEnabled = true;
            checkedListBox_GeoFlags.Location = new Point(269, 3);
            checkedListBox_GeoFlags.Name = "checkedListBox_GeoFlags";
            checkedListBox_GeoFlags.Size = new Size(260, 264);
            checkedListBox_GeoFlags.TabIndex = 1;
            // 
            // checkedListBox_MatFlags
            // 
            checkedListBox_MatFlags.Dock = DockStyle.Fill;
            checkedListBox_MatFlags.FormattingEnabled = true;
            checkedListBox_MatFlags.Location = new Point(3, 3);
            checkedListBox_MatFlags.Name = "checkedListBox_MatFlags";
            checkedListBox_MatFlags.Size = new Size(260, 264);
            checkedListBox_MatFlags.TabIndex = 0;
            // 
            // textBox_MatsList
            // 
            tableLayoutPanel1.SetColumnSpan(textBox_MatsList, 2);
            textBox_MatsList.Dock = DockStyle.Fill;
            textBox_MatsList.Location = new Point(3, 318);
            textBox_MatsList.Multiline = true;
            textBox_MatsList.Name = "textBox_MatsList";
            textBox_MatsList.ReadOnly = true;
            textBox_MatsList.ScrollBars = ScrollBars.Vertical;
            textBox_MatsList.Size = new Size(526, 129);
            textBox_MatsList.TabIndex = 3;
            // 
            // btn_FindMats
            // 
            btn_FindMats.Dock = DockStyle.Fill;
            btn_FindMats.Location = new Point(535, 318);
            btn_FindMats.Name = "btn_FindMats";
            btn_FindMats.Size = new Size(262, 129);
            btn_FindMats.TabIndex = 4;
            btn_FindMats.Text = "Find Mats";
            btn_FindMats.UseVisualStyleBackColor = true;
            btn_FindMats.Click += FindMats_Click;
            // 
            // chk_IncludeOnlySelectedMatFlags
            // 
            chk_IncludeOnlySelectedMatFlags.AutoSize = true;
            chk_IncludeOnlySelectedMatFlags.Location = new Point(3, 273);
            chk_IncludeOnlySelectedMatFlags.Name = "chk_IncludeOnlySelectedMatFlags";
            chk_IncludeOnlySelectedMatFlags.Size = new Size(174, 24);
            chk_IncludeOnlySelectedMatFlags.TabIndex = 5;
            chk_IncludeOnlySelectedMatFlags.Text = "Include Only Selected";
            chk_IncludeOnlySelectedMatFlags.UseVisualStyleBackColor = true;
            // 
            // MatFinder
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "MatFinder";
            Text = "GFD Material Finder";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }
        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private CheckedListBox checkedListBox_VertFlags;
        private CheckedListBox checkedListBox_GeoFlags;
        private CheckedListBox checkedListBox_MatFlags;
        private TextBox textBox_MatsList;
        private Button btn_FindMats;
        private CheckBox chk_IncludeOnlySelectedVertFlags;
        private CheckBox chk_IncludeOnlySelectedGeoFlags;
        private CheckBox chk_IncludeOnlySelectedMatFlags;
    }
}
