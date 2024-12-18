using System.ServiceProcess;

namespace Mibfa.OCR.Service.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new svcOCR() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
