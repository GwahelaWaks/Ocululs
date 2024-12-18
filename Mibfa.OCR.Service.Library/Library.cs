using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Oculus.Configuration.Components;
using static Leadtools.Dicom.Common.DataTypes.PatientUpdater.PatientUpdaterConstants;
using System.Security.Claims;
using System.Windows.Input;
namespace Mibfa.OCR.Service.Library
{
    public partial class Library
    {
        private Thread m_oProcessingThread;
        private bool m_bShouldStop;
        const string m_ApplicationName = "Mibfa.OCR.Service";
        private int PollInterval = 0;
        private string webServicesBase = "";
        private string OculusUser = "";
        private string OculusPassword = "";
        private string QueueName = "";
        private string AppTempFolder = "";
        private string transactionID = "";
        public Oculus.Services.Managers.CachedSecurityManager cachedSecurityManager = new Oculus.Services.Managers.CachedSecurityManager();
        //XmlDocument documentXml = new XmlDocument();
        XmlNamespaceManager documentNamespaceManager; /* helper for selecting out of the document */
        private const string DOCUMENT_NAMESPACE = "https://oculus.co.za/imaging/document/1/0";
        string LoggingWS = ""; //GetApplicationSetting("OutputFolder");
        Logging logging;
        private string logsfolder = @"E:\Program Files\Oculus 10\Logs";
        private Dictionary<string, string> SavingsDocTypes = new Dictionary<string, string>();
        private List<clsFragment> _Fragments = new List<clsFragment>();

        public void StartProcessing()
        {
            LoggingWS = GetApplicationSetting("LogWebService");
            logging = new Logging(logsfolder, LoggingWS);

            string pollInterval = GetApplicationSetting("PollInterval");
            Int32.TryParse(pollInterval, out PollInterval);

            webServicesBase = GetApplicationSetting("WebServices");
            if (!webServicesBase.EndsWith("/"))
                webServicesBase += "/";

            OculusUser = GetApplicationSetting("OculusUser");
            OculusPassword = GetApplicationSetting("OculusPassword");
            QueueName = GetApplicationSetting("QueueName");
            AppTempFolder = GetApplicationSetting("AppTempFolder");

            ClearTempFiles();

            m_oProcessingThread = new Thread(this.MonitorOcrQueue);
            m_bShouldStop = false;
            m_oProcessingThread.IsBackground = true;
            m_oProcessingThread.Start();
        }
        private void ClearTempFiles()
        {
            string[] tmpFiles = Directory.GetFiles(AppTempFolder, "*.*");
            foreach (string tmpFile in tmpFiles)
            {
                System.IO.File.Delete(tmpFile);
            }
        }
        public void StopProcessing()
        {
            if (m_oProcessingThread == null)
            {
                return;
            }
            m_bShouldStop = true;
        }
        public void MonitorOcrQueue()
        {
            LoadAllConfigs();
            GetSavingsDocTypes();
            while (m_bShouldStop == false)
            {
                try
                {
                    //ServicePointManager.ServerCertificateValidationCallback = delegate (object sender1, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return (true); };
                    if (webServicesBase.ToLower().StartsWith("https:"))
                    {
                        System.Net.ServicePointManager.Expect100Continue = true;
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender1, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return (true); };
                    }

                    wsSecurity.Security wsSecurity = new wsSecurity.Security();
                    wsSecurity.Url = webServicesBase + "Security.asmx";

                    wsQueue.Queue wsQueue = new wsQueue.Queue();
                    wsQueue.Url = webServicesBase + "Queue.asmx";

                    Guid sessionID = wsSecurity.Login(OculusUser, OculusPassword);

                    int queueCount = wsQueue.GetQueueTotalCount(QueueName);

                    while (queueCount > 0)
                    {
                        //get the next document in the queue
                        string docxml = "";
                        Guid documentID = wsQueue.RequestDocument(sessionID, QueueName, out docxml);

                        XmlDocument queueDocXml = new XmlDocument();
                        queueDocXml.LoadXml(docxml);

                        XmlDocument documentXML = new XmlDocument();
                        documentNamespaceManager = new System.Xml.XmlNamespaceManager(documentXML.NameTable);
                        documentNamespaceManager.AddNamespace("ocudoc", DOCUMENT_NAMESPACE);
                        documentXML = queueDocXml;

                        System.Xml.XmlElement documentNode = null;
                        System.Xml.XmlElement dataNode = null;
                        documentNode = (XmlElement)documentXML.SelectSingleNode("/ocudoc:Document", documentNamespaceManager);
                        dataNode = (XmlElement)documentXML.SelectSingleNode("/ocudoc:Document/ocudoc:Data", documentNamespaceManager);
                        string profileID = dataNode.GetAttribute("Profile");
                        string profileContext = dataNode.GetAttribute("ProfileVersion");
                        string profileName = Oculus.Configuration.ProfileContext.GetProfileContext(new Guid(profileContext), true).ProfileName;
                        string profileContextName = Oculus.Configuration.ProfileContext.GetProfileContext(new Guid(profileContext), true).ContextName;
                        int start = profileContextName.IndexOf("[") + 1;
                        int end = profileContextName.IndexOf("]", start);
                        string verName = profileContextName.Substring(start, end - start);

                        bool fragsSaved = false;
                        try
                        {
                            Guid[] fragmentIds;
                            wsQueue.GetFragments(sessionID, documentID, out fragmentIds);
                            foreach (Guid fragmentID in fragmentIds)
                            {
                                Guid fragDocId;
                                string fragmentXMLstring = wsQueue.GetFragment(sessionID, fragmentID, out fragDocId);
                                byte[] fragmentBytes = wsQueue.GetFragmentData(sessionID, fragmentID);
                                XmlDocument fragmentXML = new XmlDocument();
                                fragmentXML.LoadXml(fragmentXMLstring);
                                ProcessFragment(documentXML, fragmentXML, fragmentBytes);
                            }
                            fragsSaved = true;
                        }
                        catch (Exception ex)
                        {
                            logging.EventLogWriteEntry(m_ApplicationName, "MonitorOcrQueue", "Exception: " + ex.ToString(), EventLogEntryType.Error);
                            wsQueue.CancelRequest(sessionID, documentID);
                        }
                        if (fragsSaved)
                        {
                            try
                            {
                                Dictionary<string, Dictionary<string, string>> packFragments = new Dictionary<string, Dictionary<string, string>>();
                                Dictionary<string, string> verifiedIDfrags = new Dictionary<string, string>();
                                string barcodeIDNumber = "";
                                foreach (clsFragment _frag in _Fragments)
                                {
                                    XmlElement fragNode = (XmlElement)_frag.FragmentXML.SelectSingleNode("/ocudoc:Document/ocudoc:Fragment", documentNamespaceManager);
                                    string guid = fragNode.GetAttribute("FragmentID");
                                    Dictionary<string, string> pageIndexes = new Dictionary<string, string>();
                                    pageIndexes.Add("LaunchProfileID", "");
                                    pageIndexes.Add("LaunchProfileVersionID", "");
                                    pageIndexes.Add("LaunchProcessID", "");
                                    pageIndexes.Add("GUID", guid);
                                    pageIndexes.Add("Thread_Index", "");
                                    pageIndexes.Add("ID_Number", "");
                                    pageIndexes.Add("Document_Type", "");
                                    pageIndexes.Add("Fund_Type", "");
                                    pageIndexes.Add("Claim_Type", "");
                                    pageIndexes.Add("Firm_Number", "");
                                    pageIndexes.Add("Members_Surname", "");
                                    pageIndexes.Add("Members_Full_Names", "");
                                    pageIndexes.Add("Company_Name", "");
                                    pageIndexes.Add("Create_Date", "");
                                    pageIndexes.Add("Requesting_Email_Address", "");
                                    pageIndexes.Add("New_Index", "");
                                    pageIndexes.Add("Status", "Not-Indexed");
                                    pageIndexes.Add("Document_Type_Code", "");
                                    pageIndexes.Add("Modified_Date", "");

                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "1", EventLogEntryType.Information);

                                    foreach (System.Data.DataRow ocrItem in dsConfigData.Tables[0].Rows)
                                    {
                                        try
                                        {
                                            string ProfileVersionName = ocrItem["ProfileVersionName"].ToString();
                                            string IndexName = ocrItem["IndexName"].ToString();
                                            string DocumentType = ocrItem["DocumentType"].ToString();
                                            string ZoneFile = ocrItem["ZoneFile"].ToString();
                                            string SearchString = ocrItem["SearchString"].ToString();
                                            bool StripSpaces = ocrItem["StripSpaces"].ToString() == "1" ? true : false;
                                            string ZoneLength = ocrItem["ZoneLength"].ToString();
                                            string LaunchProfileID = ocrItem["ProfileID"].ToString();
                                            string LaunchProfileVersionID = ocrItem["VersionID"].ToString();
                                            string LaunchProcessID = ocrItem["ProcessID"].ToString();

                                            //check to see if this document's profile has been configured for OCR
                                            if (LaunchProfileID.ToString().ToUpper() == profileID.ToUpper())
                                            {
                                                //get the page as per the configured page number

                                                string Thread_Index = GetIndexValue(dataNode, "Thread_Index"); // for logging
                                                string ID_Number = GetIndexValue(dataNode, "ID_Number");
                                                string Document_Type = GetIndexValue(dataNode, "Document_Type");
                                                string Fund_Type = GetIndexValue(dataNode, "Fund_Type");
                                                string Claim_Type = GetIndexValue(dataNode, "Claim_Type");
                                                string Firm_Number = GetIndexValue(dataNode, "Firm_Number");
                                                string Members_Surname = GetIndexValue(dataNode, "Members_Surname");
                                                string Members_Full_Names = GetIndexValue(dataNode, "Members_Full_Names");
                                                string Company_Name = GetIndexValue(dataNode, "Company_Name");
                                                string Create_Date = GetIndexValue(dataNode, "Create_Date");
                                                string Requesting_Email_Address = GetIndexValue(dataNode, "Requesting_Email_Address");
                                                string New_Index = GetIndexValue(dataNode, "New_Index");
                                                string Status = GetIndexValue(dataNode, "Status");
                                                string Document_Type_Code = GetIndexValue(dataNode, "Document_Type_Code");
                                                string Modified_Date = GetIndexValue(dataNode, "Modified_Date");

                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "transactionID: " + transactionID + " - Thread_Index=" + Thread_Index + ", ID_Number=" + ID_Number + ", Firm_Number=" + Firm_Number + ", Document_Type=" + Document_Type, EventLogEntryType.Information);

                                                try
                                                {
                                                    string verifiedID = "";
                                                    pageIndexes["LaunchProfileID"] = LaunchProfileID;
                                                    pageIndexes["LaunchProfileVersionID"] = LaunchProfileVersionID;
                                                    pageIndexes["LaunchProcessID"] = LaunchProcessID;
                                                    pageIndexes["GUID"] = guid;
                                                    pageIndexes["Thread_Index"] = Thread_Index;
                                                    pageIndexes["ID_Number"] = ID_Number;
                                                    pageIndexes["Document_Type"] = Document_Type;
                                                    pageIndexes["Fund_Type"] = Fund_Type;
                                                    pageIndexes["Claim_Type"] = Claim_Type;
                                                    pageIndexes["Firm_Number"] = Firm_Number;
                                                    pageIndexes["Members_Surname"] = Members_Surname;
                                                    pageIndexes["Members_Full_Names"] = Members_Full_Names;
                                                    pageIndexes["Company_Name"] = Company_Name;
                                                    pageIndexes["Create_Date"] = Create_Date;
                                                    pageIndexes["Requesting_Email_Address"] = Requesting_Email_Address;
                                                    pageIndexes["New_Index"] = New_Index;
                                                    pageIndexes["Status"] = "Not-Indexed";
                                                    pageIndexes["Document_Type_Code"] = Document_Type_Code;
                                                    pageIndexes["Modified_Date"] = Modified_Date;
                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "2", EventLogEntryType.Information);

                                                    if (Document_Type != "Email Message.pdf" && Path.GetFileName(_frag.FragmentFileName).ToLower().EndsWith(".pdf"))
                                                    {
                                                        if (string.Equals(profileID, LaunchProfileID.ToString(), StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "3", EventLogEntryType.Information);
                                                            Reader reader = new Reader(LoggingWS, logsfolder, transactionID, AppTempFolder);
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "4", EventLogEntryType.Information);
                                                            string ocrText = "";
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "5", EventLogEntryType.Information);
                                                            string targetFilename = Path.Combine(AppTempFolder, Path.GetFileName(_frag.FragmentFileName));
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "6", EventLogEntryType.Information);
                                                            string pageImageFile = "";
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "7", EventLogEntryType.Information);
                                                            string ocrZoneText = reader.Read(targetFilename, 1, SearchString, ZoneFile, ref ocrText, ref pageImageFile, StripSpaces);
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "8", EventLogEntryType.Information);
                                                            reader.Dispose();
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "9", EventLogEntryType.Information);

                                                            BarcodeReader bcr = new BarcodeReader(LoggingWS, logsfolder, transactionID, AppTempFolder);
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "10", EventLogEntryType.Information);

                                                            string[] barcodes = bcr.Read(targetFilename, 1);
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "11", EventLogEntryType.Information);
                                                            bcr.Dispose();
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "12", EventLogEntryType.Information);
                                                            if (barcodes.Length > 0)
                                                            {
                                                                if (barcodes[0].Length == 13)
                                                                {
                                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "13", EventLogEntryType.Information);
                                                                    barcodeIDNumber = barcodes[0];
                                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "14", EventLogEntryType.Information);
                                                                    verifiedID = AddIdStamp(targetFilename);
                                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "15", EventLogEntryType.Information);
                                                                    if (!verifiedIDfrags.ContainsKey(guid))
                                                                    {
                                                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "16", EventLogEntryType.Information);
                                                                        verifiedIDfrags.Add(guid, verifiedID);
                                                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "17", EventLogEntryType.Information);
                                                                    }
                                                                }
                                                            }
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "18", EventLogEntryType.Information);
                                                            if (!string.IsNullOrEmpty(ID_Number))
                                                            {
                                                                barcodeIDNumber = ID_Number;
                                                            }
                                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "19", EventLogEntryType.Information);
                                                            if (!string.IsNullOrEmpty(ZoneFile) && !string.IsNullOrEmpty(SearchString))
                                                            {
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "20", EventLogEntryType.Information);
                                                                if (ocrZoneText.Length != Convert.ToInt32(ZoneLength))
                                                                {
                                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "transactionID: " + transactionID + " - Zone result length of " + ocrZoneText.Length.ToString() + " invalid. Expected length is " + ZoneLength + " Text found in zone: " + ocrZoneText, EventLogEntryType.Information);
                                                                    if (IndexName == "ID_Number" && !string.IsNullOrEmpty(barcodeIDNumber))
                                                                    {
                                                                        pageIndexes[IndexName] = barcodeIDNumber;
                                                                        //verifiedID = AddIdStamp(targetFilename);
                                                                    }
                                                                    else
                                                                    {
                                                                        pageIndexes[IndexName] = "";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "transactionID: " + transactionID + " - Correct zone length of " + ZoneLength + "found. Text found in zone: " + ocrZoneText, EventLogEntryType.Information);
                                                                    if (IndexName == "ID_Number" && !string.IsNullOrEmpty(barcodeIDNumber))
                                                                    {
                                                                        pageIndexes[IndexName] = barcodeIDNumber;
                                                                        //verifiedID = AddIdStamp(targetFilename);
                                                                    }
                                                                    else
                                                                    {
                                                                        pageIndexes[IndexName] = ocrZoneText;
                                                                    }
                                                                }
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "21", EventLogEntryType.Information);
                                                                if (ocrText.ToUpper().Contains(SearchString.ToUpper()))
                                                                {
                                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "transactionID: " + transactionID + " - Found SearchString on page: " + SearchString, EventLogEntryType.Information);
                                                                    pageIndexes["Document_Type"] = ocrZoneText;
                                                                }
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "22", EventLogEntryType.Information);
                                                            }
                                                            else if (!string.IsNullOrEmpty(ZoneFile))
                                                            {
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "23", EventLogEntryType.Information);
                                                                if (ocrZoneText.Length != Convert.ToInt32(ZoneLength))
                                                                {
                                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "transactionID: " + transactionID + " - Zone result length of " + ZoneLength + "invalid. Text found in zone: " + ocrZoneText, EventLogEntryType.Information);
                                                                    if (IndexName == "ID_Number" && !string.IsNullOrEmpty(barcodeIDNumber))
                                                                    {
                                                                        pageIndexes[IndexName] = barcodeIDNumber;
                                                                        //verifiedID = AddIdStamp(targetFilename);
                                                                    }
                                                                    else
                                                                    {
                                                                        pageIndexes[IndexName] = "";
                                                                    }
                                                                    pageIndexes["Document_Type"] = "";
                                                                }
                                                                else
                                                                {
                                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "transactionID: " + transactionID + " - Correct zone length of " + ZoneLength + "found. Text found in zone: " + ocrZoneText, EventLogEntryType.Information);
                                                                    if (IndexName == "ID_Number" && !string.IsNullOrEmpty(barcodeIDNumber))
                                                                    {
                                                                        pageIndexes[IndexName] = barcodeIDNumber;
                                                                        //verifiedID = AddIdStamp(targetFilename);
                                                                    }
                                                                    else
                                                                    {
                                                                        pageIndexes[IndexName] = ocrZoneText;
                                                                    }
                                                                    pageIndexes["Document_Type"] = "";
                                                                }
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "24", EventLogEntryType.Information);
                                                            }
                                                            else if (!string.IsNullOrEmpty(SearchString))
                                                            {
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "25", EventLogEntryType.Information);
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "transactionID: " + transactionID + " - Text on page: \r\n" + ocrText, EventLogEntryType.Information);
                                                                switch (SearchString)
                                                                {
                                                                    case "I.D. No.":
                                                                    case "NATIONAL IDENTITY":
                                                                    case "S.A.CITIZEN":
                                                                    case "S.A.BURGER":
                                                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "26", EventLogEntryType.Information);
                                                                        string trimmedOcrText = ocrText.Trim().Replace(" ", "");
                                                                        if (trimmedOcrText.ToUpper().Contains(SearchString.ToUpper().Replace(" ", "")))
                                                                        {
                                                                            if (IndexName == "ID_Number" && !string.IsNullOrEmpty(barcodeIDNumber))
                                                                            {
                                                                                pageIndexes[IndexName] = barcodeIDNumber;
                                                                                //verifiedID = AddIdStamp(targetFilename);
                                                                            }
                                                                            else
                                                                            {
                                                                                pageIndexes[IndexName] = "";
                                                                            }
                                                                            pageIndexes["Document_Type"] = DocumentType;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (IndexName == "ID_Number" && !string.IsNullOrEmpty(barcodeIDNumber))
                                                                            {
                                                                                pageIndexes[IndexName] = barcodeIDNumber;
                                                                                //pageIndexes["Document_Type"] = DocumentType;
                                                                                //verifiedID = AddIdStamp(targetFilename);
                                                                            }
                                                                            else
                                                                            {
                                                                                pageIndexes[IndexName] = "";
                                                                                pageIndexes["Document_Type"] = "";
                                                                            }
                                                                        }
                                                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "27", EventLogEntryType.Information);
                                                                        break;
                                                                    default:
                                                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "28", EventLogEntryType.Information);
                                                                        if (IndexName == "ID_Number" && !string.IsNullOrEmpty(barcodeIDNumber))
                                                                        {
                                                                            pageIndexes[IndexName] = barcodeIDNumber;
                                                                            //pageIndexes["Document_Type"] = DocumentType;
                                                                            //verifiedID = AddIdStamp(targetFilename);
                                                                        }
                                                                        else
                                                                        {
                                                                            pageIndexes[IndexName] = "";
                                                                            pageIndexes["Document_Type"] = "";
                                                                        }
                                                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "29", EventLogEntryType.Information);
                                                                        break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "30", EventLogEntryType.Information);
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "transactionID: " + transactionID + " - SearchString not found: " + SearchString, EventLogEntryType.Information);
                                                                if (!string.IsNullOrEmpty(barcodeIDNumber))
                                                                {
                                                                    pageIndexes[IndexName] = barcodeIDNumber;
                                                                    //verifiedID = AddIdStamp(targetFilename);
                                                                }
                                                                else
                                                                {
                                                                    pageIndexes[IndexName] = "";
                                                                }
                                                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "31", EventLogEntryType.Information);
                                                                pageIndexes["Document_Type"] = "";
                                                            }
                                                        }

                                                    }


                                                }
                                                catch (Exception ex)
                                                {
                                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "32", EventLogEntryType.Information);
                                                    throw ex;
                                                }

                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "33", EventLogEntryType.Information);
                                            logging.EventLogWriteEntry(m_ApplicationName, "MonitorOcrQueue", profileID.ToUpper() + " - Exception: " + ex.Message.ToString(), EventLogEntryType.Error);
                                            logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "34", EventLogEntryType.Information);
                                            throw ex;
                                        }
                                    }
                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "35a", EventLogEntryType.Information);
                                    packFragments.Add(guid, pageIndexes);
                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "35b", EventLogEntryType.Information);
                                }
                                int fragmentCounter = 0;
                                foreach (KeyValuePair<string, Dictionary<string, string>> packFragment in packFragments)
                                {
                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "36", EventLogEntryType.Information);
                                    if (!string.IsNullOrEmpty(barcodeIDNumber))
                                    {
                                        if (barcodeIDNumber.Length == 13)
                                        {
                                            packFragment.Value["ID_Number"] = barcodeIDNumber;
                                        }
                                    }
                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "37", EventLogEntryType.Information);
                                    System.Xml.XmlElement fragNode = null;
                                    wsLaunch.Launch launch = new wsLaunch.Launch();
                                    launch = new wsLaunch.Launch();
                                    launch.Url = webServicesBase + "Launch.asmx";
                                    Guid uniqueDocID = new Guid();
                                    logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "38", EventLogEntryType.Information);
                                    try
                                    {
                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "39", EventLogEntryType.Information);
                                        fragNode = (XmlElement)_Fragments[fragmentCounter].FragmentXML.SelectSingleNode("/ocudoc:Document/ocudoc:Fragment", documentNamespaceManager);
                                        string fragID = fragNode.GetAttribute("FragmentID");
                                        string fragmentName = fragNode.GetAttribute("FragmentName");
                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "40", EventLogEntryType.Information);

                                        string DocID = Guid.NewGuid().ToString();
                                        string DocXML = GetDocXML(DocID, packFragment.Value["LaunchProfileID"], packFragment.Value["LaunchProfileVersionID"], 1, packFragment.Value);
                                        // Start launching the document into Oculus
                                        uniqueDocID = launch.LaunchDocumentWithProcessId(sessionID, DocXML, new Guid(packFragment.Value["LaunchProcessID"]));
                                        //get the fragment XML for the current fragment of the document
                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "41", EventLogEntryType.Information);
                                        string FragmentXML = GetFragXML(DocID, packFragment.Value["LaunchProfileID"], packFragment.Value["LaunchProfileVersionID"], fragmentName, 1, 1);
                                        if (verifiedIDfrags.ContainsKey(packFragment.Key))
                                        {
                                            if (!string.IsNullOrEmpty(verifiedIDfrags[packFragment.Key]))
                                            {
                                                _Fragments[fragmentCounter].FragmentData = System.IO.File.ReadAllBytes(verifiedIDfrags[packFragment.Key]);
                                            }
                                        }
                                        logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "42", EventLogEntryType.Information);
                                        launch.ProcessFragment(sessionID, uniqueDocID, FragmentXML, _Fragments[fragmentCounter].FragmentData);
                                        launch.CompleteLaunch(sessionID, uniqueDocID);
                                        logging.EventLogWriteEntry(m_ApplicationName, transactionID, "Document with ID " + DocID + " launched successfully", EventLogEntryType.Information);
                                        fragmentCounter++;
                                    }
                                    catch (Exception ex)
                                    {
                                        launch.CancelLaunch(sessionID, uniqueDocID);
                                        logging.EventLogWriteEntry(m_ApplicationName, transactionID, "An error occurred launching a fragment as a new document: " + ex.ToString(), EventLogEntryType.Error);
                                        throw new Exception("An error occurred launching a fragment as a new document: " + ex.ToString());
                                    }
                                }
                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "43", EventLogEntryType.Information);
                                wsQueue.ProcessDocument(sessionID, documentID, documentXML.InnerXml);
                                wsQueue.CompleteDocument(sessionID, documentID);
                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "44", EventLogEntryType.Information);
                            }
                            catch (Exception ex)
                            {
                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "45", EventLogEntryType.Information);
                                wsQueue.CancelRequest(sessionID, documentID);
                                logging.EventLogWriteEntry(m_ApplicationName, "CloseDocument", "46", EventLogEntryType.Information);
                                logging.EventLogWriteEntry(m_ApplicationName, "MonitorOcrQueue", "Exception: " + ex.ToString(), EventLogEntryType.Error);
                            }
                        }
                        ClearTempFiles();
                        queueCount = wsQueue.GetQueueTotalCount(QueueName);
                    }
                    wsSecurity.Logout(sessionID);
                }
                catch (Exception ex)
                {
                    logging.EventLogWriteEntry(m_ApplicationName, "MonitorOcrQueue", "Exception: " + ex.ToString(), EventLogEntryType.Error);
                }
                Thread.Sleep((PollInterval < 1 ? 60 : PollInterval) * 1000);
            }

        }
        private void ProcessFragment(XmlDocument documentXML, XmlDocument fragmentXml, byte[] fragmentBytes)
        {
            documentNamespaceManager = new System.Xml.XmlNamespaceManager(documentXML.NameTable);
            documentNamespaceManager.AddNamespace("ocudoc", DOCUMENT_NAMESPACE);
            System.Xml.XmlElement fragNode = null;
            fragNode = (XmlElement)fragmentXml.SelectSingleNode("/ocudoc:Document/ocudoc:Fragment", documentNamespaceManager);
            string fragID = fragNode.GetAttribute("FragmentID");
            string fragFileName = fragNode.GetAttribute("FragmentName");
            string docType = ""; // this.GetIndexValue(fragDataNode, "Document_Type");
            logging.EventLogWriteEntry(m_ApplicationName, transactionID, "fragName: " + fragFileName, EventLogEntryType.Information);

            //byte[] fragmentBytes = ReadStream(fragmentData);
            AddFragment(fragmentXml, fragmentBytes, fragFileName);

            string tempFilename = Path.Combine(AppTempFolder, System.IO.Path.GetFileName(fragFileName));
            //save the fragment to a temp file
            //byte[] fragmentBytes = ReadStream(fragmentData);
            System.IO.File.WriteAllBytes(tempFilename, fragmentBytes);
        }
        private string AddIdStamp(string pdfDocument)
        {
            try
            {
                PdfDocument pdfin = PdfReader.Open(pdfDocument, PdfDocumentOpenMode.Import);
                PdfDocument pdfout = new PdfDocument();

                for (int Pg = 0; Pg < pdfin.Pages.Count; Pg++)
                {
                    PdfPage pp = pdfout.AddPage(pdfin.Pages[Pg]);

                    if (Pg == 0)
                    {
                        XGraphics gfx = XGraphics.FromPdfPage(pp);
                        XFont font = new XFont("Arial", 16, XFontStyleEx.Bold);
                        gfx.DrawString("ID Document Verified", font, XBrushes.Black, new XRect(0, 0, pp.Width, pp.Height - 50), XStringFormats.BottomCenter);
                    }
                }

                string outputfile = Path.Combine(Path.GetDirectoryName(pdfDocument), Path.GetFileNameWithoutExtension(pdfDocument) + "-ID_verified" + Path.GetExtension(pdfDocument));
                pdfout.Save(outputfile);
                return outputfile;
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(m_ApplicationName, "AddIdStamp", "transactionID: " + transactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }

        }
        private string GetDocXML(string _DocGuid, string _ProfileID, string _ProfileVersionID, int _FragmentCount, Dictionary<string, string> documentIndexes)
        {
            //if (_IndexValues.Length != _IndexNames.Length)
            //    throw new Exception("Number of Index Names does notmatch number of Index Values:\r\n" + _IndexNames + "\r\n" + _IndexValues);

            string l_sXMLstring = "<ocudoc:Document ID=\"";
            l_sXMLstring += _DocGuid + "\" FragmentCount=\"" + _FragmentCount;
            l_sXMLstring += "\" xmlns:ocudoc=\"https://oculus.co.za/imaging/document/1/0\"><ocudoc:Data Profile=\"" + _ProfileID;
            l_sXMLstring += "\" ProfileVersion=\"" + _ProfileVersionID + "\">";

            foreach (KeyValuePair<string, string> packIndex in documentIndexes)
            {
                if (packIndex.Key != "LaunchProfileID" && packIndex.Key != "LaunchProfileVersionID" && packIndex.Key != "LaunchProcessID")
                {
                    l_sXMLstring += "<ocudoc:Index Name=\"" + packIndex.Key + "\" Value=\"" + packIndex.Value + "\" />";
                }
            }
            //for (int i = 0; i < _IndexNames.Length; i++)
            //    l_sXMLstring += "<ocudoc:Index Name=\"" + _IndexNames[i] + "\" Value=\"" + _IndexValues[i] + "\" />";

            l_sXMLstring += "</ocudoc:Data>";
            l_sXMLstring += "</ocudoc:Document>";
            return l_sXMLstring;
        }
        private string GetFragXML(string _DocGuid, string _ProfileID, string _ProfileVersionID, string _FragmentFilename, int _FragmentCount, int _FragmentNumber)
        {
            string l_sFragGuid = Guid.NewGuid().ToString();
            string l_sXMLstring = "<ocudoc:Document ID=\"";
            l_sXMLstring += _DocGuid + "\" FragmentCount=\"" + _FragmentCount.ToString();
            l_sXMLstring += "\" xmlns:ocudoc=\"https://oculus.co.za/imaging/document/1/0\"><ocudoc:Data Profile=\"" + _ProfileID;
            l_sXMLstring += "\" ProfileVersion=\"" + _ProfileVersionID + "\"/>";
            l_sXMLstring += "<ocudoc:Fragment FragmentID=\"" + l_sFragGuid + "\" FragmentName=\"" + Escaped(_FragmentFilename);
            l_sXMLstring += "\" FragmentNo=\"" + _FragmentNumber.ToString() + "\"/></ocudoc:Document>";
            return l_sXMLstring;
        }
        private string Escaped(string value)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement element = doc.CreateElement("tag");
            element.InnerText = value;
            return element.OuterXml.Replace("<tag>", "").Replace("</tag>", "");
        }
        private bool IsNumber(string value)
        {
            char[] letters = value.ToCharArray();
            foreach (char letter in letters)
            {
                int a = -1;
                if (!int.TryParse(letter.ToString(), out a))
                {
                    return false;
                }
            }
            return true;
        }
        private byte[] GetFragmentBytes(Stream input)
        {
            using (var memoryStream = new MemoryStream())
            {
                input.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        private void AddFragment(XmlDocument fragmentXml, byte[] fragmentData, string fragFileName)
        {
            clsFragment fragment = new clsFragment();
            fragment.FragmentFileName = fragFileName;
            fragment.FragmentXML = fragmentXml;
            fragment.FragmentData = fragmentData;
            fragment.TempFragment = System.IO.Path.GetFileName(fragFileName);
            _Fragments.Add(fragment);
        }
        public byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private Dictionary<string, string> GetIndexes(XmlElement p_oDataNode)
        {
            Dictionary<string, string> indexes = new Dictionary<string, string>();
            XmlNodeList l_oIndexNodes = p_oDataNode.SelectNodes("ocudoc:Index", documentNamespaceManager);
            foreach (XmlElement item in l_oIndexNodes)
            {
                indexes.Add(item.GetAttribute("Name"), item.GetAttribute("Value"));
            }

            return indexes;
        }
        private string GetIndexValue(XmlElement p_oDataNode, string p_sIndexName)
        {
            XmlElement l_oIndexNode = (XmlElement)p_oDataNode.SelectSingleNode("ocudoc:Index[@Name = '" + p_sIndexName + "']", documentNamespaceManager);

            if (l_oIndexNode == null)
            {
                return "";
            }

            return l_oIndexNode.GetAttribute("Value");
        }
        private string GetDocAttribute(XmlElement p_oDocNode, string attributeName)
        {
            return p_oDocNode.GetAttribute(attributeName);
        }
        private string GetDataAttribute(XmlElement p_oDataNode, string attributeName)
        {
            return p_oDataNode.GetAttribute(attributeName);
        }
        private List<string> GetIndexNames(XmlElement p_oDataNode)
        {
            XmlNodeList l_oIndexNodes = p_oDataNode.SelectNodes("ocudoc:Index", documentNamespaceManager);

            if (l_oIndexNodes == null)
            {
                return null;
            }

            List<string> indexNames = new List<string>();
            foreach (XmlElement indexNode in l_oIndexNodes)
            {
                indexNames.Add(indexNode.GetAttribute("Name").ToString());
            }

            return indexNames;
        }
        private void SetIndex(XmlElement p_oDataNode, string p_sIndexName, string p_sIndexValue)
        {
            XmlElement l_oIndexNode = (XmlElement)p_oDataNode.SelectSingleNode("ocudoc:Index[@Name = '" + p_sIndexName + "']", documentNamespaceManager);

            try
            {
                if (l_oIndexNode == null)
                {
                    l_oIndexNode = (XmlElement)p_oDataNode.AppendChild(p_oDataNode.OwnerDocument.CreateElement("ocudoc", "Index", DOCUMENT_NAMESPACE));
                }

                l_oIndexNode.SetAttribute("Name", p_sIndexName);
                l_oIndexNode.SetAttribute("Value", p_sIndexValue);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(m_ApplicationName, "SetIndex", ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
        }
        private void GetSavingsDocTypes()
        {
            SavingsDocTypes.Add("SGF", "Savings application form");
            SavingsDocTypes.Add("IYD", "Identity document");
            SavingsDocTypes.Add("BNC", "Bank confirmation letter/statement");
            SavingsDocTypes.Add("MEC", "Marriage certificate");
            SavingsDocTypes.Add("DEC", "Divorce order/annexures");
            SavingsDocTypes.Add("MEO", "Maintenance order");
            SavingsDocTypes.Add("CTA", "Court order/annexures");
            SavingsDocTypes.Add("AVT", "Affidavit");
            SavingsDocTypes.Add("SGD", "Supporting Document");
            SavingsDocTypes.Add("EML", "Email Message");
            SavingsDocTypes.Add("OFM", "Option Form");
            SavingsDocTypes.Add("DEL", "Deleted Document");
            SavingsDocTypes.Add("PAK", "Savings Document Pack");
        }
        private string GetSavingsDocCode(string doctype)
        {
            string SavingsID = SavingsDocTypes.FirstOrDefault(x => x.Value == doctype).Key;
            if (string.IsNullOrEmpty(SavingsID))
            {
                return SavingsID;
            }
            else
            {
                return "";
            }
        }
        private string GetSavingsDocType(string doccode)
        {
            if (SavingsDocTypes.ContainsKey(doccode))
            {
                return SavingsDocTypes[doccode];
            }
            else
            {
                return "";
            }
        }
        private Oculus.Configuration.Components.Collections.ProfileList GetProfileList()
        {
            return Oculus.Configuration.Utility.GetAllProfiles();
        }
    }
}
