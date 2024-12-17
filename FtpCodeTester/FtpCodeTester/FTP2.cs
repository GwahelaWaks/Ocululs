using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Oculus10.FTPandMail.Library
{
    public partial class FTP2
    {
        public void Upload(string host, string user, string password, string localfile, bool usebin, bool usepassive)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(host);
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.Credentials = new NetworkCredential(user, password);
            req.UseBinary = usebin;
            req.UsePassive = usepassive;
            if (usebin)
            {
                Stream instm = new FileStream(localfile, FileMode.Open,
                FileAccess.Read);
                Stream outstm = req.GetRequestStream();
                byte[] b = new byte[10000];
                int n;
                while ((n = instm.Read(b, 0, b.Length)) > 0)
                {
                    outstm.Write(b, 0, n);
                }
                instm.Close();
                outstm.Close();
            }
            else
            {
                StreamReader sr = new StreamReader(localfile);
                StreamWriter sw = new StreamWriter(req.GetRequestStream());
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                }
                sr.Close();
                sw.Close();
            }
            FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
            //Console.WriteLine(resp.StatusCode);
        }
    }
}
public class FtpConnection
    {
        private TcpClient ctrl;
        private NetworkStream ctrlstm;
        private StreamWriter ctrlwrt;
        private StreamReader ctrlrdr;
        private TcpClient data;
        private NetworkStream datastm;
        private void SendCmd(string cmd)
        {
            ctrlwrt.WriteLine(cmd);
            ctrlwrt.Flush();
        }
        private string ReceiveStatus()
        {
            StringBuilder sb = new StringBuilder();
            string line;
            do
            {
                line = ctrlrdr.ReadLine();
                sb.Append(line + Environment.NewLine);
            }
            while((line.Length < 3) || ((line.Length != 3) && (line[3] == '-')));
            return sb.ToString();
        }
        public string Command(string cmd)
        {
            SendCmd(cmd);
            return ReceiveStatus();
        }
        private void SetupData()
        {
            string dataaddr = Command("PASV");
            string[] addrparts = dataaddr.Split("()".ToCharArray())[1].Split(",".ToCharArray());
            string datahost = addrparts[0] + "." + addrparts[1] + "." + addrparts[2] + "." + addrparts[3];
            int dataport = int.Parse(addrparts[4]) * 256 + int.Parse(addrparts[5]);
            data = new TcpClient(datahost, dataport);
            datastm = data.GetStream();
        }
        public FtpConnection(string host, string username, string password)
        {
            ctrl = new TcpClient(host, 21);   
            ctrlstm = ctrl.GetStream();
            ctrlwrt = new StreamWriter(ctrlstm, Encoding.Default);
            ctrlrdr = new StreamReader(ctrlstm, Encoding.Default);
            ReceiveStatus();
            Command("USER " + username);
            Command("PASS " + password);
        }
        public string Dir()
        {
            StringBuilder sb = new StringBuilder();
            SetupData();
            Command("LIST");
            StreamReader datardr = new StreamReader(datastm);
            string line;
            while((line = datardr.ReadLine()) != null)
            {
                sb.Append(line + Environment.NewLine);
            }
            datardr.Close();
            datastm.Close();
            data.Close();
            ReceiveStatus();
            return sb.ToString();
        }
        public string ShortDir()
        {
            StringBuilder sb = new StringBuilder();
            SetupData();
            Command("NLST");
            StreamReader datardr = new StreamReader(datastm);
            string line;
            while((line = datardr.ReadLine()) != null)
            {
                sb.Append(line + Environment.NewLine);
            }
            datardr.Close();
            datastm.Close();
            data.Close();
            ReceiveStatus();
            return sb.ToString();
        }
        public void MkDir(string dir)
        {
            Command("MKD " + dir);
        }
        public void RmDir(string dir)
        {
            Command("RMD " + dir);
        }
        public void ChDir(string dir)
        {
            Command("CWD " + dir);
        }
        public void UpLoad(string filename, bool binary)
        {
            if(binary)
            {
                Command("TYPE I");
                SetupData();
                Command("STOR " + filename);
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                byte[] b = new byte[100000];
                int n;
                while((n = fs.Read(b, 0, b.Length)) > 0)
                {
                    datastm.Write(b, 0, n);
                }
                fs.Close();
                datastm.Close();
                data.Close();
                ReceiveStatus();
            }
            else
            {
                Command("TYPE A");
                SetupData();
                Command("STOR " + filename);
                StreamReader sr = new StreamReader(filename, Encoding.Default);
                StreamWriter datawrt = new StreamWriter(datastm, Encoding.Default);
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    datawrt.WriteLine(line);
                }
                sr.Close();
                datawrt.Close();
                datastm.Close();
                data.Close();
                ReceiveStatus();
            }
        }
        public void DownLoad(string filename, bool binary)
        {
            if(binary)
            {
                Command("TYPE I");
                SetupData();
                Command("RETR " + filename);
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                byte[] b = new byte[100000];
                int n;
                while((n = datastm.Read(b, 0, b.Length)) > 0)
                {
                    fs.Write(b, 0, n);
                }
                fs.Close();
                datastm.Close();
                data.Close();
                ReceiveStatus();
            }
            else
            {
                Command("TYPE A");
                SetupData();
                Command("RETR " + filename);
                StreamReader datardr = new StreamReader(datastm, Encoding.Default);
                StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);
                string line;
                while((line = datardr.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                }
                sw.Close();
                datardr.Close();
                datastm.Close();
                data.Close();
                ReceiveStatus();
            }
        }
        public void Logout()
        {
            Command("QUIT");
            ctrlwrt.Close();
            ctrlrdr.Close();
            ctrlstm.Close();
            ctrl.Close();
        }
        public void XFtpDir()
        {
            SetupData();
            Command("LIST");
        }
        public void XFtpShortDir()
        {
            SetupData();
            Command("NLST");
        }
        public void XFtpUpLoad(string filename, bool binary)
        {
            if(binary)
            {
                Command("TYPE I");
                SetupData();
                Command("STOR " + filename);
            }
            else
            {
                Command("TYPE A");
                SetupData();
                Command("STOR " + filename);
            }
        }
        public void XFtpDownLoad(string filename, bool binary)
        {
            if(binary)
            {
                Command("TYPE I");
                SetupData();
                Command("RETR " + filename);
            }
            else
            {
                Command("TYPE A");
                SetupData();
                Command("RETR " + filename);
            }
        }
        public Stream XFtpGetStream()
        {
            return datastm;
        }
        public void XFtpEnd()
        {
            ReceiveStatus();
            datastm.Close();
            data.Close();
        }
    }
    public class XFtpWebRequestCreate : IWebRequestCreate
    {
        public WebRequest Create(Uri url)
        {
            return new XFtpWebRequest(url.Host, url.AbsolutePath);
        }
    }
    public class XFtpWebRequest : WebRequest
    {
        private string host;
        private string dir;
        private string fnm;
        private ICredentials cred;
        private bool usebin;
        private bool usepas;
        private string method;
        private FtpConnection con;
        public XFtpWebRequest(string host, string path)
        {
            this.host = host;
            if(Path.GetDirectoryName(path) != null) 
            {
                this.dir = Path.GetDirectoryName(path).Replace(@"\", "/");
            }
            else
            {
                this.dir = path;
            }
            this.fnm = Path.GetFileName(path);
        }
        public override Stream GetRequestStream()
        {
            if(!usepas)
            {
                throw new Exception("Only passive mode supported");
            }
            con = new FtpConnection(host, ((NetworkCredential)cred).UserName, ((NetworkCredential)cred).Password);
            con.ChDir(dir);
            if(method == WebRequestMethods.Ftp.UploadFile) 
            {
                con.XFtpUpLoad(fnm, usebin);
            }
            else
            {
                throw new Exception("Method " + method + " is not supported");
            }
            return con.XFtpGetStream();
        }
        public override WebResponse GetResponse()
        {
            if(!usepas)
            {
                throw new Exception("Only passive mode supported");
            }
            if(method == WebRequestMethods.Ftp.UploadFile) 
            {
                con.XFtpGetStream().Close();
                return new XFtpWebResponse(con);
            }
            con = new FtpConnection(host, ((NetworkCredential)cred).UserName, ((NetworkCredential)cred).Password);
            con.ChDir(dir);
            if(method == WebRequestMethods.Ftp.ListDirectoryDetails) 
            {
                con.XFtpDir();
            }
            else if(method == WebRequestMethods.Ftp.ListDirectory) 
            {
                con.XFtpShortDir();
            }
            else if(method == WebRequestMethods.Ftp.DownloadFile) 
            {
                con.XFtpDownLoad(fnm, usebin);
            }
            else
            {
                throw new Exception("Method " + method + " is not supported");
            }
            return new XFtpWebResponse(con);
        }
        public bool UseBinary
        {
            get
            {
                return usebin;
            }
            set
            {
                usebin = value;
            }
        }
        public bool UsePassive
        {
            get
            {
                return usepas;
            }
            set
            {
                usepas = value;
            }
        }
        public override ICredentials Credentials 
        {
            get
            {
                return cred;
            }
            set 
            {
                cred = value;
            }
        }
        public override string Method
        {
            get
            {
                return method;
            }
            set 
            {
                method = value;
            }
        }
    }
    public class XFtpWebResponse : WebResponse
    {
        private FtpConnection con;
        public XFtpWebResponse(FtpConnection con)
        {
            this.con = con;
        }
        public override Stream GetResponseStream()
        {
            return con.XFtpGetStream();
        }
        public override void Close()
        {
            con.XFtpEnd();
            con.Logout();
        }
    }
    public class MainClass
    {
        private static void TestUpload(string host, string user, string password, string file)
        {
            XFtpWebRequest req = (XFtpWebRequest)WebRequest.Create(host); //"xftp://ftp.xxxx.dk/xftp.cs");
            req.Credentials = new NetworkCredential(user, password);
            req.UseBinary = false;
            req.UsePassive = true;
            req.Method = WebRequestMethods.Ftp.UploadFile;
            StreamReader sr = new StreamReader(file);
            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sw.WriteLine(line);
            }
            sr.Close();
            sw.Close();
            XFtpWebResponse resp = (XFtpWebResponse)req.GetResponse();
            resp.Close();
        }
        public static void Upload(string host, string user, string password, string file)
        {
            WebRequest.RegisterPrefix("xftp", new XFtpWebRequestCreate());
            TestUpload(host, user, password, file);
        }
    }
