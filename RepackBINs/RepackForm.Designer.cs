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
            tableLayoutPanel1 = new TableLayoutPanel();
            btn_Repack = new Button();
            btn_SelectAll = new Button();
            checkedListBox_Areas = new CheckedListBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(btn_Repack, 1, 1);
            tableLayoutPanel1.Controls.Add(btn_SelectAll, 0, 1);
            tableLayoutPanel1.Controls.Add(checkedListBox_Areas, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.Size = new Size(356, 428);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_Repack
            // 
            btn_Repack.Dock = DockStyle.Fill;
            btn_Repack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn_Repack.Location = new Point(181, 388);
            btn_Repack.Name = "btn_Repack";
            btn_Repack.Size = new Size(172, 37);
            btn_Repack.TabIndex = 0;
            btn_Repack.Text = "Repack Selected";
            btn_Repack.UseVisualStyleBackColor = true;
            btn_Repack.Click += Repack_Click;
            // 
            // btn_SelectAll
            // 
            btn_SelectAll.Dock = DockStyle.Fill;
            btn_SelectAll.Font = new Font("Segoe UI", 9F);
            btn_SelectAll.Location = new Point(3, 388);
            btn_SelectAll.Name = "btn_SelectAll";
            btn_SelectAll.Size = new Size(172, 37);
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
            checkedListBox_Areas.Size = new Size(350, 379);
            checkedListBox_Areas.TabIndex = 2;
            // 
            // RepackForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(356, 428);
            Controls.Add(tableLayoutPanel1);
            Name = "RepackForm";
            Text = "RepackForm";
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button btn_Repack;
        private Button btn_SelectAll;
        private CheckedListBox checkedListBox_Areas;
    }
}