using System;

namespace Mibfa.OCR.Service.Library
{
    public partial class Library
    {

        private string GetApplicationSetting(string configValue)
        {
            //this method returns the attributes in the parent node ONLY  
            System.Xml.XmlElement l_oApplicationNode;


            l_oApplicationNode = Oculus.Configuration.Utility.GetApplicationConfigurationNode(m_ApplicationName);

            if (l_oApplicationNode == null)
            {
                throw new System.Exception("Configuration NOT found for " + m_ApplicationName + "!");
            }
            try
            {
                return l_oApplicationNode.GetAttribute(configValue);
            }
            catch (Exception ex)
            {
                return "";
            }


        }



        private string CreateInitialDocumentXML(string sDocID, string sFragmentCount, string sProfile, string sProfileVersion)
        {
            try
            {
                //   '<ocudoc:Document ID="48C8C09C-167A-4981-8195-85884E1BEAD8" FragmentCount="1" xmlns:ocudoc="https://oculus.co.za/imaging/document/1/0">
                //   ' <ocudoc:Data Profile="dfa397b3-0278-4097-9af9-3c3d68277ef4" ProfileVersion="7f56ecb7-6ca2-4061-b751-0e6f860ddf56"/>
                //   '</ocudoc:Document>

                string sXML = "";

                sXML += "<ocudoc:Document ID=\"" + sDocID.ToUpper() + "\" FragmentCount=\"" + sFragmentCount + "\" xmlns:ocu=\"https://oculus.co.za/imaging/config/1/0\"   xmlns:ocudoc=\"https://oculus.co.za/imaging/document/1/0\" >";
               
                sXML += "<ocudoc:Data Profile=\"" + sProfile + "\" ProfileVersion=\"" + sProfileVersion + "\" />";
                sXML += "</ocudoc:Document>";


                return sXML;
            }
            catch (Exception ex)
            {
                //          MsgBox("Error in CreateInitialDocumentXML: " & ex.Message.ToString)
                return "";
            }

        }

        private string CreateInitialFragmentXML(string sDocID, string sFragmentCount, string sProfile,
                        string sProfileVersion, string sFragmentID, string sFragmentName,
                    string sFragmentNo)
        {
            try
            {
                //    '<ocudoc:Fragment FragmentID="B84CA038-AECE-447E-8078-B517EB9F800F" FragmentName="B84CA038-AECE-447E-8078-B517EB9F800F.TIF" FragmentNo="1"/> </ocudoc:Document>



                string sXML = "";

                sXML += "<ocudoc:Document ID=\"" + sDocID.ToUpper() + "\" FragmentCount=\"" + sFragmentCount + "\" xmlns:ocudoc=\"https://oculus.co.za/imaging/document/1/0\"  xmlns:ocu=\"https://oculus.co.za/imaging/config/1/0\" > ";
                sXML += "<ocudoc:Data Profile=\"" + sProfile + "\" ProfileVersion=\"" + sProfileVersion + "\"/>";
                sXML += "<ocudoc:Fragment FragmentID=\"" + sFragmentID.ToUpper() + "\" FragmentName=\"" + sFragmentName.ToUpper() + "\" FragmentNo=\"" + sFragmentNo + "\"/> ";
                sXML += "</ocudoc:Document>";

                return sXML;
            }
            catch (Exception ex)
            {
                //   MsgBox("Error in CreateInitialFragmentXML: " & ex.Message.ToString)
                return "";
            }


        }







    }
}
