using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oculus10.FTPandMail.Library;
using System.IO;

namespace FtpCodeTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ftpURL = txtServer.Text;
            if (!ftpURL.ToUpper().StartsWith("FTP://"))
                ftpURL = "ftp://" + ftpURL;
            if (!ftpURL.EndsWith("/"))
                ftpURL += "/";

            FTP2 myFTP = new FTP2();
            string file = txtFile.Text;
            string start = DateTime.Now.ToString();
            myFTP.Upload(ftpURL + Path.GetFileName(file), txtUser.Text, txtPassword.Text, file, chkBinary.Checked, chkPassive.Checked);
            string end = DateTime.Now.ToString();


            MessageBox.Show("Done\r\n" + start + "\r\n" + end);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
