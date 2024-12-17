using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Mibfa.OCR.Service.Designer
{
    public partial class Designer : UserControl, Oculus.Applications.Interfaces.ControlCentre.IDesigner
    {
        public Designer()
        {
            InitializeComponent();
        }

        #region Global Decs
        public List<profileVersion> profilesCollection = new List<profileVersion>();
        //public struct structVersions
        //{
        //    public string ProfileName;
        //    public System.Guid ProfileID;
        //    public System.Guid ProfileVersionID;
        //    public System.Guid ProcessVersionID;
        //}
        #endregion

        #region IDesigner Members
        /// <summary>
        /// Get/set the settings for the app
        /// </summary>
        public System.Xml.XmlElement Settings
        {
            get
            {
                XmlDocument l_doc = new XmlDocument();
                l_doc.LoadXml("<Settings><Data></Data></Settings>");

                // Dim l_doc As New System.Xml.XmlDocument
                //l_doc.LoadXml("<Settings><Data></Data></Settings>")
                System.Xml.XmlElement l_Settings = l_doc.DocumentElement;

                l_Settings.SetAttribute("LogWebService", this.txtLogWebService.Text);
                l_Settings.SetAttribute("PollInterval", this.txtPollInterval.Text);
                l_Settings.SetAttribute("WebServices", this.txtWebServices.Text);
                l_Settings.SetAttribute("OculusUser", this.txtOculusUser.Text);
                l_Settings.SetAttribute("OculusPassword", this.txtOculusPassword.Text);
                l_Settings.SetAttribute("QueueName", this.txtQueueName.Text);
                l_Settings.SetAttribute("AppTempFolder", this.txtAppTempFolder.Text);

                System.Xml.XmlNode xNode   = l_Settings.FirstChild;
                l_Settings.RemoveChild(xNode);

                foreach ( System.Data.DataRow rw in configData.Tables["tblImportSettings"].Rows)
                {
                    System.Xml.XmlNode xNewNode   = xNode.Clone();
                    System.Xml.XmlElement xNewElemt;

                    xNewElemt = (System.Xml.XmlElement)(xNewNode );

                    xNewElemt.SetAttribute("ProfileVersionName", rw["ProfileVersionName"].ToString());
                    xNewElemt.SetAttribute("IndexName", rw["IndexName"].ToString());
                    xNewElemt.SetAttribute("DocumentType", rw["DocumentType"].ToString());
                    xNewElemt.SetAttribute("ZoneFile", rw["ZoneFile"].ToString());
                    xNewElemt.SetAttribute("SearchString", rw["SearchString"].ToString());
                    xNewElemt.SetAttribute("StripSpaces", rw["StripSpaces"].ToString());
                    xNewElemt.SetAttribute("ZoneLength", rw["ZoneLength"].ToString());
                    xNewElemt.SetAttribute("ProfileID", rw["ProfileID"].ToString());
                    xNewElemt.SetAttribute("VersionID", rw["VersionID"].ToString());
                    xNewElemt.SetAttribute("ProcessID", rw["ProcessID"].ToString());

                    l_Settings.AppendChild(xNewElemt);
                }

                return l_doc.DocumentElement;

            }
            set
            {
                if (null == value)
                return;

                this.txtLogWebService.Text = value.GetAttribute("LogWebService");
                this.txtPollInterval.Text = value.GetAttribute("PollInterval");
                this.txtWebServices.Text = value.GetAttribute("WebServices");
                this.txtOculusUser.Text = value.GetAttribute("OculusUser");
                this.txtOculusPassword.Text = value.GetAttribute("OculusPassword");
                this.txtQueueName.Text = value.GetAttribute("QueueName");
                this.txtAppTempFolder.Text = value.GetAttribute("AppTempFolder");

                System.Xml.XmlNodeList xmlNodeList; 
                try
                {
                    xmlNodeList = value.SelectNodes("Data");

                    foreach(System.Xml.XmlElement nde in xmlNodeList)
                    {
                        List<string> sRowVals = new List<string>();

                        sRowVals.Add(nde.GetAttribute("ProfileVersionName"));
                        sRowVals.Add(nde.GetAttribute("IndexName"));
                        sRowVals.Add(nde.GetAttribute("DocumentType"));
                        sRowVals.Add(nde.GetAttribute("ZoneFile"));
                        sRowVals.Add(nde.GetAttribute("SearchString"));
                        sRowVals.Add(nde.GetAttribute("StripSpaces"));
                        sRowVals.Add(nde.GetAttribute("ZoneLength"));
                        sRowVals.Add(nde.GetAttribute("ProfileID"));
                        sRowVals.Add(nde.GetAttribute("VersionID"));
                        sRowVals.Add(nde.GetAttribute("ProcessID"));

                        configData.Tables["tblImportSettings"].Rows.Add(sRowVals.ToArray());
                    }
                }
                catch
                {

                }


            }
        }
        #endregion

        #region IManager Members
        /// <summary>
        /// Set the configuration for the designer
        /// </summary>
        public Oculus.Configuration.Components.Configuration Configuration
        {
            set { /* do nothing */ }
        }
        #endregion

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bindingSource1.MoveFirst();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            bindingSource1.MovePrevious();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            bindingSource1.MoveNext();
        }
        private void btnLast_Click(object sender, EventArgs e)
        {
            bindingSource1.MoveLast();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
             Settings odlg = new Settings();
            //'first initialize all the profiles in the cb
            Oculus.Configuration.Components.Collections.ProfileList profiles = new Oculus.Configuration.Components.Collections.ProfileList();
            profiles = GetProfileList();
            odlg.cbProfile.Items.Clear();
            profilesCollection.Clear();

            foreach (Oculus.Configuration.Components.Profile prof in profiles)
            {
                foreach (Oculus.Configuration.Components.ProfileVersion PV in prof.Versions)
                {
                    odlg.cbProfile.Items.Add(prof.Name + " Version: " + PV.Version.ToString());

                    profileVersion profVersion = new profileVersion();
                    profVersion.ProfileName = prof.Name + " Version: " + PV.Version.ToString();
                    profVersion.ProfileID = prof.ID;
                    profVersion.ProfileVersionID = PV.ID;
                    profVersion.ProcessVersionID = PV.Process;
                    profilesCollection.Add(profVersion);

                }
            }
            odlg.profilesCollection = profilesCollection;
            odlg.txtZoneFile.Text = "";
         
            if( odlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Data.DataRow oRow;
                List<string> sInValues = new List<string>();
                profileVersion selectedProfile = new profileVersion();
                selectedProfile = profilesCollection[odlg.cbProfile.SelectedIndex];

                sInValues.Add(odlg.cbProfile.Text);
                sInValues.Add(odlg.txtIndexName.Text);
                sInValues.Add(odlg.txtDocumentType.Text);
                sInValues.Add(odlg.txtZoneFile.Text);
                sInValues.Add(odlg.txtSearchString.Text);
                sInValues.Add(odlg.chkStripZoneSpaces.Checked ? "1" : "0");
                sInValues.Add(odlg.udZoneLength.Text);
                sInValues.Add(selectedProfile.ProfileID.ToString());
                sInValues.Add(odlg.txtProfileVersionID.Text);
                sInValues.Add(odlg.txtProcessVersionID.Text);

                configData.Tables["tblImportSettings"].Rows.Add(sInValues.ToArray());
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
             try
             {
            //'delete the selected row
            if( DataGridView1.CurrentRow.Index > -1)
            {
                configData.Tables[0].Rows.RemoveAt(DataGridView1.CurrentRow.Index);
            }
             }
        catch 
             { //   'an error occureed
        }
         
        
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			Settings odlg = new Settings();

            //System.Windows.Forms.DataGridViewRow oRow;
            //oRow = this.DataGridView1.CurrentRow;
            //sRowVals.Add(nde.GetAttribute("IndexName"));
            //sRowVals.Add(nde.GetAttribute("ZoneFile"));

            Oculus.Configuration.Components.Collections.ProfileList profiles = new Oculus.Configuration.Components.Collections.ProfileList();
            profiles = GetProfileList();
            odlg.cbProfile.Items.Clear();
            profilesCollection.Clear();
            foreach (Oculus.Configuration.Components.Profile prof in profiles)
            {
                foreach (Oculus.Configuration.Components.ProfileVersion PV in prof.Versions)
                {
                    odlg.cbProfile.Items.Add(prof.Name + " Version: " + PV.Version.ToString());

                    profileVersion profVerion = new profileVersion();
                    profVerion.ProfileName = prof.Name + " Version: " + PV.Version.ToString();
                    profVerion.ProfileID = prof.ID;
                    profVerion.ProfileVersionID = PV.ID;
                    profVerion.ProcessVersionID = PV.Process;
                    profilesCollection.Add(profVerion);
                }
            }
            odlg.profilesCollection = profilesCollection;

            try
            {
                odlg.cbProfile.SelectedItem = DataGridView1.CurrentRow.Cells["colProfileVersionName"].Value;
            }
            catch (Exception ex)
            {

            }
            odlg.txtIndexName.Text = DataGridView1.CurrentRow.Cells["colIndexName"].Value.ToString();
            odlg.txtDocumentType.Text = DataGridView1.CurrentRow.Cells["colDocumentType"].Value.ToString();
            odlg.txtZoneFile.Text = DataGridView1.CurrentRow.Cells["colZoneFile"].Value.ToString();
            odlg.txtSearchString.Text = DataGridView1.CurrentRow.Cells["colSearchString"].Value.ToString();
            odlg.chkStripZoneSpaces.Checked = DataGridView1.CurrentRow.Cells["colStripSpaces"].Value.ToString() == "1" ? true : false;
            odlg.udZoneLength.Text = DataGridView1.CurrentRow.Cells["colZoneLength"].Value.ToString();
            //sInValues.Add(odlg.chkStripZoneSpaces.Checked ? "1" : "0");
            //sInValues.Add(odlg.udZoneLength.Text);

            if (odlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //'we have made changes and need to write these to the dataset
                profileVersion selectedProfile = new profileVersion();
                selectedProfile = profilesCollection[odlg.cbProfile.SelectedIndex];

                DataGridView1.CurrentRow.Cells["colProfileVersionName"].Value = odlg.cbProfile.Text;
                DataGridView1.CurrentRow.Cells["colIndexName"].Value = odlg.txtIndexName.Text;
                DataGridView1.CurrentRow.Cells["colDocumentType"].Value = odlg.txtDocumentType.Text;
                DataGridView1.CurrentRow.Cells["colZoneFile"].Value = odlg.txtZoneFile.Text;
                DataGridView1.CurrentRow.Cells["colSearchString"].Value = odlg.txtSearchString.Text;
                DataGridView1.CurrentRow.Cells["colStripSpaces"].Value = odlg.chkStripZoneSpaces.Checked ? "1" : "0";
                DataGridView1.CurrentRow.Cells["colZoneLength"].Value = odlg.udZoneLength.Text;
                DataGridView1.CurrentRow.Cells["colProfileID"].Value = odlg.txtProfileID.Text;
                DataGridView1.CurrentRow.Cells["colVersionID"].Value = odlg.txtProfileVersionID.Text;
                DataGridView1.CurrentRow.Cells["colProcessID"].Value = odlg.txtProcessVersionID.Text;
            }
            else
            {
             //   'do nothing
            }
        }
        private void Designer_Load(object sender, EventArgs e)
        {

        }

        private void tblImportSettingsBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void TxtSQLUser_TextChanged(object sender, EventArgs e)
        {

        }
        private Oculus.Configuration.Components.Collections.ProfileList GetProfileList()
        {
            return Oculus.Configuration.Utility.GetAllProfiles();
        }

        private void btnBrowseAppTempFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtAppTempFolder.Text.Trim())) {
                dlg.SelectedPath = txtAppTempFolder.Text.Trim();
            }
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtAppTempFolder.Text = dlg.SelectedPath.Trim();
            }

        }
    }
}
