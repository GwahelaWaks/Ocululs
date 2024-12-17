using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Mibfa.OCR.Service.Designer
{
    public partial class Settings : Form
    {
        public List<profileVersion> profilesCollection = new List<profileVersion>();
        //public struct structVersions
        //{
        //    public string ProfileName;
        //    public System.Guid ProfileID;
        //    public System.Guid ProfileVersionID;
        //    public System.Guid ProcessVersionID;
        //}
        public Settings()
        {
            InitializeComponent();
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {

        }

        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            profileVersion ss = new profileVersion();
            //public string ProfileName;
            //public System.Guid ProfileID;
            //public System.Guid ProfileVersionID;
            //public System.Guid ProcessVersionID;

            ss = profilesCollection[cbProfile.SelectedIndex];

            string profileName = ss.ProfileName;//'ProfileName
            string profileID = ss.ProfileID.ToString();
            string profileVersionID = ss.ProfileVersionID.ToString();//'profileversionid
            string processID = ss.ProcessVersionID.ToString();//'profileversionid

            txtProfileID.Text = profileID;
            txtProfileVersionID.Text = profileVersionID;
            txtProcessVersionID.Text = processID;
        }

        private void txtZoneFileBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.Title = "Open OCR Zone File:";
            ofd.InitialDirectory = string.IsNullOrEmpty(txtZoneFile.Text) ? @"E:\Program Files (x86)\Oculus 10\Store\Zones" : Path.GetDirectoryName(txtZoneFile.Text);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtZoneFile.Text = ofd.FileName;
            }
        }
    }
}
