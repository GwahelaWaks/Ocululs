namespace Mibfa.OCR.Service.Designer
{
    partial class Designer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.tblImportSettingsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.btnBrowseAppTempFolder = new System.Windows.Forms.Button();
            this.txtAppTempFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtQueueName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtOculusPassword = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtOculusUser = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtWebServices = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtPollInterval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLogWebService = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.colProfileVersionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIndexName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colZoneFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStripSpaces = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colZoneLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProfileID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVersionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcessID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSearchString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.configData = new Mibfa.OCR.Service.Designer.ConfigData();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblImportSettingsBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.configData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(3, 489);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(31, 23);
            this.btnFirst.TabIndex = 31;
            this.btnFirst.Text = "|<<";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(111, 489);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(31, 23);
            this.btnLast.TabIndex = 30;
            this.btnLast.Text = ">>|";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(74, 489);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(31, 23);
            this.btnNext.TabIndex = 29;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(37, 489);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(31, 23);
            this.btnPrevious.TabIndex = 28;
            this.btnPrevious.Text = "<<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(184, 489);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(31, 23);
            this.btnRemove.TabIndex = 27;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(148, 489);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(31, 23);
            this.btnAdd.TabIndex = 26;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // DataGridView1
            // 
            this.DataGridView1.AllowUserToAddRows = false;
            this.DataGridView1.AllowUserToDeleteRows = false;
            this.DataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridView1.AutoGenerateColumns = false;
            this.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProfileVersionName,
            this.colIndexName,
            this.colDocumentType,
            this.colZoneFile,
            this.colSearchString,
            this.colStripSpaces,
            this.colZoneLength,
            this.colProfileID,
            this.colVersionID,
            this.colProcessID});
            this.DataGridView1.DataSource = this.tblImportSettingsBindingSource1;
            this.DataGridView1.Location = new System.Drawing.Point(-9, 204);
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.RowHeadersWidth = 51;
            this.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView1.Size = new System.Drawing.Size(478, 299);
            this.DataGridView1.TabIndex = 25;
            this.DataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellClick);
            // 
            // tblImportSettingsBindingSource1
            // 
            this.tblImportSettingsBindingSource1.DataMember = "tblImportSettings";
            this.tblImportSettingsBindingSource1.DataSource = this.bindingSource1;
            // 
            // btnBrowseAppTempFolder
            // 
            this.btnBrowseAppTempFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseAppTempFolder.Location = new System.Drawing.Point(461, 158);
            this.btnBrowseAppTempFolder.Name = "btnBrowseAppTempFolder";
            this.btnBrowseAppTempFolder.Size = new System.Drawing.Size(23, 20);
            this.btnBrowseAppTempFolder.TabIndex = 112;
            this.btnBrowseAppTempFolder.Text = "...";
            this.btnBrowseAppTempFolder.UseVisualStyleBackColor = true;
            this.btnBrowseAppTempFolder.Click += new System.EventHandler(this.btnBrowseAppTempFolder_Click);
            // 
            // txtAppTempFolder
            // 
            this.txtAppTempFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAppTempFolder.Location = new System.Drawing.Point(118, 158);
            this.txtAppTempFolder.Name = "txtAppTempFolder";
            this.txtAppTempFolder.Size = new System.Drawing.Size(337, 20);
            this.txtAppTempFolder.TabIndex = 111;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 113;
            this.label3.Text = "Temp Folder";
            // 
            // txtQueueName
            // 
            this.txtQueueName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQueueName.Location = new System.Drawing.Point(118, 132);
            this.txtQueueName.Name = "txtQueueName";
            this.txtQueueName.Size = new System.Drawing.Size(368, 20);
            this.txtQueueName.TabIndex = 110;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 135);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 13);
            this.label12.TabIndex = 109;
            this.label12.Text = "Queue Name";
            // 
            // txtOculusPassword
            // 
            this.txtOculusPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOculusPassword.Location = new System.Drawing.Point(117, 108);
            this.txtOculusPassword.Name = "txtOculusPassword";
            this.txtOculusPassword.Size = new System.Drawing.Size(369, 20);
            this.txtOculusPassword.TabIndex = 108;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 110);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 13);
            this.label13.TabIndex = 107;
            this.label13.Text = "Oculus Password";
            // 
            // txtOculusUser
            // 
            this.txtOculusUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOculusUser.Location = new System.Drawing.Point(118, 83);
            this.txtOculusUser.Name = "txtOculusUser";
            this.txtOculusUser.Size = new System.Drawing.Size(368, 20);
            this.txtOculusUser.TabIndex = 106;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 86);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 13);
            this.label14.TabIndex = 105;
            this.label14.Text = "Oculus User";
            // 
            // txtWebServices
            // 
            this.txtWebServices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWebServices.Location = new System.Drawing.Point(118, 59);
            this.txtWebServices.Name = "txtWebServices";
            this.txtWebServices.Size = new System.Drawing.Size(368, 20);
            this.txtWebServices.TabIndex = 104;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 61);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(101, 13);
            this.label15.TabIndex = 103;
            this.label15.Text = "Web Services Base";
            // 
            // txtPollInterval
            // 
            this.txtPollInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPollInterval.Location = new System.Drawing.Point(118, 35);
            this.txtPollInterval.Name = "txtPollInterval";
            this.txtPollInterval.Size = new System.Drawing.Size(368, 20);
            this.txtPollInterval.TabIndex = 102;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 101;
            this.label2.Text = "Poll Interval (seconds)";
            // 
            // txtLogWebService
            // 
            this.txtLogWebService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogWebService.Location = new System.Drawing.Point(118, 10);
            this.txtLogWebService.Name = "txtLogWebService";
            this.txtLogWebService.Size = new System.Drawing.Size(368, 20);
            this.txtLogWebService.TabIndex = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 99;
            this.label1.Text = "Log Web-Service";
            // 
            // colProfileVersionName
            // 
            this.colProfileVersionName.DataPropertyName = "ProfileVersionName";
            this.colProfileVersionName.HeaderText = "Profile Name";
            this.colProfileVersionName.Name = "colProfileVersionName";
            this.colProfileVersionName.Width = 92;
            // 
            // colIndexName
            // 
            this.colIndexName.DataPropertyName = "IndexName";
            this.colIndexName.HeaderText = "Index Name";
            this.colIndexName.Name = "colIndexName";
            this.colIndexName.Width = 89;
            // 
            // colDocumentType
            // 
            this.colDocumentType.DataPropertyName = "DocumentType";
            this.colDocumentType.HeaderText = "Document Type";
            this.colDocumentType.Name = "colDocumentType";
            this.colDocumentType.Width = 99;
            // 
            // colZoneFile
            // 
            this.colZoneFile.DataPropertyName = "ZoneFile";
            this.colZoneFile.HeaderText = "Zone File";
            this.colZoneFile.MinimumWidth = 6;
            this.colZoneFile.Name = "colZoneFile";
            this.colZoneFile.Width = 70;
            // 
            // colStripSpaces
            // 
            this.colStripSpaces.DataPropertyName = "StripSpaces";
            this.colStripSpaces.HeaderText = "StripSpaces";
            this.colStripSpaces.Name = "colStripSpaces";
            this.colStripSpaces.Width = 89;
            // 
            // colZoneLength
            // 
            this.colZoneLength.DataPropertyName = "ZoneLength";
            this.colZoneLength.HeaderText = "ZoneLength";
            this.colZoneLength.Name = "colZoneLength";
            this.colZoneLength.Width = 90;
            // 
            // colProfileID
            // 
            this.colProfileID.DataPropertyName = "ProfileID";
            this.colProfileID.HeaderText = "Profile ID";
            this.colProfileID.Name = "colProfileID";
            this.colProfileID.Width = 70;
            // 
            // colVersionID
            // 
            this.colVersionID.DataPropertyName = "VersionID";
            this.colVersionID.HeaderText = "Version ID";
            this.colVersionID.Name = "colVersionID";
            this.colVersionID.Width = 75;
            // 
            // colProcessID
            // 
            this.colProcessID.DataPropertyName = "ProcessID";
            this.colProcessID.HeaderText = "Process ID";
            this.colProcessID.Name = "colProcessID";
            this.colProcessID.Width = 78;
            // 
            // colSearchString
            // 
            this.colSearchString.DataPropertyName = "SearchString";
            this.colSearchString.HeaderText = "Search String";
            this.colSearchString.Name = "colSearchString";
            this.colSearchString.Width = 88;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = this.configData;
            this.bindingSource1.Position = 0;
            // 
            // configData
            // 
            this.configData.DataSetName = "ConfigData";
            this.configData.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Designer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBrowseAppTempFolder);
            this.Controls.Add(this.txtAppTempFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtQueueName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtOculusPassword);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtOculusUser);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtWebServices);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtPollInterval);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLogWebService);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.DataGridView1);
            this.Name = "Designer";
            this.Size = new System.Drawing.Size(487, 620);
            this.Load += new System.EventHandler(this.Designer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblImportSettingsBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.configData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnFirst;
        internal System.Windows.Forms.Button btnLast;
        internal System.Windows.Forms.Button btnNext;
        internal System.Windows.Forms.Button btnPrevious;
        internal System.Windows.Forms.Button btnRemove;
        internal System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.BindingSource bindingSource1;
        private ConfigData configData;
        private System.Windows.Forms.DataGridViewTextBoxColumn processIndexesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn thresholdDataGridViewTextBoxColumn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfigName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReceiveFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuccessFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMailFromName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMailFromEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFTPPassive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFTPCommandOnly;
        internal System.Windows.Forms.DataGridView DataGridView1;
        private System.Windows.Forms.BindingSource tblImportSettingsBindingSource1;
        internal System.Windows.Forms.Button btnBrowseAppTempFolder;
        private System.Windows.Forms.TextBox txtAppTempFolder;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox txtQueueName;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.TextBox txtOculusPassword;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.TextBox txtOculusUser;
        internal System.Windows.Forms.Label label14;
        internal System.Windows.Forms.TextBox txtWebServices;
        internal System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtPollInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLogWebService;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfileVersionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndexName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colZoneFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSearchString;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStripSpaces;
        private System.Windows.Forms.DataGridViewTextBoxColumn colZoneLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfileID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessID;
    }
}
