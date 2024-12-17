using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Reflection;

namespace Mibfa.OCR.Service.Library
{
    public class Logging
    {
        private string logFolder = "";
        private string logWSurl = "";
        public Logging(string LogFolder)
        {
            logFolder = LogFolder;
            logWSurl = "";
        }
        public Logging(string LogFolder, string LogWSURL)
        {
            logFolder = LogFolder;
            logWSurl = LogWSURL;
        }
        public void EventLogWriteEntry(string application, string message, EventLogEntryType eventLogEntryType)
        {
            StackTrace stackTrace = new StackTrace();
            MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
            WriteLogFileEntry(application, methodBase.Name, message);
        }
        public void EventLogWriteEntry(string application, string transactionID, string message, EventLogEntryType eventLogEntryType)
        {
            StackTrace stackTrace = new StackTrace();
            MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
            //Console.WriteLine(methodBase.Name); // e.g.

            if (!string.IsNullOrEmpty(logFolder))
            {
                WriteLogFileEntry(application, methodBase.Name, message);
            }
            //            return;

            if (!string.IsNullOrEmpty(logWSurl))
            {
                wsLogService.LogService logging = new wsLogService.LogService();
                try
                {
                    logging.Url = logWSurl; // GetApplicationSetting("LoggingWS");
                    if (logWSurl.ToLower().StartsWith("https:"))
                    {
                        System.Net.ServicePointManager.Expect100Continue = true;
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender1, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return (true); };
                    }
                    string entryType = "";
                    switch (eventLogEntryType)
                    {
                        case EventLogEntryType.Error:
                            entryType = "Error";
                            break;
                        case EventLogEntryType.FailureAudit:
                            entryType = "FailureAudit";
                            break;
                        case EventLogEntryType.Information:
                            entryType = "Information";
                            break;
                        case EventLogEntryType.SuccessAudit:
                            entryType = "SuccessAudit";
                            break;
                        case EventLogEntryType.Warning:
                            entryType = "Warning";
                            break;
                        default:
                            entryType = "General";
                            break;
                    }

                    logging.WriteLogEntry(application, methodBase.Name, transactionID + " - " + message, entryType);
                }
                catch (Exception ex)
                {
                    WriteLogFileEntry(application, "EventLogWriteEntry", "Exception occured during saving log entry to database: " + ex.Message);
                }
                finally
                {
                    try
                    {
                        logging.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        private void WriteLogFileEntry(string application, string method, string message)
        {
            DateTime logEventTime = DateTime.Now;
            int retry = 0;
            while (retry < 5)
            {
                try
                {
                    if (!Directory.Exists(logFolder)) Directory.CreateDirectory(logFolder);
                    string logFile = Path.Combine(logFolder, application + "-" + logEventTime.ToString("yyyyMMdd") + "_log.csv");
                    bool logExists = File.Exists(logFile);
                    StreamWriter writetext = new StreamWriter(logFile, true);
                    //writetext.WriteLine(DateTime.Now.ToString() + " - " + methodBase.Name + " - " + message);
                    if (!logExists)
                    {
                        writetext.WriteLine("sep=|");
                        writetext.WriteLine("DATE_LOGGED|METHOD|LOG_MESSAGE");
                    }
                    writetext.WriteLine(logEventTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + "|" + method + "|" + message);
                    writetext.Close();
                    return;
                }
                catch (Exception ex)
                {
                    GC.Collect();
                    Thread.Sleep(250);
                    retry++;
                    if (retry == 20)
                        return;
                }
            }
        }
    }
}