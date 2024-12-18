using Leadtools.Barcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mibfa.OCR.Service.Library
{
    // Each page has a list of barcodes and a current selected barcode index
    public class PageBarcodes
    {
        public PageBarcodes()
        {
            _barcodes = new List<Leadtools.Barcode.BarcodeData>();
            _selectedIndex = -1;
        }

        private IList<Leadtools.Barcode.BarcodeData> _barcodes;
        public IList<Leadtools.Barcode.BarcodeData> Barcodes
        {
            get { return _barcodes; }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }
    }

    // A document has a list of PageBarcodes
    public class DocumentBarcodes
    {
        public DocumentBarcodes()
        {
            _pages = new List<PageBarcodes>();
        }

        private IList<PageBarcodes> _pages;
        public IList<PageBarcodes> Pages
        {
            get { return _pages; }
        }
    }
}
