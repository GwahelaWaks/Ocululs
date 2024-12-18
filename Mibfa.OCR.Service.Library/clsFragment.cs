using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mibfa.OCR.Service.Library
{
    public class clsFragment
    {
        private string _FragmentFileName;
        private XmlDocument _FragmentXML;
        private byte[] _FragmentData;
        private string _TempFragment;

        public string TempFragment
        {
            get { return _TempFragment; }
            set { _TempFragment = value; }
        }
        public byte[] FragmentData
        {
            get { return _FragmentData; }
            set { _FragmentData = value; }
        }
        public XmlDocument FragmentXML
        {
            get { return _FragmentXML; }
            set { _FragmentXML = value; }
        }
        public string FragmentFileName
        {
            get { return _FragmentFileName; }
            set { _FragmentFileName = value; }
        }

    }
}