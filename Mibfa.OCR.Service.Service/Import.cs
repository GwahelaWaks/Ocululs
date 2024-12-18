using System.ServiceProcess;

namespace Mibfa.OCR.Service.Service
{
    public partial class svcOCR : ServiceBase
    {
        public svcOCR()
        {
            InitializeComponent();
        }

        //private   Europcar.License.Processor.Library.LicenseProcessing m_oOculusDriversLic;
        private Mibfa.OCR.Service.Library.Library m_OculusOCR;
        protected override void OnStart(string[] args)
        {

            m_OculusOCR = new Mibfa.OCR.Service.Library.Library();
            m_OculusOCR.StartProcessing();

        }

        protected override void OnStop()
        {
            m_OculusOCR.StopProcessing();

        }
    }
}
