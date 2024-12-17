using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.IO;
//using Leadtools.Demos;
//using Leadtools.Demos.Dialogs;
using Leadtools;
//using Leadtools.Twain;
using Leadtools.Codecs;
//using Leadtools.Controls;
//using Leadtools.ImageProcessing;
//using Leadtools.ImageProcessing.Core;
//using Leadtools.ImageProcessing.Color;
//using Leadtools.ImageProcessing.Effects;
using Leadtools.Ocr;
using Leadtools.Document.Writer;
//using Leadtools.Drawing;
using static System.Net.Mime.MediaTypeNames;
//using Leadtools.Ocr.LEADEngine;
using System.ComponentModel;
using System.Reflection.Emit;
//using Leadtools.WinForms;
//using Leadtools.Twain;
//using Leadtools.Barcode;
using System.Drawing.Drawing2D;
using Leadtools.Barcode;

namespace Mibfa.OCR.Service.Library
{
    public class Reader : IDisposable
    {
        // The RasterCodecs instance used to load/save images
        private RasterCodecs _rasterCodecs;
        // The OCR engine instance used in this demo
        private IOcrEngine _ocrEngine;
        // The current OCR document
        private IOcrDocument _ocrDocument;
        // The current OCR page in the viewer
        private IOcrPage _ocrPage;
        private bool _disposed;
        ~Reader() => Dispose();

        string OcrLEADRuntimeDir = @"E:\Program Files\Oculus 10\Bin\OcrLEADRuntime";

        //string MY_LICENSE_FILE = @"E:\License_BarcodeOCR\LEADTOOLS.LIC";
        //string MY_DEVELOPER_KEY = System.IO.File.ReadAllText(@"E:\License_BarcodeOCR\LEADTOOLS.LIC.key");
        private bool _omrOptionsDismissed;
        private bool _saveAfterRecognize;
        private string _ocrDocumentFilePath;
        private string _openInitialPath = string.Empty;
        private string _fileName = string.Empty;
        private bool _customFileName = false; // Has user given own file name for save.
        private bool _documentMode = false;
        private string _outputDir = string.Empty;
        public bool PerspectiveDeskewActive = false;
        public bool UnWarpActive = false;
        private string appName = "MIBFA.OCR";
        private string ocrText = "";
        private string ocrZoneText = "";
        private string LoggingWS = "";
        private string TransactionID = "";
        private string TempFolder = "";
        private XmlNamespaceManager m_oDocNamespaceManager; /* helper for selecting out of the document */
        private const string DOC_NAMESPACE = "https://oculus.co.za/imaging/document/1/0";
        private Logging logging; // = new Logging(@"E:\Program Files\Oculus 10\Logs");
        private string logsFolder = "";
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this._rasterCodecs?.Dispose();
                this._ocrEngine?.Dispose();
                this._ocrDocument?.Dispose();
                this._ocrPage?.Dispose();
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Reader(string loggingWS, string logFolder, string transactionID, string tempFolder)
        {
            LoggingWS = loggingWS;
            TransactionID = transactionID;
            TempFolder = tempFolder;
            logsFolder = logFolder;
            logging = new Logging(logsFolder, LoggingWS);
        }
        public string Read(string documentToOCR, int targetPage, string searchString, string zoneFile, ref string pageText, ref string pageImageFile, bool stripZoneSpaces)
        {
            try
            {
                logging.EventLogWriteEntry(appName, "OCRReader.Read", TransactionID + " - documentToOCR: " + documentToOCR + " - startPage: " + targetPage.ToString() + " - endPage: " + targetPage.ToString(), EventLogEntryType.Information);
                logging.EventLogWriteEntry(appName, "OCRReader.Read", "1", EventLogEntryType.Information);
                Startup();
                logging.EventLogWriteEntry(appName, "OCRReader.Read", "2", EventLogEntryType.Information);

                try
                {
                    logging.EventLogWriteEntry(appName, "OCRReader.Read", TransactionID + " - ZoneFile: " + zoneFile, EventLogEntryType.Information);
                    logging.EventLogWriteEntry(appName, "OCRReader.Read", "3", EventLogEntryType.Information);

                    System.Collections.ArrayList extractedImages = SplitPDFImages(documentToOCR, TempFolder, Path.GetFileNameWithoutExtension(documentToOCR));
                    logging.EventLogWriteEntry(appName, "OCRReader.Read", "4", EventLogEntryType.Information);
                    pageImageFile = extractedImages[targetPage - 1].ToString();
                    logging.EventLogWriteEntry(appName, "OCRReader.Read", "5 " + pageImageFile, EventLogEntryType.Information);
                    OpenDocument(pageImageFile);
                    logging.EventLogWriteEntry(appName, "OCRReader.Read", "6", EventLogEntryType.Information);
                    logging.EventLogWriteEntry(appName, "OCRReader.Read", TransactionID + " - attempting OCR recognition", EventLogEntryType.Information);
                    RecognizeDocument(pageImageFile, false, targetPage, targetPage, searchString, zoneFile);
                    logging.EventLogWriteEntry(appName, "OCRReader.Read", "7", EventLogEntryType.Information);
                    logging.EventLogWriteEntry(appName, "OCRReader.Read", TransactionID + " - ocrText: " + ocrText.Replace("\r\n", "").Trim() + " - ocrZoneText: " + ocrZoneText.Replace("\r\n", "").Trim(), EventLogEntryType.Information);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.Read", TransactionID + " - Exception: " + ex.Message.ToString(), EventLogEntryType.Error);
                throw ex;
            }
            if (!string.IsNullOrEmpty(zoneFile) && !string.IsNullOrEmpty(searchString))
            {
                pageText = ocrText.Replace("\r\n", "").Trim();
                return stripZoneSpaces ? ocrZoneText.Replace("\r\n", "").Replace(" ", "").Trim() : ocrZoneText.Replace("\r\n", "").Trim();
            }
            else if (!string.IsNullOrEmpty(zoneFile))
            {
                pageText = "";
                return stripZoneSpaces ? ocrZoneText.Replace("\r\n", "").Replace(" ", "").Trim() : ocrZoneText.Replace("\r\n", "").Trim();
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                pageText = ocrText.Replace("\r\n", "").Trim();
                return "";
            }
            pageText = "";
            return "";

        }
        private void RecognizeDocument(string documentToOCR, bool allPages, int startPage, int endPage, string searchString, string zoneFile)
        {
            logging.EventLogWriteEntry(appName, "OCRReader.RecognizeDocument", "1", EventLogEntryType.Information);
            //Recognize current or all pages in the document
            try
            {
                string licenseFilePath = @"E:\License_BarcodeOCR\LEADTOOLS.LIC";
                string developerKey = System.IO.File.ReadAllText(@"E:\License_BarcodeOCR\LEADTOOLS.LIC.key");
                RasterSupport.SetLicense(licenseFilePath, developerKey);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.RecognizeDocument", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
            logging.EventLogWriteEntry(appName, "OCRReader.RecognizeDocument", "2", EventLogEntryType.Information);

            //CreateOcrDocument(OcrCreateDocumentOptions.LoadExisting, documentToOCR);

            // Setup the arguments for the callback
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("allPages", allPages);
            logging.EventLogWriteEntry(appName, "OCRReader.RecognizeDocument", "3", EventLogEntryType.Information);

            // Call the process dialog
            try
            {
                logging.EventLogWriteEntry(appName, "OCRReader.RecognizeDocument", "4", EventLogEntryType.Information);
                logging.EventLogWriteEntry(appName, "OCRReader.RecognizeDocument", TransactionID + " - attempting OCR recognition", EventLogEntryType.Information);
                DoRecognize(documentToOCR, false, startPage, endPage, searchString, zoneFile);
                logging.EventLogWriteEntry(appName, "OCRReader.RecognizeDocument", "5", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.RecognizeDocument", TransactionID + " - Exception: " + ex.Message.ToString(), EventLogEntryType.Error);
                throw ex;
            }
            finally
            {
                logging.EventLogWriteEntry(appName, "OCRReader.RecognizeDocument", "6", EventLogEntryType.Information);
                // Re-paint current page to show new zones
                //_viewerControl.ZonesUpdated();
                //UpdateUIState();
            }
        }
        private void DoRecognize(string documentToOCR, bool allPages, int startPage, int endPage, string searchString, string zoneFile)
        {
            try
            {
                DateTime beginTime = DateTime.Now;
                logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", "1", EventLogEntryType.Information);

                if (!string.IsNullOrEmpty(zoneFile) && string.IsNullOrEmpty(searchString))
                {
                    logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", "2", EventLogEntryType.Information);
                    logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", TransactionID + " - OCR a zone", EventLogEntryType.Information);
                    if (_ocrDocument == null)
                    {
                        CreateOcrDocument(OcrCreateDocumentOptions.InMemory, documentToOCR);
                    }
                    _ocrDocument.LoadZones(zoneFile);
                    _ocrPage.Recognize(null);
                    ocrZoneText = _ocrPage.GetText(-1); // -1 will get all page recognized text
                    logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", "3", EventLogEntryType.Information);
                }
                else if (string.IsNullOrEmpty(zoneFile) && !string.IsNullOrEmpty(searchString))
                {
                    logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", "4", EventLogEntryType.Information);
                    logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", TransactionID + " - OCR the complete page", EventLogEntryType.Information);
                    if (_ocrDocument == null)
                    {
                        CreateOcrDocument(OcrCreateDocumentOptions.InMemory, documentToOCR);
                    }
                    _ocrPage.Recognize(null);
                    ocrText = _ocrPage.GetText(-1); // -1 will get all page recognized text
                    logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", "5", EventLogEntryType.Information);
                }
                else if (!string.IsNullOrEmpty(zoneFile) && !string.IsNullOrEmpty(searchString))
                {
                    logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", "6", EventLogEntryType.Information);
                    logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", TransactionID + " - OCR the complete page and a zone", EventLogEntryType.Information);
                    if (_ocrDocument == null)
                    {
                        CreateOcrDocument(OcrCreateDocumentOptions.InMemory, documentToOCR);
                    }
                    _ocrPage.Recognize(null);
                    ocrText = _ocrPage.GetText(-1); // -1 will get all page recognized text

                    _ocrDocument.LoadZones(zoneFile);
                    _ocrPage.Recognize(null);
                    ocrZoneText = _ocrPage.GetText(-1); // -1 will get all page recognized text
                    logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", "7", EventLogEntryType.Information);
                }

            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", "8", EventLogEntryType.Information);
                logging.EventLogWriteEntry(appName, "OCRReader.DoRecognize", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
            finally
            {
            }
        }
        private void Startup()
        {
            logging.EventLogWriteEntry(appName, "OCRReader.Startup", "1", EventLogEntryType.Information);
            Properties.Settings settings = new Properties.Settings();
            try
            {
                logging.EventLogWriteEntry(appName, "OCRReader.Startup", "2", EventLogEntryType.Information);
                SetLicense();
                logging.EventLogWriteEntry(appName, "OCRReader.Startup", "3", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.Startup", "5", EventLogEntryType.Information);
                logging.EventLogWriteEntry(appName, "OCRReader.Startup", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }

            logging.EventLogWriteEntry(appName, "OCRReader.Startup", "6", EventLogEntryType.Information);
            // Initialize the RasterCodecs object
            _rasterCodecs = new RasterCodecs();

            logging.EventLogWriteEntry(appName, "OCRReader.Startup", "7", EventLogEntryType.Information);
            // Use the new RasterizeDocumentOptions to default loading document files at 300 DPI
            _rasterCodecs.Options.RasterizeDocument.Load.XResolution = 300;
            logging.EventLogWriteEntry(appName, "OCRReader.Startup", "8", EventLogEntryType.Information);
            _rasterCodecs.Options.RasterizeDocument.Load.YResolution = 300;
            logging.EventLogWriteEntry(appName, "OCRReader.Startup", "9", EventLogEntryType.Information);
            _rasterCodecs.Options.Pdf.Load.EnableInterpolate = true;
            logging.EventLogWriteEntry(appName, "OCRReader.Startup", "10", EventLogEntryType.Information);
            _rasterCodecs.Options.Load.AutoFixImageResolution = true;
            logging.EventLogWriteEntry(appName, "OCRReader.Startup", "11", EventLogEntryType.Information);

            try
            {
                logging.EventLogWriteEntry(appName, "OCRReader.Startup", "12", EventLogEntryType.Information);
                _ocrEngine = OcrEngineManager.CreateEngine(OcrEngineType.LEAD);
                logging.EventLogWriteEntry(appName, "OCRReader.Startup", "13", EventLogEntryType.Information);

#if LT_CLICKONCE
            _ocrEngine.Startup( _rasterCodecs, null, null, Application.StartupPath + @"\OCR Engine" );
#else
                _ocrEngine.Startup(_rasterCodecs, null, null, OcrLEADRuntimeDir);
#endif // #if LT_CLICKONCE
                logging.EventLogWriteEntry(appName, "OCRReader.Startup", "14", EventLogEntryType.Information);

            }
            catch (System.Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.Startup", "Error starting the OCR engine: " + ex.ToString(), EventLogEntryType.Information);
                logging.EventLogWriteEntry(appName, "OCRReader.Startup", "15", EventLogEntryType.Error);
                //string message = string.Format("{0}. This demo cannot start without OCR capabilities.", ex.Message);
                //MessageBox.Show(this, message, "Engine Startup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Close();
                return;
            }

            // Load the default document
            //#if LT_CLICKONCE
            //         string defaultDocumentFile = Path.Combine(Application.StartupPath, "ocr1.tif");
            //#else
            //            string defaultDocumentFile = Path.Combine(DemosGlobal.ImagesFolder, "ocr1.tif");
            //#endif

            //if (File.Exists(defaultDocumentFile))
            //{
            //    OpenDocument(defaultDocumentFile, null, 1, 1);
            //}
        }
        private void SetFileName(string fileName)
        {
            if (_documentMode && (_ocrDocument != null) && !(_ocrDocument.Pages.Count >= 1))
            {
                _fileName = fileName;
            }
            else if (!_documentMode && _outputDir.Equals(String.Empty))
            {
                if (_customFileName)
                    _fileName = Path.Combine(Path.GetDirectoryName(_fileName), Path.GetFileName(fileName));
                else
                    _fileName = fileName;
            }
            else if (!_documentMode)
            {
                if (fileName != null)
                {
                    if (!_outputDir.Equals(String.Empty))
                        _fileName = Path.Combine(_outputDir, Path.GetFileName(fileName));
                    else if (_customFileName)
                        _outputDir = Path.GetDirectoryName(_fileName);
                }
            }
        }
        private void OpenDocument(string fileName)
        {
            // Open a document from disk
            try
            {
                OpenDocument(fileName, null, 1, 1);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
        }
        private void OpenDocument(string fileName, RasterImage rasterImage, int firstPage, int lastPage)
        {
            // Open the document in file name
            try
            {
                logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", "1", EventLogEntryType.Information);
                SetFileName(fileName);

                logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", "2" + fileName, EventLogEntryType.Information);
                bool multipage = lastPage != firstPage;

                DateTime beginTime = DateTime.Now;
                int pageIndex = firstPage;


                // Load the bitmap page
                RasterImage image = null;
                logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", "3", EventLogEntryType.Information);

                if (rasterImage != null)
                {
                    logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", "4", EventLogEntryType.Information);
                    image = rasterImage;
                    logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", "5", EventLogEntryType.Information);
                }
                else
                {
                    logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", "6", EventLogEntryType.Information);
                    image = _rasterCodecs.Load(fileName, firstPage);
                    logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", "7", EventLogEntryType.Information);

                    _ocrPage = _ocrEngine.CreatePage(image, OcrImageSharingMode.AutoDispose);
                    logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", "8", EventLogEntryType.Information);
                }

            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.OpenDocument", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
        }
        private System.Collections.ArrayList SplitPDFImages(string sInputFullPath, string tempFolderPath, string attachmentPrefix)
        {
            System.Collections.ArrayList retValue = new System.Collections.ArrayList();
            retValue.Clear();

            //create a new GUID for the images
            string filenamePattern = attachmentPrefix + System.Guid.NewGuid().ToString();
            try
            {
                retValue = ExportImagesFromPDFToArrayList(sInputFullPath, tempFolderPath, filenamePattern);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;

        }
        private void CreateOcrDocument(OcrCreateDocumentOptions options, string ocrDocumentFilePath)
        {
            // Create a new document
            logging.EventLogWriteEntry(appName, "OCRReader.CreateOcrDocument", TransactionID + " - Create a new document", EventLogEntryType.Information);
            if (_ocrDocument != null)
            {
                if (IsOcrDocumentInMemory())
                {
                    DoCloseOcrDocument();
                }

                if (_ocrDocument != null)
                {
                    _ocrDocument.Dispose();
                    _ocrDocument = null;
                }
            }

            string ocrDocumentFile = (options != OcrCreateDocumentOptions.InMemory) ? ocrDocumentFilePath : null;
            _ocrDocument = _ocrEngine.DocumentManager.CreateDocument(ocrDocumentFile, options);
            if (options == OcrCreateDocumentOptions.InMemory && _ocrPage != null)
            {
                // Insert the existing OCR page into the document
                _ocrDocument.Pages.Insert(-1, _ocrPage);
            }

            _documentMode = true;
        }
        private void DoCloseOcrDocument()
        {
            bool isInMemory = IsOcrDocumentInMemory();
            _ocrDocument.Dispose();
            _ocrDocument = null;
            _customFileName = false;

            if (isInMemory)
            {
                _ocrPage = null;
            }

            _documentMode = false;
        }
        private bool IsOcrDocumentInMemory()
        {
            bool isInMemory = false;

            if (_ocrDocument != null)
                isInMemory = _ocrDocument.IsInMemory;

            return isInMemory;
        }
        int GetOcrDocumentPagesCount()
        {
            int pagesCount = 0;

            if (_ocrDocument != null)
                pagesCount = _ocrDocument.Pages.Count;

            return pagesCount;
        }
        bool IsCurrentOcrPagePartOfOcrDocument()
        {
            if (IsOcrDocumentInMemory())
            {
                int pageIndex = -1;
                pageIndex = _ocrDocument.Pages.IndexOf(_ocrPage);
                if (pageIndex != -1)
                    return true;
            }

            return false;
        }

        void CloseCurrentOcrPage()
        {
            if (_ocrPage != null)
            {
                _ocrPage.Dispose();
                _ocrPage = null;
                //_viewerControl.ClearZones();
                //_viewerControl.SetImageAndPage(null, null, null);
                //_viewerControl.SetPages(0, 0);
                //_viewerControl.Title = string.Empty;
                //RepopulateOcrPageTextWindow();
                //UpdateUIState();
            }
        }

        public bool SetLicense(bool silent)
        {
            try
            {
                string licenseFilePath = @"E:\License_BarcodeOCR\LEADTOOLS.LIC";
                string developerKey = System.IO.File.ReadAllText(@"E:\License_BarcodeOCR\LEADTOOLS.LIC.key");
                RasterSupport.SetLicense(licenseFilePath, developerKey);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.SetLicense", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }

            if (RasterSupport.KernelExpired)
            {
                string[] dirs =
                {
               System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
#if !NET
               ,System.IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath)
#endif
            };

                var lic = GetCommonLicPath();

                if (!string.IsNullOrEmpty(lic))
                {
                    string developerKey = System.IO.File.ReadAllText(lic + ".key");
                    try
                    {
                        RasterSupport.SetLicense(lic, developerKey);
                    }
                    catch (Exception ex)
                    {
                        logging.EventLogWriteEntry(appName, "OCRReader.SetLicense", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                        throw ex;
                    }
                }
            }

            if (RasterSupport.KernelExpired)
            {
                if (silent == false)
                {
                    string msg = "Your license file is missing, invalid or expired. LEADTOOLS will not function. Please contact LEAD Sales for information on obtaining a valid license.";
                    string logmsg = string.Format("*** NOTE: {0} ***{1}", msg, Environment.NewLine);
                    System.Diagnostics.Debugger.Log(0, null, "*******************************************************************************" + Environment.NewLine);
                    System.Diagnostics.Debugger.Log(0, null, logmsg);
                    System.Diagnostics.Debugger.Log(0, null, "*******************************************************************************" + Environment.NewLine);

                    logging.EventLogWriteEntry(appName, "OCRReader.SetLicense", TransactionID + " - Exception: " + msg, EventLogEntryType.Error);


#if LT_MP
               Console.WriteLine("Or to acquire an evaluation license, please refer to  https://www.leadtools.com/portal/evaluation?evallicenseonly=true");
#else
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://www.leadtools.com/portal/evaluation?evallicenseonly=true") { UseShellExecute = true });
#endif // #if LT_MP
                }

                return false;
            }
            return true;
        }

        private string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            string dirtyString = str.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) // || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        private string GetCommonLicPath()
        {

            const int levels = 10;

            string[] dirs =
            {
               Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
#if !NET
               Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath),
#endif // #if !NET
         };

            foreach (string baseDir in dirs)
            {
                string currentDir = baseDir;
                for (var i = 0; i < levels; i++)
                {
                    currentDir = Path.Combine(currentDir, "..");

                    var path = Path.GetFullPath(Path.Combine(currentDir, "Support", "Common", "License", "LEADTOOLS.lic"));
                    if (File.Exists(path))
                        return path;
                }
            }

            return String.Empty;
        }
        public bool SetLicense()
        {
            try
            {
                return SetLicense(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public System.Collections.ArrayList ExportImagesFromPDFToArrayList(string pdfSourceFile, string outputFolder, string filenamePattern)
        {

            System.Collections.ArrayList retValue = new System.Collections.ArrayList();
            retValue.Clear();

            try
            {
                retValue = SavePdfToTiff(pdfSourceFile, outputFolder, filenamePattern);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.ExportImagesFromPDFToArrayList", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }

            return retValue;
        }
        private System.Collections.ArrayList SavePdfToTiff(string fileName, string outputFolder, string filenamePattern)
        {
            logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "1", EventLogEntryType.Information);
            try
            {
                string MY_LICENSE_FILE = @"E:\License_BarcodeOCR\LEADTOOLS.LIC";
                string MY_DEVELOPER_KEY = System.IO.File.ReadAllText(@"E:\License_BarcodeOCR\LEADTOOLS.LIC.key");
                RasterSupport.SetLicense(MY_LICENSE_FILE, MY_DEVELOPER_KEY);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "2", EventLogEntryType.Information);
                logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
            logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "3", EventLogEntryType.Information);
            try
            {
                RasterCodecs rasterCodecs = new RasterCodecs();
                rasterCodecs.Options.RasterizeDocument.Load.Resolution = 300;
                logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "4", EventLogEntryType.Information);

                RasterImage rasterImage = null;
                rasterImage = rasterCodecs.Load(fileName);
                logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "5" + fileName, EventLogEntryType.Information);

                System.Collections.ArrayList retValue = new System.Collections.ArrayList();

                logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "6", EventLogEntryType.Information);
                for (int i = 1; i <= rasterImage.PageCount; i++)
                {
                    logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "7", EventLogEntryType.Information);
                    string outputFile = System.String.Format(Path.Combine(outputFolder, filenamePattern + "-{0}.jpg"), i);
                    logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "8" + outputFile, EventLogEntryType.Information);
                    rasterCodecs.Save(rasterImage, new System.Uri(outputFile), RasterImageFormat.Jpeg, rasterImage.BitsPerPixel, i, i);
                    logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "9", EventLogEntryType.Information);
                    //rasterCodecs.Save(rasterImage, new System.Uri(outputFile), RasterImageFormat.Bmp, 8, i, i);
                    retValue.Add(outputFile);
                    logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "10", EventLogEntryType.Information);
                }
                logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", "11", EventLogEntryType.Information);
                return retValue;
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "OCRReader.SavePdfToTiff", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
        }
    }
}
