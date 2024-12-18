using System.ComponentModel;
using System.Configuration.Install;


namespace Mibfa.OCR.Service.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
