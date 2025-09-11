namespace GFSUpdaterGUI
{
    partial class GFSUpdaterGUI
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
            txt_OG_GFS = new TextBox();
            btn_OutputDir = new Button();
            btn_OG_GFS = new Button();
            btn_EditedGMD = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            txt_OutputDir = new TextBox();
            btn_GenerateOutput = new Button();
            txt_EditedGMD = new TextBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            gFSModeToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AllowDrop = true;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(txt_OG_GFS, 1, 1);
            tableLayoutPanel1.Controls.Add(btn_OutputDir, 2, 0);
            tableLayoutPanel1.Controls.Add(btn_OG_GFS, 1, 0);
            tableLayoutPanel1.Controls.Add(btn_EditedGMD, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 2, 1);
            tableLayoutPanel1.Controls.Add(txt_EditedGMD, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 28);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Size = new Size(800, 422);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // txt_OG_GFS
            // 
            txt_OG_GFS.BorderStyle = BorderStyle.None;
            txt_OG_GFS.Dock = DockStyle.Fill;
            txt_OG_GFS.Location = new Point(269, 298);
            txt_OG_GFS.Multiline = true;
            txt_OG_GFS.Name = "txt_OG_GFS";
            txt_OG_GFS.ReadOnly = true;
            txt_OG_GFS.Size = new Size(260, 121);
            txt_OG_GFS.TabIndex = 7;
            // 
            // btn_OutputDir
            // 
            btn_OutputDir.AllowDrop = true;
            btn_OutputDir.Dock = DockStyle.Fill;
            btn_OutputDir.Location = new Point(535, 3);
            btn_OutputDir.Name = "btn_OutputDir";
            btn_OutputDir.Size = new Size(262, 289);
            btn_OutputDir.TabIndex = 2;
            btn_OutputDir.Text = "Drag Output Dir";
            btn_OutputDir.UseVisualStyleBackColor = true;
            btn_OutputDir.DragDrop += DragDrop_Dir;
            btn_OutputDir.DragEnter += DragEnter;
            // 
            // btn_OG_GFS
            // 
            btn_OG_GFS.AllowDrop = true;
            btn_OG_GFS.Dock = DockStyle.Fill;
            btn_OG_GFS.Location = new Point(269, 3);
            btn_OG_GFS.Name = "btn_OG_GFS";
            btn_OG_GFS.Size = new Size(260, 289);
            btn_OG_GFS.TabIndex = 1;
            btn_OG_GFS.Text = "Drag OG Field GFS";
            btn_OG_GFS.UseVisualStyleBackColor = true;
            btn_OG_GFS.DragDrop += DragDrop_GFS;
            btn_OG_GFS.DragEnter += DragEnter;
            // 
            // btn_EditedGMD
            // 
            btn_EditedGMD.AllowDrop = true;
            btn_EditedGMD.Dock = DockStyle.Fill;
            btn_EditedGMD.Location = new Point(3, 3);
            btn_EditedGMD.Name = "btn_EditedGMD";
            btn_EditedGMD.Size = new Size(260, 289);
            btn_EditedGMD.TabIndex = 0;
            btn_EditedGMD.Text = "Drag Edited Field GMD";
            btn_EditedGMD.UseVisualStyleBackColor = true;
            btn_EditedGMD.DragDrop += DragDrop_GMD;
            btn_EditedGMD.DragEnter += DragEnter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(txt_OutputDir, 0, 0);
            tableLayoutPanel2.Controls.Add(btn_GenerateOutput, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(535, 298);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(262, 121);
            tableLayoutPanel2.TabIndex = 5;
            // 
            // txt_OutputDir
            // 
            txt_OutputDir.BorderStyle = BorderStyle.None;
            txt_OutputDir.Dock = DockStyle.Fill;
            txt_OutputDir.Location = new Point(3, 3);
            txt_OutputDir.Multiline = true;
            txt_OutputDir.Name = "txt_OutputDir";
            txt_OutputDir.ReadOnly = true;
            txt_OutputDir.Size = new Size(256, 54);
            txt_OutputDir.TabIndex = 8;
            // 
            // btn_GenerateOutput
            // 
            btn_GenerateOutput.AllowDrop = true;
            btn_GenerateOutput.Dock = DockStyle.Fill;
            btn_GenerateOutput.Enabled = false;
            btn_GenerateOutput.Location = new Point(3, 63);
            btn_GenerateOutput.Name = "btn_GenerateOutput";
            btn_GenerateOutput.Size = new Size(256, 55);
            btn_GenerateOutput.TabIndex = 5;
            btn_GenerateOutput.Text = "Generate Output GFS";
            btn_GenerateOutput.UseVisualStyleBackColor = true;
            btn_GenerateOutput.Click += GenerateBtn_Click;
            // 
            // txt_EditedGMD
            // 
            txt_EditedGMD.BorderStyle = BorderStyle.None;
            txt_EditedGMD.Dock = DockStyle.Fill;
            txt_EditedGMD.Location = new Point(3, 298);
            txt_EditedGMD.Multiline = true;
            txt_EditedGMD.Name = "txt_EditedGMD";
            txt_EditedGMD.ReadOnly = true;
            txt_EditedGMD.Size = new Size(260, 121);
            txt_EditedGMD.TabIndex = 6;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, optionsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, loadToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(125, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += Save_Click;
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(125, 26);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += Load_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { gFSModeToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(75, 24);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // gFSModeToolStripMenuItem
            // 
            gFSModeToolStripMenuItem.Checked = true;
            gFSModeToolStripMenuItem.CheckOnClick = true;
            gFSModeToolStripMenuItem.CheckState = CheckState.Checked;
            gFSModeToolStripMenuItem.Name = "gFSModeToolStripMenuItem";
            gFSModeToolStripMenuItem.Size = new Size(224, 26);
            gFSModeToolStripMenuItem.Text = "GFS Mode";
            // 
            // GFSUpdaterGUI
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "GFSUpdaterGUI";
            Text = "GFSUpdaterGUI";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button btn_EditedGMD;
        private Button btn_OutputDir;
        private Button btn_OG_GFS;
        private TextBox txt_OG_GFS;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox txt_OutputDir;
        private Button btn_GenerateOutput;
        private TextBox txt_EditedGMD;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem gFSModeToolStripMenuItem;
    }
}
