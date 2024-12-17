namespace Mibfa.OCR.Service.Designer
{
    partial class Settings
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
            this.Label7 = new System.Windows.Forms.Label();
            this.FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.OK_Button = new System.Windows.Forms.Button();
            this.txtZoneFile = new System.Windows.Forms.TextBox();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.cbProfile = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIndexName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDocumentType = new System.Windows.Forms.TextBox();
            this.txtSearchString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtZoneFileBrowse = new System.Windows.Forms.Button();
            this.chkStripZoneSpaces = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.udZoneLength = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProcessVersionID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProfileVersionID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtProfileID = new System.Windows.Forms.TextBox();
            this.TableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udZoneLength)).BeginInit();
            this.SuspendLayout();
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(58, 197);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(54, 13);
            this.Label7.TabIndex = 40;
            this.Label7.Text = "Zone File:";
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OK_Button.Location = new System.Drawing.Point(3, 3);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(67, 23);
            this.OK_Button.TabIndex = 7;
            this.OK_Button.Text = "OK";
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // txtZoneFile
            // 
            this.txtZoneFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtZoneFile.Location = new System.Drawing.Point(118, 194);
            this.txtZoneFile.Name = "txtZoneFile";
            this.txtZoneFile.Size = new System.Drawing.Size(269, 20);
            this.txtZoneFile.TabIndex = 2;
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(76, 3);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(67, 23);
            this.Cancel_Button.TabIndex = 8;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel1.ColumnCount = 2;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.Controls.Add(this.Cancel_Button, 1, 0);
            this.TableLayoutPanel1.Controls.Add(this.OK_Button, 0, 0);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(280, 282);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 1;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(146, 29);
            this.TableLayoutPanel1.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(59, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 51;
            this.label6.Text = "Profile:";
            // 
            // cbProfile
            // 
            this.cbProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbProfile.FormattingEnabled = true;
            this.cbProfile.Location = new System.Drawing.Point(118, 11);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(308, 21);
            this.cbProfile.TabIndex = 79;
            this.cbProfile.SelectedIndexChanged += new System.EventHandler(this.cbProfile_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 84;
            this.label2.Text = "Index Name:";
            // 
            // txtIndexName
            // 
            this.txtIndexName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIndexName.Location = new System.Drawing.Point(118, 116);
            this.txtIndexName.Name = "txtIndexName";
            this.txtIndexName.Size = new System.Drawing.Size(308, 20);
            this.txtIndexName.TabIndex = 83;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 87;
            this.label4.Text = "Document Type:";
            // 
            // txtDocumentType
            // 
            this.txtDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocumentType.Location = new System.Drawing.Point(118, 142);
            this.txtDocumentType.Name = "txtDocumentType";
            this.txtDocumentType.Size = new System.Drawing.Size(308, 20);
            this.txtDocumentType.TabIndex = 88;
            // 
            // txtSearchString
            // 
            this.txtSearchString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchString.Location = new System.Drawing.Point(118, 168);
            this.txtSearchString.Name = "txtSearchString";
            this.txtSearchString.Size = new System.Drawing.Size(308, 20);
            this.txtSearchString.TabIndex = 90;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 89;
            this.label1.Text = "Search String:";
            // 
            // txtZoneFileBrowse
            // 
            this.txtZoneFileBrowse.Location = new System.Drawing.Point(393, 192);
            this.txtZoneFileBrowse.Name = "txtZoneFileBrowse";
            this.txtZoneFileBrowse.Size = new System.Drawing.Size(33, 23);
            this.txtZoneFileBrowse.TabIndex = 91;
            this.txtZoneFileBrowse.Text = "...";
            this.txtZoneFileBrowse.UseVisualStyleBackColor = true;
            this.txtZoneFileBrowse.Click += new System.EventHandler(this.txtZoneFileBrowse_Click);
            // 
            // chkStripZoneSpaces
            // 
            this.chkStripZoneSpaces.AutoSize = true;
            this.chkStripZoneSpaces.Location = new System.Drawing.Point(118, 220);
            this.chkStripZoneSpaces.Name = "chkStripZoneSpaces";
            this.chkStripZoneSpaces.Size = new System.Drawing.Size(15, 14);
            this.chkStripZoneSpaces.TabIndex = 92;
            this.chkStripZoneSpaces.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 93;
            this.label3.Text = "Strip Spaces";
            // 
            // udZoneLength
            // 
            this.udZoneLength.Location = new System.Drawing.Point(118, 241);
            this.udZoneLength.Name = "udZoneLength";
            this.udZoneLength.Size = new System.Drawing.Size(120, 20);
            this.udZoneLength.TabIndex = 94;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 243);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 95;
            this.label5.Text = "Zone Length:";
            // 
            // txtProcessVersionID
            // 
            this.txtProcessVersionID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcessVersionID.Enabled = false;
            this.txtProcessVersionID.Location = new System.Drawing.Point(118, 90);
            this.txtProcessVersionID.Name = "txtProcessVersionID";
            this.txtProcessVersionID.Size = new System.Drawing.Size(308, 20);
            this.txtProcessVersionID.TabIndex = 101;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 13);
            this.label8.TabIndex = 100;
            this.label8.Text = "Process Version ID:";
            // 
            // txtProfileVersionID
            // 
            this.txtProfileVersionID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProfileVersionID.Enabled = false;
            this.txtProfileVersionID.Location = new System.Drawing.Point(118, 64);
            this.txtProfileVersionID.Name = "txtProfileVersionID";
            this.txtProfileVersionID.Size = new System.Drawing.Size(308, 20);
            this.txtProfileVersionID.TabIndex = 99;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 13);
            this.label9.TabIndex = 98;
            this.label9.Text = "Profile Version ID:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(59, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 97;
            this.label10.Text = "Profile ID:";
            // 
            // txtProfileID
            // 
            this.txtProfileID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProfileID.Enabled = false;
            this.txtProfileID.Location = new System.Drawing.Point(118, 38);
            this.txtProfileID.Name = "txtProfileID";
            this.txtProfileID.Size = new System.Drawing.Size(308, 20);
            this.txtProfileID.TabIndex = 96;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 323);
            this.Controls.Add(this.txtProcessVersionID);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtProfileVersionID);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtProfileID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.udZoneLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkStripZoneSpaces);
            this.Controls.Add(this.txtZoneFileBrowse);
            this.Controls.Add(this.txtSearchString);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDocumentType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIndexName);
            this.Controls.Add(this.cbProfile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.txtZoneFile);
            this.Controls.Add(this.TableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.TableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udZoneLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1;
        internal System.Windows.Forms.Button OK_Button;
        internal System.Windows.Forms.TextBox txtZoneFile;
        internal System.Windows.Forms.Button Cancel_Button;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.ComboBox cbProfile;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox txtIndexName;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtDocumentType;
        internal System.Windows.Forms.TextBox txtSearchString;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button txtZoneFileBrowse;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label label5;
        public System.Windows.Forms.CheckBox chkStripZoneSpaces;
        public System.Windows.Forms.NumericUpDown udZoneLength;
        internal System.Windows.Forms.TextBox txtProcessVersionID;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.TextBox txtProfileVersionID;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.TextBox txtProfileID;
    }
}