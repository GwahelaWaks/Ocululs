﻿using Leadtools.Codecs;
using Leadtools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mibfa.OCR.Service.Library
{
    public static class BarcodesGlobal
    {
#if LTV21_CONFIG
      public static int LTVersion = 21;
#endif
#if LTV22_CONFIG
      public static int LTVersion = 22;
#endif
#if LTV23_CONFIG
      public static int LTVersion = 23;
#endif

#if !LT_MP
        static BarcodesGlobal()
        {
            RegistryKey rk = OpenSoftwareKey(@"Microsoft\InetStp");

            if (rk != null)
            {
                object major = rk.GetValue("MajorVersion");
                object minor = rk.GetValue("MinorVersion");

                rk.Close();

                if (major != null && minor != null)
                {
                    _IISMajorVersionNumber = (int)major;
                    _IISVersion = major.ToString() + "." + minor.ToString();
                }
                else
                {
                    _IISMajorVersionNumber = 0;
                }
            }
        }

        public static bool IsOnXP
        {
            get
            {
                return Environment.OSVersion.Version.Major == 5;
            }
        }

        public static bool IsOnVista
        {
            get
            {
                return Environment.OSVersion.Version.Major >= 6;
            }
        }

        public static bool IsOnWindows7
        {
            get
            {
                return (Environment.OSVersion.Version.Major > 6) ||
                     (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1);
            }
        }

        public static bool IsOnWindows2003
        {
            get
            {
                return (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 2);
            }
        }

        public static bool IsOnWindows2000
        {
            get
            {
                return (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 0);
            }
        }

        public static bool IsOnVistaOrLater
        {
            get
            {
                OperatingSystem OS = Environment.OSVersion;
                return (OS.Platform == PlatformID.Win32NT) && (OS.Version.Major >= 6);
            }
        }


        public static bool Is64Process()
        {
            return IntPtr.Size == 8;
        }

        public static bool NeedUAC()
        {
            OperatingSystem system = Environment.OSVersion;

            if (system.Platform == PlatformID.Win32NT && system.Version.Major >= 6)
            {
                return true;
            }

            return false;
        }

        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);

            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static bool MustRestartElevated()
        {
            return BarcodesGlobal.NeedUAC() && !BarcodesGlobal.IsAdmin();
        }

        //public static void RestartElevated(string[] args)
        //{
        //   if (System.Diagnostics.Debugger.IsAttached) // Warn the user that we need to restart and that debugging will end.
        //   {
        //      string msg = string.Format("{0}: {1}\n{2}.", DemosGlobalization.AdminPrivilege, Process.GetCurrentProcess().ProcessName, DemosGlobalization.DebuggerWarning);
        //      MessageBox.Show(msg, DemosGlobalization.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //   }

        //   ProcessStartInfo startInfo = new ProcessStartInfo();

        //   startInfo.UseShellExecute = true;
        //   startInfo.WorkingDirectory = Environment.CurrentDirectory;
        //   startInfo.FileName = Application.ExecutablePath;
        //   startInfo.Verb = "runas";
        //   startInfo.Arguments = string.Join(" ", args);
        //   try
        //   {
        //      Process p = Process.Start(startInfo);
        //   }
        //   catch (System.ComponentModel.Win32Exception)
        //   {
        //      return;
        //   }
        //}

        //public static void TryRestartElevated(string[] args)
        //{
        //   foreach (string s in args)
        //   {
        //      if (string.Compare("/restartElevated", s) == 0)
        //      {
        //         string msg = string.Format("{0}: {1}", DemosGlobalization.AdminPrivilege, Process.GetCurrentProcess().ProcessName);
        //         MessageBox.Show(msg, DemosGlobalization.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //         return;
        //      }
        //   }
        //   string[] argsNew = new string[args.Length + 1];
        //   Array.Copy(args, argsNew, args.Length);
        //   argsNew[args.Length] = "/restartElevated";

        //   BarcodesGlobal.RestartElevated(argsNew);
        //}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Runtime.InteropServices.DllImport("shell32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, uint dwFlags, [System.Runtime.InteropServices.Out] System.Text.StringBuilder pszPath);

        private static string GetCommonDocumentsFolder()
        {
            int SIDL_COMMON_DOCUMENTS = 0x002E;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(1024);
            SHGetFolderPath(IntPtr.Zero, SIDL_COMMON_DOCUMENTS, IntPtr.Zero, 0, sb);
            return sb.ToString();
        }

        private static string ReadInstallLocationFromRegistry(string regKey)
        {
            if (string.IsNullOrEmpty(regKey))
            {
                return string.Empty;
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regKey))
            {
                if (key == null)
                {
                    return string.Empty;
                }

                object value = key.GetValue("InstallLocation");

                if (value != null)
                {
                    return value.ToString();
                }
            }
            return string.Empty;
        }

        private static string GetInstallLocation(string regKey)
        {
            string location = ReadInstallLocationFromRegistry(regKey);

            if (location == string.Empty)
            {
                location = Application.StartupPath;
            }

            return location;
        }

        public static string InstallLocation
        {
            get
            {
                string location = string.Empty;
                string regKey = string.Empty;

#if LTV21_CONFIG
            const string englishInstallGuid = @"{352DD5BC-BEA1-43BC-B077-6395B597672A}";
            const string japaneseInstallGuid = @"{A2B5F965-E5B9-4C94-8703-EA552CE774F8}";

            // Try English setup first
            if (Is64Process())
               regKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + englishInstallGuid;
            else
               regKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + englishInstallGuid;

            location = ReadInstallLocationFromRegistry(regKey);

            // If not English setup, try Japanese
            if (string.IsNullOrEmpty(location))
            {
               if (Is64Process())
                  regKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + japaneseInstallGuid;
               else
                  regKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + japaneseInstallGuid;
               location = ReadInstallLocationFromRegistry(regKey);
            }

            // If both Japanese and English setup missing, use startup path.
            if (string.IsNullOrEmpty(location))
            {
               location = Application.StartupPath;
            }
#endif
                return location;
            }
        }

        private static int _IISMajorVersionNumber = -1;

        public static int IISMajorVersionNumber
        {
            get
            {
                return BarcodesGlobal._IISMajorVersionNumber;
            }
        }

        private static string _IISVersion = string.Empty;

        public static string IISVersion
        {
            get { return BarcodesGlobal._IISVersion; }
        }


        public static RegistryKey OpenSoftwareKey(string keyName)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\" + keyName);

            if (key == null)
            {
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + keyName);
                if (key == null)
                {
                    key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + keyName);
                }
            }

            return key;
        }

        public static bool IsDotNet35Installed()
        {
            bool ret = false;
            RegistryKey rk = OpenSoftwareKey(@"\Microsoft\NET Framework Setup\NDP\v3.5");
            if (rk != null)
            {
                ret = true;
                rk.Close();
            }

            else
            {
                ret = false;
            }

            return ret;
        }

        public static bool IsDotNet4Installed()
        {
            bool ret = false;
            RegistryKey rk = OpenSoftwareKey(@"\Microsoft\NET Framework Setup\NDP\v4");
            if (rk != null)
            {
                ret = true;
                rk.Close();
            }

            else
            {
                ret = false;
            }

            return ret;
        }

        public static bool IsDotNet45OrLaterInstalled()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            bool dotNet45Installed = false;
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    dotNet45Installed = true;
                }
                else
                {
                    dotNet45Installed = false;
                }
            }
            return dotNet45Installed;
        }

        public static bool CheckKnown3rdPartyTwainIssues(System.Windows.Forms.IWin32Window window, string sourceName)
        {
            bool thirdPartyTwainWithKnownProblem = false;
            bool continueScan = true;

            // The TWAIN2 FreeImage Software Scanner 64-bit has a problem when running under .NET 4.5 or later, check
            const string twain2FreeImageSourceName = "TWAIN2 FreeImage Software Scanner";
            if (sourceName == twain2FreeImageSourceName && System.IntPtr.Size == 8)
            {
                // Check if we are running under .NET 4.5 or later
                var targetFrameworks = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false);
                if (targetFrameworks != null && targetFrameworks[0] != null)
                {
                    var attr = targetFrameworks[0] as System.Runtime.Versioning.TargetFrameworkAttribute;
                    if (attr != null && !attr.FrameworkName.Contains("v4.0"))
                    {
                        thirdPartyTwainWithKnownProblem = true;
                    }
                }
            }

            if (thirdPartyTwainWithKnownProblem)
            {
                string message = "The 64-bit TWAIN Free Image Scanner virtual TWAIN driver has known compatibility issues with .NET 4.5 and above.\n" +
                                 "If you are using this TWAIN driver as a source with our LEADTOOLS TWAIN SDK and are having any issues, you will need to upgrade the FreeImagex64.dll that is included with the driver to v3.18.0.\n" +
                                 "Another option is to change the target .NET Framework from 4.5 to 4.0 or lower.\n" +
                                 "For more information see: https://www.leadtools.com/support/forum/posts/t12411-";
                DialogResult ret = MessageBox.Show(window, message, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (ret == DialogResult.Cancel)
                {
                    continueScan = false;
                }
            }

            return continueScan;
        }

        public static RectangleF GetBoundingRectangle(PointF[] pts)
        {
            if (pts.Length == 2)
            {
                return RectangleF.FromLTRB(
                   Math.Min(pts[0].X, pts[1].X),
                   Math.Min(pts[0].Y, pts[1].Y),
                   Math.Max(pts[0].X, pts[1].X),
                   Math.Max(pts[0].Y, pts[1].Y));
            }
            else if (pts.Length == 4)
            {
                return RectangleF.FromLTRB(
                   Math.Min(pts[0].X, Math.Min(pts[1].X, Math.Min(pts[2].X, pts[3].X))),
                   Math.Min(pts[0].Y, Math.Min(pts[1].Y, Math.Min(pts[2].Y, pts[3].Y))),
                   Math.Max(pts[0].X, Math.Max(pts[1].X, Math.Max(pts[2].X, pts[3].X))),
                   Math.Max(pts[0].Y, Math.Max(pts[1].Y, Math.Max(pts[2].Y, pts[3].Y))));
            }
            else
            {
                float xMin = pts[0].X;
                float yMin = pts[0].Y;
                float xMax = xMin;
                float yMax = yMin;

                for (int i = 1; i < pts.Length; i++)
                {
                    xMin = Math.Min(xMin, pts[i].X);
                    yMin = Math.Min(yMin, pts[i].Y);
                    xMax = Math.Max(xMax, pts[i].X);
                    yMax = Math.Max(yMax, pts[i].Y);
                }

                return FixRectangle(RectangleF.FromLTRB(xMin, yMin, xMax, yMax));
            }
        }

        public static PointF[] GetBoundingPoints(RectangleF rc)
        {
            return new PointF[]
            {
            new PointF(rc.Left, rc.Top),
            new PointF(rc.Right, rc.Top),
            new PointF(rc.Right, rc.Bottom),
            new PointF(rc.Left, rc.Bottom)
            };
        }

        public static Rectangle GetBoundingRectangle(Point center, Size size)
        {
            return new Rectangle(
               center.X - size.Width / 2,
               center.Y - size.Height / 2,
               size.Width,
               size.Height);
        }

        public static RectangleF GetBoundingRectangle(PointF center, SizeF size)
        {
            return new RectangleF(
               center.X - size.Width / 2.0f,
               center.Y - size.Height / 2.0f,
               size.Width,
               size.Height);
        }

        public static Rectangle GetBoundingRectangle(RectangleF rc)
        {
            return FixRectangle(new Rectangle(
               (int)rc.Left,
               (int)rc.Top,
               (int)Math.Ceiling(rc.Width) + 1,
               (int)Math.Ceiling(rc.Height) + 1));
        }

        public static RectangleF GetBoundingRectangle(PointF pt1, PointF pt2)
        {
            return RectangleF.FromLTRB(
               Math.Min(pt1.X, pt2.X),
               Math.Min(pt1.Y, pt2.Y),
               Math.Max(pt1.X, pt2.X),
               Math.Max(pt1.Y, pt2.Y));
        }

        public static RectangleF FixRectangle(RectangleF rc)
        {
            if (rc.Left > rc.Right || rc.Top > rc.Bottom)
            {
                return RectangleF.FromLTRB(
                   Math.Min(rc.Left, rc.Right),
                   Math.Min(rc.Top, rc.Bottom),
                   Math.Max(rc.Left, rc.Right),
                   Math.Max(rc.Top, rc.Bottom));
            }
            else
            {
                return rc;
            }
        }

        public static Rectangle FixRectangle(Rectangle rc)
        {
            if (rc.Left > rc.Right || rc.Top > rc.Bottom)
            {
                return Rectangle.FromLTRB(
                   Math.Min(rc.Left, rc.Right),
                   Math.Min(rc.Top, rc.Bottom),
                   Math.Max(rc.Left, rc.Right),
                   Math.Max(rc.Top, rc.Bottom));
            }
            else
            {
                return rc;
            }
        }

        public static void SetDefaultComments(RasterImage rasterImage, RasterCodecs rasterCodecs)
        {
            rasterCodecs.Options.Save.Comments = true;

            RasterCommentMetadata commentSoftware = new RasterCommentMetadata();
            commentSoftware.Type = RasterCommentMetadataType.Software;
            if (!Is64Process())
            {
                commentSoftware.FromAscii("LEADTOOLS For .NET 32 bit");
            }
            else
            {
                commentSoftware.FromAscii("LEADTOOLS For .NET 64 bit");
            }

            RasterCommentMetadata commentCopyright = new RasterCommentMetadata();
            commentCopyright.Type = RasterCommentMetadataType.Copyright;
            commentCopyright.FromAscii("Copyright (c) 1991-2024 LEAD Technologies, Inc.  All Rights Reserved.");

            rasterImage.Comments.Add(commentSoftware);
            rasterImage.Comments.Add(commentCopyright);
        }

        public static String GetOcrOutputFileName(IWin32Window owner, string filter)
        {
            string outFileName = string.Empty;
            using (System.Windows.Forms.SaveFileDialog saveFileDlg = new System.Windows.Forms.SaveFileDialog())
            {
                saveFileDlg.Filter = filter;
                saveFileDlg.FilterIndex = 1;

                if (saveFileDlg.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                {
                    outFileName = saveFileDlg.FileName;
                }
            }

            return outFileName;
        }

        //public static String GetOcrOutputPath(IWin32Window owner)
        //{
        //   string outPath = string.Empty;
        //   using (FolderBrowserDialog browseDlg = new FolderBrowserDialog())
        //   {
        //      browseDlg.ShowNewFolderButton = false;
        //      browseDlg.Description = string.Format("{0}:", DemosGlobalization.SelectOutputFolder);
        //      if (browseDlg.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
        //      {
        //         outPath = browseDlg.SelectedPath;
        //      }
        //   }

        //   return outPath;
        //}

        //      public static void ShowWebPage(string url)
        //      {
        //#if NET
        //         Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true } );
        //#else
        //         System.Diagnostics.Process.Start(url);
        //#endif
        //      }

        //      public static void ShowHelpFile(IWin32Window owner, string helpPath)
        //      {
        //         try
        //         {
        //            ShowWebPage(helpPath);
        //         }
        //         catch (Exception)
        //         {
        //            MessageBox.Show(owner, "Error opening file:\r\n" + helpPath);
        //         }
        //      }

        //      public static void LaunchHowTo(string topicName)
        //      {
        //         string helpPath = Application.ExecutablePath;
        //         int index = helpPath.ToLower().IndexOf("bin");
        //         helpPath = helpPath.Remove(index);
        //         helpPath += @"Help";

        //         if (!Directory.Exists(helpPath))
        //         {
        //            string[] pathsToCheck =
        //           {
        //               @"..\..\..\..\Help",
        //               @"..\..\..\..\..\Help",
        //               @"..\..\..\..\..\..\Help",
        //               @"..\..\..\..\..\..\..\Help"
        //            };

        //            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //            foreach (string pathToCheck in pathsToCheck)
        //            {
        //               helpPath = Path.GetFullPath(Path.Combine(currentDirectory, pathToCheck));
        //               if (Directory.Exists(helpPath))
        //               {
        //                  break;
        //               }
        //            }
        //         }

        //         ProcessStartInfo startInfo = new ProcessStartInfo();
        //         startInfo.FileName = @"hh.exe";
        //         startInfo.Arguments = String.Format("\"mk:@MSITStore:{0}::/{1}\"", helpPath, topicName);

        //         using (Process process = new Process())
        //         {
        //            process.StartInfo = startInfo;
        //            process.Start();
        //         }
        //      }

        //      public static void LaunchHelp2(string topicName)
        //      {
        //         string helpPath = Path.Combine(InstallLocation, "Help");

        //         ProcessStartInfo startInfo = new ProcessStartInfo();
        //         startInfo.FileName = Path.Combine(helpPath, "LeadtoolsHelpUtilities.exe");

        //         startInfo.Arguments = String.Format("/viewalt:{0}", topicName);

        //         startInfo.Arguments = String.Format("/viewalt:{0} {1} ", topicName, LTVersion);
        //         using (Process process = new Process())
        //         {
        //            process.StartInfo = startInfo;
        //            process.Start();
        //         }
        //      }

        ///
        /// Convert any possible string-Value of a given enumeration
        /// type to its internal representation.
        ///
        public static object StringToEnum(Type t, string Value)
        {
            foreach (FieldInfo fi in t.GetFields())
            {
                if (fi.Name == Value)
                {
                    return fi.GetValue(null);    // We use null because
                }
            }
            // enumeration values
            // are static

            throw new Exception(string.Format("Can not convert {0} to {1}", Value, t.ToString()));
        }

        // Trims off the % of an IPv6 address
        // Sets to lower case
        // Ipv4 addresses are returned unchanged
        // If IpV6 address is of form: ::ffff:192.168.0.xxx, then it strips off the ::ffff:
        public static string CleanIp(string ipAddress)
        {
            string prefix = "::ffff:";
            string[] ipParts = ipAddress.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            if (ipParts.Length > 0)
            {
                ipAddress = ipParts[0];
            }

            if (ipAddress.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                ipAddress = ipAddress.Substring(prefix.Length);
            }

            ipAddress = ipAddress.ToLower();
            return ipAddress;
        }
#endif // #if !LT_MP

        private static string FindReferencePath(string dir, int levels = 6)
        {
            dir = dir.Replace('\\', Path.DirectorySeparatorChar);
            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            for (int i = 0; i <= levels; i++)
            {
                var path = Path.GetFullPath(Path.Combine(currentDirectory, dir));
                if (Directory.Exists(path))
                {
                    // Add Directory Separator Char on end if not exist
                    if (!path[path.Length - 1].Equals(Path.DirectorySeparatorChar))
                        path += Path.DirectorySeparatorChar;
                    return path;
                }
                dir = ".." + Path.DirectorySeparatorChar + dir;
            }
            return null;
        }

        //public static readonly string ImagesFolderRunner = BuildImagesFolder();

        //      private static string BuildImagesFolder()
        //      {
        //         string imagesPath;

        //         try
        //         {
        //            // Check relative to current directory
        //            imagesPath = FindReferencePath(@"..\..\Resources\Images");
        //            if (!string.IsNullOrEmpty(imagesPath))
        //               return imagesPath;
        //         }
        //         catch { }

        //#if !LT_MP
        //         // Try registry next
        //         imagesPath = string.Format(@"Software\LEAD Technologies, Inc.\{0}\Images", LTVersion);
        //         RegistryKey rk = Registry.LocalMachine.OpenSubKey(imagesPath);
        //         if (rk != null)
        //         {
        //            string value = rk.GetValue(null) as string;
        //            rk.Close();
        //            return value;
        //         }

        //         try
        //         {
        //            // Check if %PUBLIC%\Documents\LEADTOOLS Images
        //            imagesPath = Path.Combine(GetCommonDocumentsFolder(), @"LEADTOOLS Images");
        //            if (Directory.Exists(imagesPath))
        //            {
        //               return imagesPath;
        //            }
        //         }
        //         catch { }
        //#endif // #if !LT_MP

        //         // Finally, use the current EXE path
        //         return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //      }

        //      public static string ImagesFolder
        //      {
        //         get
        //         {
        //            return ImagesFolderRunner;
        //         }
        //      }

        //public static readonly string OCRLeadRuntimeFolderRunner = BuildOCRRuntimeFolder();

        //      private static string BuildOCRRuntimeFolder()
        //      {
        //#if FOR_NUGET
        //         return null;
        //#else
        //         try
        //         {
        //            // Check relative to current directory
        //            string ocrRuntimePath = FindReferencePath(@"..\..\Common\OcrLEADRuntime", 0);
        //            if (!string.IsNullOrEmpty(ocrRuntimePath))
        //               return ocrRuntimePath;

        //            ocrRuntimePath = FindReferencePath(@"..\..\Bin\Common\OcrLEADRuntime", 8);
        //            if (!string.IsNullOrEmpty(ocrRuntimePath))
        //               return ocrRuntimePath;

        //            ocrRuntimePath = FindReferencePath($@"..\..\Bin{LTVersion}\Common\OcrLEADRuntime", 8);
        //            if (!string.IsNullOrEmpty(ocrRuntimePath))
        //               return ocrRuntimePath;

        //            ocrRuntimePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"LEADTOOLS\OcrLEADRuntime");
        //            if (Directory.Exists(ocrRuntimePath))
        //            {
        //               return ocrRuntimePath;
        //            }
        //         }
        //         catch { }

        //#if !LT_MP
        //         // Try registry next
        //         using (RegistryKey rk = Registry.LocalMachine.OpenSubKey($@"Software\LEAD Technologies, Inc.\OCRPathLEAD{LTVersion}"))
        //         {
        //            if (rk != null)
        //            {
        //               string value = rk.GetValue(null) as string;
        //               rk.Close();
        //               return value;
        //            }
        //         }
        //#endif // #if !LT_MP

        //         return string.Empty;
        //#endif // #if FOR_NUGET
        //      }

        //public static string OCRLeadRuntimeFolder
        //{
        //   get
        //   {
        //      return OCRLeadRuntimeFolderRunner;
        //   }
        //}

        //      public static readonly string ASRLeadRuntimeFolderRunner = BuildASRRuntimeFolder();

        //      private static string BuildASRRuntimeFolder()
        //      {
        //#if FOR_NUGET
        //         return null;
        //#else
        //         try
        //         {
        //            // Check relative to current directory
        //            string asrRuntimePath = FindReferencePath(@"..\..\Bin\Common\ASRRuntime");
        //            if (!string.IsNullOrEmpty(asrRuntimePath))
        //               return asrRuntimePath;

        //            asrRuntimePath = FindReferencePath($@"..\..\Bin{LTVersion}\Common\ASRRuntime");
        //            if (!string.IsNullOrEmpty(asrRuntimePath))
        //               return asrRuntimePath;

        //            asrRuntimePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"LEADTOOLS\ASRRuntime");
        //            if (Directory.Exists(asrRuntimePath))
        //            {
        //               return asrRuntimePath;
        //            }
        //         }
        //         catch { }

        //         return string.Empty;
        //#endif // #if FOR_NUGET
        //      }

        //      public static string ASRLeadRuntimeFolder
        //      {
        //         get
        //         {
        //            return ASRLeadRuntimeFolderRunner;
        //         }
        //      }


        private static string GetLibraryPath()
        {
#if NET
         var currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
         string binFolderName = "";
         var kernelName = string.Empty;
         if(Platform.IsLinux)
         {
             binFolderName = Path.Combine("Lib", Platform.Is64Bit ? "x64" : "x86");
             kernelName = "libltkrn.so";
         }
         else if(Platform.IsMacOS)
         {
            binFolderName = "Xcode/Frameworks/macOS/";
            kernelName = "Leadtools.framework";
         }
         else
         {
            binFolderName = Path.Combine("CDLL", Platform.Is64Bit ? "x64" : "Win32");
            kernelName = Platform.Is64Bit ? "LtKrnx.dll" : "LtKrnu.dll";
         }

         const int levels = 10;
         //string bin = @"Bin";
         string[] bins = new string[] { "Bin", $"Bin{LTVersion}" };

         for (var i = 0; i < levels; i++)
         {
            currentDir = Path.Combine(currentDir, "..");

            foreach (string bin in bins)
            {
               var path = Path.GetFullPath(Path.Combine(currentDir, bin, binFolderName, kernelName));
               if (Platform.IsMacOS && Directory.Exists(path) || File.Exists(path))
               {
                  return Path.GetDirectoryName(path);
               }
            }
         }
         
         throw new DllNotFoundException(kernelName);
#else
            throw new NotImplementedException();
#endif
        }

        public static void InitRuntime(string path)
        {
#if NET && !FOR_NUGET
         if (string.IsNullOrEmpty(path))
            Leadtools.Platform.LibraryPath = Path.GetFullPath(GetLibraryPath());
         else
            Leadtools.Platform.LibraryPath = path;
#endif
        }

        public static void InitRuntime()
        {
#if NET && !FOR_NUGET
            Leadtools.Platform.LibraryPath = @"E:\Program Files (x86)\Oculus 10\Bin"; // Path.GetFullPath(GetLibraryPath());
#endif
        }

    }
}
