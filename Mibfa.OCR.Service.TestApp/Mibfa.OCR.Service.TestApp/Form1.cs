using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Mibfa.OCR.Service.Library;

namespace Mibfa.OCR.Service.TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Mibfa.OCR.Service.Library.Library m_OculusOCR;
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            m_OculusOCR = new Mibfa.OCR.Service.Library.Library();
            m_OculusOCR.StartProcessing();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_OculusOCR.StopProcessing();
            button1.Enabled = true;
        }

    }
}
