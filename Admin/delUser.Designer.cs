
namespace АИС_Автосалон.Admin
{
    partial class delUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(delUser));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.crownComboBox1 = new ReaLTaiizor.Controls.CrownComboBox();
            this.lostCancelButton1 = new ReaLTaiizor.Controls.LostCancelButton();
            this.lostLabel1 = new ReaLTaiizor.Controls.LostLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.crownComboBox1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lostCancelButton1, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.lostLabel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 36);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(827, 190);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // crownComboBox1
            // 
            this.crownComboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.crownComboBox1, 2);
            this.crownComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.crownComboBox1.FormattingEnabled = true;
            this.crownComboBox1.Location = new System.Drawing.Point(23, 78);
            this.crownComboBox1.Name = "crownComboBox1";
            this.crownComboBox1.Size = new System.Drawing.Size(531, 30);
            this.crownComboBox1.TabIndex = 0;
            // 
            // lostCancelButton1
            // 
            this.lostCancelButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lostCancelButton1.BackColor = System.Drawing.Color.Crimson;
            this.lostCancelButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lostCancelButton1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lostCancelButton1.ForeColor = System.Drawing.Color.White;
            this.lostCancelButton1.HoverColor = System.Drawing.Color.IndianRed;
            this.lostCancelButton1.Image = null;
            this.lostCancelButton1.Location = new System.Drawing.Point(560, 78);
            this.lostCancelButton1.Name = "lostCancelButton1";
            this.lostCancelButton1.Size = new System.Drawing.Size(264, 34);
            this.lostCancelButton1.TabIndex = 1;
            this.lostCancelButton1.Text = "Удалить";
            this.lostCancelButton1.Click += new System.EventHandler(this.lostCancelButton1_Click);
            // 
            // lostLabel1
            // 
            this.lostLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lostLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lostLabel1, 3);
            this.lostLabel1.ForeColor = System.Drawing.Color.White;
            this.lostLabel1.Location = new System.Drawing.Point(23, 49);
            this.lostLabel1.Name = "lostLabel1";
            this.lostLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lostLabel1.Size = new System.Drawing.Size(801, 23);
            this.lostLabel1.TabIndex = 2;
            this.lostLabel1.Text = "Выберете пользователя:";
            // 
            // delUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(831, 228);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Image = ((System.Drawing.Image)(resources.GetObject("$this.Image")));
            this.MaximizeBox = false;
            this.Name = "delUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Удаление пользователя";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ReaLTaiizor.Controls.CrownComboBox crownComboBox1;
        private ReaLTaiizor.Controls.LostCancelButton lostCancelButton1;
        private ReaLTaiizor.Controls.LostLabel lostLabel1;
    }
}