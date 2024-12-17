using System.Collections.Generic;

namespace Mibfa.OCR.Service.Library
{
    public partial class Library
    {


        #region config settings
        private ConfigData dsConfigData = new ConfigData();
        

        public void PopulateConfigDataSet()
        {
            //this method returns the attributes in the parent node ONLY  
            System.Xml.XmlElement l_oApplicationNode;


            l_oApplicationNode = Oculus.Configuration.Utility.GetApplicationConfigurationNode(m_ApplicationName);


            System.Xml.XmlNodeList xmlNodeList;
            try
            {
                xmlNodeList = l_oApplicationNode.SelectNodes("Data");

                foreach (System.Xml.XmlElement nde in xmlNodeList)
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
                    dsConfigData.Tables["tblImportSettings"].Rows.Add(sRowVals.ToArray());
                }
            }
            catch
            {

            }

        }

        private void LoadAllConfigs()
        {
            PopulateConfigDataSet();
        }
        # endregion



    }
}
