using Leadtools;
using Leadtools.Barcode;
using Leadtools.Codecs;
using Leadtools.Controls;
using Leadtools.Demos;
using Leadtools.Demos.Dialogs;
using Leadtools.ImageProcessing;
using Leadtools.ImageProcessing.Core;
using Leadtools.Twain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Mibfa.OCR.Service.Library
{
    public class BarcodeReader : IDisposable
    {        
        private RasterCodecs _rasterCodecs; // The RasterCodecs instance used to load/save images
        private BarcodeEngine _barcodeEngine; // The Barcode engine
        private DocumentBarcodes _documentBarcodes; // Barcodes read or written in this document

        //private string MY_LICENSE_FILE = @"E:\License_BarcodeOCR\LEADTOOLS.LIC";
        //private string MY_DEVELOPER_KEY = System.IO.File.ReadAllText(@"E:\License_BarcodeOCR\LEADTOOLS.LIC.key");
        private string appName = "MIBFA.Barcode";
        private string LoggingWS = "";
        private string TransactionID = "";
        private string TempFolder = "";
        private Logging logging = new Logging(@"E:\Program Files\Oculus 10\Logs");
        private string logsFolder = "";
        private bool _disposed;
        ~BarcodeReader() => Dispose();
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this._rasterCodecs?.Dispose();
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public BarcodeReader(string loggingWS, string logFolder, string transactionID, string tempFolder)
        {
            LoggingWS = loggingWS;
            TransactionID = transactionID;
            TempFolder = tempFolder;
            logsFolder = logFolder;
            //logging = new Logging(logsFolder, LoggingWS);
            logging = new Logging(logsFolder);
        }
        public string[] Read(string documentToRead, int targetPage)
        {
            logging.EventLogWriteEntry(appName, "BarcodeReader.Read", TransactionID + " - documentToRead: " + documentToRead, EventLogEntryType.Information);

            Mibfa.OCR.Service.Library.BarcodesGlobal.InitRuntime();
            // Set the license
            try
            {
                if (!SetLicense())
                {
                    throw new Exception("License not set");
                }
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "BarcodeReader.Read", TransactionID + " - Exception: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }

            try
            {
                System.Collections.ArrayList extractedImages = SplitPDFImages(documentToRead, TempFolder, Path.GetFileNameWithoutExtension(documentToRead));
                string pageImageFile = extractedImages[targetPage - 1].ToString();

                BarcodeDataObject barcodeDataObject = new BarcodeDataObject();
                barcodeDataObject.FilePath = pageImageFile;
                //barcodeDataObject.IsWrite = false;
                barcodeDataObject.EnableDoublePass = false;
                //barcodeDataObject.BarcodeLocation = new LeadRect(0, 0, 0, 0);
                //barcodeDataObject.BarcodeType = BarcodeSymbology.Unknown;
                //barcodeDataObject.OutputFormat = RasterImageFormat.Unknown;

                //Start Barcode Engine
                BarcodeEngine barcodeEngine = new BarcodeEngine();
                BarcodeData[] barcodeData = ReadBarcode(barcodeDataObject, barcodeEngine);
                List<string> barcodes = new List<string>();
                foreach (BarcodeData item in barcodeData)
                {
                    barcodes.Add(item.Value);
                }

                return barcodes.ToArray();
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "BarcodeReader.Read", TransactionID + " - Exception: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
            finally
            {
            }
            // Parse the command line and verify it

        }
        private BarcodeData[] ReadBarcode(BarcodeDataObject barcodeDataObject, BarcodeEngine barcodeEngine)
        {
            try
            {
                // Create RasterCodecs
                RasterCodecs rasterCodecs = new RasterCodecs();
                rasterCodecs.Options.RasterizeDocument.Load.Resolution = 300;

                // Load the raster image
                RasterImage rasterImage = rasterCodecs.Load(barcodeDataObject.FilePath);
                // Enable double pass (even if first pass is successful)
                //if (barcodeDataObject.EnableDoublePass)
                //{
                //    Console.WriteLine("Enable double pass...");
                //    ((OneDBarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.UPCA)).EnableDoublePass = true;
                //    ((DatamatrixBarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.Datamatrix)).EnableDoublePassIfSuccess = true;
                //    ((MicroPDF417BarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.MicroPDF417)).EnableDoublePassIfSuccess = true;
                //    ((PDF417BarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.PDF417)).EnableDoublePassIfSuccess = true;
                //    ((QRBarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.QR)).EnableDoublePassIfSuccess = true;
                //}

                // Set barcode symbology to UNKNOWN
                BarcodeSymbology[] symbologies = { BarcodeSymbology.Code3Of9, BarcodeSymbology.Code128 };

                //if (barcodeDataObject.EnableImagePreprocessing)
                //{
                //    ((OneDBarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.UPCA)).EnablePreprocessing = true;
                //    ((MicroPDF417BarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.MicroPDF417)).EnablePreprocessing = true;
                //    ((PDF417BarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.PDF417)).EnablePreprocessing = true;
                //    ((DatamatrixBarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.Datamatrix)).EnablePreprocessing = true;
                //    ((QRBarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.QR)).EnablePreprocessing = true;
                //}

                // Read Barcode
                //Console.WriteLine("Read Barcode...");
                BarcodeData[] barcodes = barcodeEngine.Reader.ReadBarcodes(rasterImage, LeadRect.Empty, 0, symbologies);

                // If no barcodes found without pre-processing, try again with image processing option
                if (barcodes.Length == 0 && !barcodeDataObject.EnableImagePreprocessing)
                {
                    ((OneDBarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.UPCA)).EnablePreprocessing = true;
                    ((MicroPDF417BarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.MicroPDF417)).EnablePreprocessing = true;
                    ((PDF417BarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.PDF417)).EnablePreprocessing = true;
                    ((DatamatrixBarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.Datamatrix)).EnablePreprocessing = true;
                    ((QRBarcodeReadOptions)barcodeEngine.Reader.GetDefaultOptions(BarcodeSymbology.QR)).EnablePreprocessing = true;

                    barcodes = barcodeEngine.Reader.ReadBarcodes(rasterImage, LeadRect.Empty, 0, symbologies);
                }

                //// Print out the Barcodes we found
                ////ShowMessage($@"The image contains {barcodes.Length} barcodes");
                //for (int i = 0; i < barcodes.Length; i++)
                //{
                //    BarcodeData barcode = barcodes[i];
                //    //ShowMessage($@"{(i + 1)}. {barcode.Symbology} --> {barcode.Value}");
                //}
                return barcodes;
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "BarcodeReader.ReadBarcode", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
        }

        private bool SetLicense(bool silent)
        {
            try
            {
                string licenseFilePath = @"E:\License_BarcodeOCR\LEADTOOLS.LIC";
                string developerKey = System.IO.File.ReadAllText(@"E:\License_BarcodeOCR\LEADTOOLS.LIC.key");
                RasterSupport.SetLicense(licenseFilePath, developerKey);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "BarcodeReader.SetLicense", TransactionID + " - Exception: " + ex.ToString(), EventLogEntryType.Error);
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
                        logging.EventLogWriteEntry(appName, "BarcodeReader.SetLicense", TransactionID + " - Exception: " + ex.ToString(), EventLogEntryType.Error);
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

                    logging.EventLogWriteEntry(appName, "BarcodeReader.SetLicense", TransactionID + " - Exception: " + msg, EventLogEntryType.Error);


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
        private System.Collections.ArrayList SplitPDFImages(string sInputFullPath, string tempFolderPath, string attachmentPrefix)
        {
            System.Collections.ArrayList retValue = new System.Collections.ArrayList();
            retValue.Clear();

            //Oculus.pdf
            //create a new GUID for the images
            string filenamePattern = attachmentPrefix + System.Guid.NewGuid().ToString();

            retValue = ExportImagesFromPDFToArrayList(sInputFullPath, tempFolderPath, filenamePattern);

            //AMA.Util.TiffManager tiffMgr = new AMA.Util.TiffManager(sInputFullPath);
            //retValue = tiffMgr.SplitTiffImage(tempFolderPath, System.Drawing.Imaging.EncoderValue.CompressionNone);

            return retValue;

        }
        public System.Collections.ArrayList ExportImagesFromPDFToArrayList(string pdfSourceFile, string outputFolder, string filenamePattern)
        {

            System.Collections.ArrayList retValue = new System.Collections.ArrayList();
            retValue.Clear();

            retValue = SavePdfToTiff(pdfSourceFile, outputFolder, filenamePattern);

            return retValue;
        }
        private System.Collections.ArrayList SavePdfToTiff(string fileName, string outputFolder, string filenamePattern)
        {
            try
            {
                string MY_LICENSE_FILE = @"E:\License_BarcodeOCR\LEADTOOLS.LIC";
                string MY_DEVELOPER_KEY = System.IO.File.ReadAllText(@"E:\License_BarcodeOCR\LEADTOOLS.LIC.key");
                RasterSupport.SetLicense(MY_LICENSE_FILE, MY_DEVELOPER_KEY);
            }
            catch (Exception ex)
            {
                logging.EventLogWriteEntry(appName, "BarcodeReader.SavePdfToTiff", TransactionID + " - Error: " + ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }

            // Create RasterCodecs
            RasterCodecs rasterCodecs = new RasterCodecs();
            rasterCodecs.Options.RasterizeDocument.Load.Resolution = 300;

            RasterImage rasterImage = null;
            rasterImage = rasterCodecs.Load(fileName);

            System.Collections.ArrayList retValue = new System.Collections.ArrayList();

            for (int i = 1; i <= rasterImage.PageCount; i++)
            {
                string outputFile = System.String.Format(Path.Combine(outputFolder, filenamePattern + "-{0}.jpg"), i);
                rasterCodecs.Save(rasterImage, new System.Uri(outputFile), RasterImageFormat.Jpeg, rasterImage.BitsPerPixel, i, i);
                //rasterCodecs.Save(rasterImage, new System.Uri(outputFile), RasterImageFormat.Bmp, 8, i, i);
                retValue.Add(outputFile);
            }
            return retValue;
        }

    }
    public class BarcodeDataObject
    {
        public bool IsWrite;                   // Flag to specify the barcode operation
        public string FilePath;                // File name - the image file to be read from or written on
        public BarcodeSymbology BarcodeType;   // Barcode type - the barcode symbology to be written on the image file
        public string BarcodeData;             // Barcode data - the data to be written on the image
        public LeadRect BarcodeLocation;       // Barcode location - the location of the written barcode
        public bool EnableDoublePass;          // Flag to enable double pass
        public RasterImageFormat OutputFormat; // RasterImageFormat - the format to save the written barcode
        public bool EnableImagePreprocessing;  // Flag to enable using image pre-processing during recognition
    }
}
