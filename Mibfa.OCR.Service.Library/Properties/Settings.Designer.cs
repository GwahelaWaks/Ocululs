﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mibfa.OCR.Service.Library.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.11.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://oculus.mibfa.co.za/OculusLogging/LogService.asmx")]
        public string Mibfa_OCR_Service_Library_wsLogService_LogService {
            get {
                return ((string)(this["Mibfa_OCR_Service_Library_wsLogService_LogService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://oculus.mibfa.co.za/Oculus/Services/Security.asmx")]
        public string Mibfa_OCR_Service_Library_wsSecurity_Security {
            get {
                return ((string)(this["Mibfa_OCR_Service_Library_wsSecurity_Security"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://oculus.mibfa.co.za/Oculus/Services/Queue.asmx")]
        public string Mibfa_OCR_Service_Library_wsQueue_Queue {
            get {
                return ((string)(this["Mibfa_OCR_Service_Library_wsQueue_Queue"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://oculus.mibfa.co.za/Oculus/Services/Launch.asmx")]
        public string Mibfa_OCR_Service_Library_wsLaunch_Launch {
            get {
                return ((string)(this["Mibfa_OCR_Service_Library_wsLaunch_Launch"]));
            }
        }
    }
}