using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
//using TestBarcodeReaderAndPDFSplitting;

namespace Mibfa.OCR.Service.Library
{
    public class PDFSplitter
    {
        //////public int ExportImagesFromPDF(string pdfSourceFile, string outputFolder, string filenamePattern)
        //////{
        //////    PdfDocument document = PdfReader.Open(pdfSourceFile);
        //////    int imageCount = 0;

        //////    try
        //////    {

        //////        // Iterate pages
        //////        foreach (PdfPage page in document.Pages)
        //////        {
        //////            // Get resources dictionary
        //////            PdfDictionary resources = page.Elements.GetDictionary("/Resources");
        //////            if (resources != null)
        //////            {
        //////                // Get external objects dictionary
        //////                PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
        //////                if (xObjects != null)
        //////                {

        //////                    System.Collections.Generic.ICollection<PdfItem> items = xObjects.Elements.Values;
        //////                    // Iterate references to external objects
        //////                    foreach (PdfItem item in items)
        //////                    {
        //////                        PdfReference reference = item as PdfReference;
        //////                        if (reference != null)
        //////                        {
        //////                            PdfDictionary xObject = reference.Value as PdfDictionary;
        //////                            // Is external object an image?
        //////                            if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
        //////                            {
        //////                                imageCount++;
        //////                                ExportImage(xObject, imageCount, outputFolder, filenamePattern);
        //////                            }
        //////                        }
        //////                    }
        //////                }
        //////            }
        //////        }
        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        System.Diagnostics.EventLog.WriteEntry(TestBarcodeReaderAndPDFSplitting.Form1.m_ApplicationName, "Export Images From PDF: " + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Information);
        //////        throw ex;
        //////    }
        //////    return imageCount;
        //////}
        public System.Collections.ArrayList ExportImagesFromPDFToArrayList(string pdfSourceFile, string outputFolder, string filenamePattern)
        {
            PdfDocument document =  PdfReader.Open(pdfSourceFile);
            int imageCount = 0;

            System.Collections.ArrayList retValue = new System.Collections.ArrayList();
            retValue.Clear();

            try
            {
                // Iterate pages
                foreach (PdfPage page in document.Pages)
                {
                    // Get resources dictionary
                    PdfDictionary resources = page.Elements.GetDictionary("/Resources");
                    if (resources != null)
                    {
                        // Get external objects dictionary
                        PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
                        if (xObjects != null)
                        {
                            System.Collections.Generic.ICollection<PdfSharp.Pdf.PdfItem> items = xObjects.Elements.Values;
                            // Iterate references to external objects

                            foreach (PdfItem item in items)
                            {
                                PdfReference reference = item as PdfReference;
                                if (reference != null)
                                {
                                    PdfDictionary xObject = reference.Value as PdfDictionary;
                                    // Is external object an image?
                                    if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
                                    {
                                        imageCount++;
                                        //now add this new filename to the arraylist
                                        string newFileName = "";

                                        newFileName = ExportImageGetFileName(xObject, imageCount, outputFolder, filenamePattern);
                                        retValue.Add(newFileName);
                                    }
                               }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
//                EventLogWriteEntry(m_ApplicationName, "ExportImagesFromPDFToArrayList", "Export PDF Images to arraylist: " + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Information);
                throw ex;
            }
            return retValue;
        }
        static string ExportImageGetFileName(PdfDictionary image, int count, string outputFolder, string filenamePattern)
        {
            string retValue = "";
            string filter = image.Elements.GetName("/Filter");
            switch (filter)
            {
                case "/DCTDecode":
                    retValue = ExportJpegImageWithName(image, count, outputFolder, filenamePattern);
                    break;
                //case "/FlateDecode":
                //    //ExportAsPngImage(image, ref count);
                //    break;
                case "/CCITTFaxDecode":
                    retValue = ExportCCITTImageWithName(image, count, outputFolder, filenamePattern);
                    break;
                default:
                    throw new InvalidDataException("Unsupported PDF filter encountered: " + filter);
            }
            return retValue;
        }
        static string ExportCCITTImageWithName(PdfDictionary image, int count, string outputFolder, string filenamePattern)
        {
            string retValue = "";

            UInt32 width = UInt32.Parse(image.Elements["/Width"].ToString());
            UInt32 height = UInt32.Parse(image.Elements["/Height"].ToString());
            UInt32 bitsPerComponent = UInt32.Parse(image.Elements["/BitsPerComponent"].ToString());

            //var colorSpace = image.Elements["/ColorSpace"].ToString();

            var compression = BitMiracle.LibTiff.Classic.Compression.CCITTFAX4;
            //  var compression = BitMiracle.LibTiff.Classic.Compression.;
          

            string fileName = System.String.Format(Path.Combine(outputFolder, filenamePattern) + "{0}.tif", count);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            byte[] stream2 = image.Stream.Value;

            ////FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            ////BinaryWriter bw = new BinaryWriter(fs);
            ////bw.Write(stream);
            ////bw.Close();

            //get the colorspace as well 


            using(var tiffImage = BitMiracle.LibTiff.Classic.Tiff.Open(fileName,"w"))
            {
                tiffImage.SetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGEWIDTH, width);
                tiffImage.SetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGELENGTH, height);
                tiffImage.SetField(BitMiracle.LibTiff.Classic.TiffTag.COMPRESSION, compression);
                tiffImage.SetField(BitMiracle.LibTiff.Classic.TiffTag.BITSPERSAMPLE, bitsPerComponent);
          
                tiffImage.SetField(BitMiracle.LibTiff.Classic.TiffTag.SAMPLESPERPIXEL, 1);
                tiffImage.WriteRawStrip(0, stream2, stream2.Length); //saving the tiff file using the raw bytes retrieved from the PDF.   
                
                
                tiffImage.Close();

            }


            ////////////    var width = image.Elements.Value .GetAsNumber(PdfName.WIDTH).IntValue; //retrieve the image width information
            ////////////    var height = xObjectInfo.GetAsNumber(PdfName.HEIGHT).IntValue; //retrieve the image height information
            ////////////    var bitsPerComponent = xObjectInfo.GetAsNumber(PdfName.BITSPERCOMPONENT).IntValue; //retrieve the BPC information used in the image
            ////////////    var compression = Compression.CCITTFAX3; //the compression type, you might want to declare this on a drop down box option when reading. 
            ////////////    //there are two CCITTFAX compression: CCITTFAX3 and CCITTFAX4. These options are usually present on the scanning options for most Canon Scanners
            ////////////    var tiffFile = "C:\temp\extractedimage.tiff"; //the tiff file name where to save
            ////////////    using (var tiff = Tiff.Open(tiffFile, "w"))
            ////////////    {   //set all the necessary properties   
            ////////////        tiff.SetField(TiffTag.IMAGEWIDTH, width);   
            ////////////        tiff.SetField(TiffTag.IMAGELENGTH, height); 
            ////////////        tiff.SetField(TiffTag.COMPRESSION, compression); 
            ////////////        tiff.SetField(TiffTag.BITSPERSAMPLE, bitsPerComponent);  
            ////////////        tiff.SetField(TiffTag.SAMPLESPERPIXEL, 1);   
            ////////////        tiff.WriteRawStrip(0, imageBytes, imageBytes.Length); //saving the tiff file using the raw bytes retrieved from the PDF.   
            ////////////        tiff.Close();
            ////////////    }


            retValue = fileName;

            return retValue;
        }
        static string ExportJpegImageWithName(PdfDictionary image, int count, string outputFolder, string filenamePattern)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
            string retValue = "";
            byte[] stream = image.Stream.Value;
            string fileName = System.String.Format(Path.Combine(outputFolder, filenamePattern) + "{0}.jpg", count);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(stream);
            bw.Close();

            //now, we need to convert this JPEG we have into a 1bpp (Black and White) TIFF
            //jmd
            //retValue = Create_TiffWithFileName(fileName, count, outputFolder, filenamePattern);
            retValue = fileName;

            //the JPEG file is just a temp file so we have to delete it no matter what the outcome of the conversion was
            //jmd
            //File.Delete(fileName);

            return retValue;
        }
        static string ExportTIFFImageWithName(PdfDictionary image, int count, string outputFolder, string filenamePattern)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
            string retValue = "";
            byte[] stream = image.Stream.Value;
            string fileName = System.String.Format(Path.Combine(outputFolder, filenamePattern) + "{0}.tif", count);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(stream);
            bw.Close();

            //now, we need to convert this JPEG we have into a 1bpp (Black and White) TIFF
           // retValue = Create_TiffWithFileName(fileName, count, outputFolder, filenamePattern);
            retValue = fileName;
            //the JPEG file is just a temp file so we have to delete it no matter what the outcome of the conversion was
           

            return retValue;
        }
        static void ExportJpegImage(PdfDictionary image, int count, string outputFolder, string filenamePattern)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
            byte[] stream = image.Stream.Value;
            string fileName = System.String.Format(Path.Combine(outputFolder, filenamePattern) + "{0}.jpeg", count);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(stream);
            bw.Close();

            //now, we need to convert this JPEG we have into a 1bpp (Black and White) TIFF
            Create_Tiff(fileName, count, outputFolder, filenamePattern);

            //the JPEG file is just a temp file so we have to delete it no matter what the outcome of the conversion was
            File.Delete(fileName);
        }
        static void Create_Tiff(string fileName, int count, string outputFolder, string filenamePattern)
        {
            using (FileStream fs = new FileStream(System.String.Format(Path.Combine(outputFolder, filenamePattern) + "{0}.tif", count), FileMode.Create, FileAccess.Write))
            {
                TiffBitmapEncoder tifEnc = new TiffBitmapEncoder();
                tifEnc.Compression = TiffCompressOption.None;
                //foreach (string fileName in list_Files)
                //{
                //BitmapImage bmpImg = new BitmapImage(new Uri(fileName));
                BitmapImage bmpImg = new BitmapImage();
                bmpImg.BeginInit();
                bmpImg.CacheOption = BitmapCacheOption.OnLoad;
                bmpImg.UriSource = new System.Uri(fileName);
                bmpImg.EndInit();
                FormatConvertedBitmap fcb = new FormatConvertedBitmap(bmpImg,
                                                                      PixelFormats.BlackWhite,
                                                                      BitmapPalettes.BlackAndWhite,
                                                                      1.0);
                tifEnc.Frames.Add(BitmapFrame.Create(fcb));
                //}
                tifEnc.Save(fs);
            }
        }
        static string Create_TiffWithFileName(string fileName, int count, string outputFolder, string filenamePattern)
        {
            string retValue = System.String.Format(Path.Combine(outputFolder, filenamePattern) + "{0}.tif", count);

            try
            {
                using (FileStream fs = new FileStream(System.String.Format(Path.Combine(outputFolder, filenamePattern) + "{0}.tif", count), FileMode.Create, FileAccess.Write))
                {
                    TiffBitmapEncoder tifEnc = new TiffBitmapEncoder();
                    tifEnc.Compression = TiffCompressOption.None;
                    //foreach (string fileName in list_Files)
                    //{
                    //BitmapImage bmpImg = new BitmapImage(new Uri(fileName));
                    BitmapImage bmpImg = new BitmapImage();
                    bmpImg.BeginInit();
                    bmpImg.CacheOption = BitmapCacheOption.OnLoad;
                    bmpImg.UriSource = new System.Uri(fileName);
                    bmpImg.EndInit();
                    FormatConvertedBitmap fcb = new FormatConvertedBitmap(bmpImg,
                                                                          PixelFormats.BlackWhite,
                                                                          BitmapPalettes.BlackAndWhite,
                                                                          1.0);
                    tifEnc.Frames.Add(BitmapFrame.Create(fcb));
                    //}
                    tifEnc.Save(fs);
                }

            }
            catch (System.Exception ex)
            {
                retValue = "";
            }


            return retValue;
        }
    }
}
