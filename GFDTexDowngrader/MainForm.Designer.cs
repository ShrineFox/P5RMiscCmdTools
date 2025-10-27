namespace GFDTexDowngrader
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            btn_Repack = new Button();
            btn_PathPS3 = new Button();
            btn_PathP5R = new Button();
            txt_PathPS3 = new TextBox();
            txt_PathP5R = new TextBox();
            lbl_PathPS3 = new Label();
            lbl_PathP5R = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.Controls.Add(lbl_PathP5R, 0, 1);
            tableLayoutPanel1.Controls.Add(btn_Repack, 1, 2);
            tableLayoutPanel1.Controls.Add(btn_PathPS3, 2, 0);
            tableLayoutPanel1.Controls.Add(btn_PathP5R, 2, 1);
            tableLayoutPanel1.Controls.Add(txt_PathPS3, 1, 0);
            tableLayoutPanel1.Controls.Add(txt_PathP5R, 1, 1);
            tableLayoutPanel1.Controls.Add(lbl_PathPS3, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(633, 136);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_Repack
            // 
            tableLayoutPanel1.SetColumnSpan(btn_Repack, 2);
            btn_Repack.Dock = DockStyle.Fill;
            btn_Repack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn_Repack.Location = new Point(129, 93);
            btn_Repack.Name = "btn_Repack";
            btn_Repack.Size = new Size(501, 40);
            btn_Repack.TabIndex = 0;
            btn_Repack.Text = "Replace Textures";
            btn_Repack.UseVisualStyleBackColor = true;
            btn_Repack.Click += Repack_Click;
            // 
            // btn_PathPS3
            // 
            btn_PathPS3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btn_PathPS3.Location = new Point(540, 8);
            btn_PathPS3.Name = "btn_PathPS3";
            btn_PathPS3.Size = new Size(90, 29);
            btn_PathPS3.TabIndex = 1;
            btn_PathPS3.Text = ". . .";
            btn_PathPS3.UseVisualStyleBackColor = true;
            btn_PathPS3.Click += PathPS3_Click;
            // 
            // btn_PathP5R
            // 
            btn_PathP5R.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btn_PathP5R.Location = new Point(540, 53);
            btn_PathP5R.Name = "btn_PathP5R";
            btn_PathP5R.Size = new Size(90, 29);
            btn_PathP5R.TabIndex = 2;
            btn_PathP5R.Text = ". . .";
            btn_PathP5R.UseVisualStyleBackColor = true;
            btn_PathP5R.Click += PathP5R_Click;
            // 
            // txt_PathPS3
            // 
            txt_PathPS3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txt_PathPS3.Location = new Point(129, 9);
            txt_PathPS3.Name = "txt_PathPS3";
            txt_PathPS3.Size = new Size(405, 27);
            txt_PathPS3.TabIndex = 3;
            // 
            // txt_PathP5R
            // 
            txt_PathP5R.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txt_PathP5R.Location = new Point(129, 54);
            txt_PathP5R.Name = "txt_PathP5R";
            txt_PathP5R.Size = new Size(405, 27);
            txt_PathP5R.TabIndex = 4;
            // 
            // lbl_PathPS3
            // 
            lbl_PathPS3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lbl_PathPS3.AutoSize = true;
            lbl_PathPS3.Location = new Point(3, 2);
            lbl_PathPS3.Name = "lbl_PathPS3";
            lbl_PathPS3.Size = new Size(120, 40);
            lbl_PathPS3.TabIndex = 5;
            lbl_PathPS3.Text = "PS3 Ext CPKs Path:";
            // 
            // lbl_PathP5R
            // 
            lbl_PathP5R.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lbl_PathP5R.AutoSize = true;
            lbl_PathP5R.Location = new Point(3, 47);
            lbl_PathP5R.Name = "lbl_PathP5R";
            lbl_PathP5R.Size = new Size(120, 40);
            lbl_PathP5R.TabIndex = 6;
            lbl_PathP5R.Text = "P5RPC Ext CPKs Path:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(633, 136);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "GFD Texture Downgrader";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button btn_Repack;
        private Button btn_PathPS3;
        private Button btn_PathP5R;
        private TextBox txt_PathPS3;
        private TextBox txt_PathP5R;
        private Label lbl_PathP5R;
        private Label lbl_PathPS3;
    }
}