namespace RepackBINs
{
    partial class RepackForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepackForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            btn_Repack = new Button();
            btn_SelectAll = new Button();
            checkedListBox_Areas = new CheckedListBox();
            chk_ShrinkNewTex = new CheckBox();
            chk_ShrinkAllTex = new CheckBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(chk_ShrinkAllTex, 1, 2);
            tableLayoutPanel1.Controls.Add(btn_Repack, 1, 1);
            tableLayoutPanel1.Controls.Add(btn_SelectAll, 0, 1);
            tableLayoutPanel1.Controls.Add(checkedListBox_Areas, 0, 0);
            tableLayoutPanel1.Controls.Add(chk_ShrinkNewTex, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 13F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.Size = new Size(356, 428);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_Repack
            // 
            btn_Repack.Dock = DockStyle.Fill;
            btn_Repack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn_Repack.Location = new Point(181, 345);
            btn_Repack.Name = "btn_Repack";
            btn_Repack.Size = new Size(172, 49);
            btn_Repack.TabIndex = 0;
            btn_Repack.Text = "Repack Selected";
            btn_Repack.UseVisualStyleBackColor = true;
            btn_Repack.Click += Repack_Click;
            // 
            // btn_SelectAll
            // 
            btn_SelectAll.Dock = DockStyle.Fill;
            btn_SelectAll.Font = new Font("Segoe UI", 9F);
            btn_SelectAll.Location = new Point(3, 345);
            btn_SelectAll.Name = "btn_SelectAll";
            btn_SelectAll.Size = new Size(172, 49);
            btn_SelectAll.TabIndex = 1;
            btn_SelectAll.Text = "Select All/None";
            btn_SelectAll.UseVisualStyleBackColor = true;
            btn_SelectAll.Click += SelectAll_Click;
            // 
            // checkedListBox_Areas
            // 
            tableLayoutPanel1.SetColumnSpan(checkedListBox_Areas, 2);
            checkedListBox_Areas.Dock = DockStyle.Fill;
            checkedListBox_Areas.FormattingEnabled = true;
            checkedListBox_Areas.Location = new Point(3, 3);
            checkedListBox_Areas.Name = "checkedListBox_Areas";
            checkedListBox_Areas.Size = new Size(350, 336);
            checkedListBox_Areas.TabIndex = 2;
            // 
            // chk_ShrinkNewTex
            // 
            chk_ShrinkNewTex.AutoSize = true;
            chk_ShrinkNewTex.Location = new Point(3, 400);
            chk_ShrinkNewTex.Name = "chk_ShrinkNewTex";
            chk_ShrinkNewTex.Size = new Size(131, 24);
            chk_ShrinkNewTex.TabIndex = 3;
            chk_ShrinkNewTex.Text = "Shrink New Tex";
            chk_ShrinkNewTex.UseVisualStyleBackColor = true;
            // 
            // chk_ShrinkAllTex
            // 
            chk_ShrinkAllTex.AutoSize = true;
            chk_ShrinkAllTex.Location = new Point(181, 400);
            chk_ShrinkAllTex.Name = "chk_ShrinkAllTex";
            chk_ShrinkAllTex.Size = new Size(119, 24);
            chk_ShrinkAllTex.TabIndex = 4;
            chk_ShrinkAllTex.Text = "Shrink All Tex";
            chk_ShrinkAllTex.UseVisualStyleBackColor = true;
            // 
            // RepackForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(356, 428);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "RepackForm";
            Text = "RepackForm";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button btn_Repack;
        private Button btn_SelectAll;
        private CheckedListBox checkedListBox_Areas;
        private CheckBox chk_ShrinkNewTex;
        private CheckBox chk_ShrinkAllTex;
    }
}