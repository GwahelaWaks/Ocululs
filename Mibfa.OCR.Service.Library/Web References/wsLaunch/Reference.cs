﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Mibfa.OCR.Service.Library.wsLaunch {
    using System.Diagnostics;
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="LaunchSoap", Namespace="https://www.gijima.com/Oculus")]
    public partial class Launch : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback LaunchDocumentOperationCompleted;
        
        private System.Threading.SendOrPostCallback LaunchDocumentWithProcessOperationCompleted;
        
        private System.Threading.SendOrPostCallback LaunchDocumentWithProcessIdOperationCompleted;
        
        private System.Threading.SendOrPostCallback ProcessFragmentOperationCompleted;
        
        private System.Threading.SendOrPostCallback CompleteLaunchOperationCompleted;
        
        private System.Threading.SendOrPostCallback CancelLaunchOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadDocumentsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Launch() {
            this.Url = global::Mibfa.OCR.Service.Library.Properties.Settings.Default.Mibfa_OCR_Service_Library_wsLaunch_Launch;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event LaunchDocumentCompletedEventHandler LaunchDocumentCompleted;
        
        /// <remarks/>
        public event LaunchDocumentWithProcessCompletedEventHandler LaunchDocumentWithProcessCompleted;
        
        /// <remarks/>
        public event LaunchDocumentWithProcessIdCompletedEventHandler LaunchDocumentWithProcessIdCompleted;
        
        /// <remarks/>
        public event ProcessFragmentCompletedEventHandler ProcessFragmentCompleted;
        
        /// <remarks/>
        public event CompleteLaunchCompletedEventHandler CompleteLaunchCompleted;
        
        /// <remarks/>
        public event CancelLaunchCompletedEventHandler CancelLaunchCompleted;
        
        /// <remarks/>
        public event UploadDocumentsCompletedEventHandler UploadDocumentsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.gijima.com/Oculus/LaunchDocument", RequestNamespace="https://www.gijima.com/Oculus", ResponseNamespace="https://www.gijima.com/Oculus", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Guid LaunchDocument(System.Guid sessionId, string documentXml) {
            object[] results = this.Invoke("LaunchDocument", new object[] {
                        sessionId,
                        documentXml});
            return ((System.Guid)(results[0]));
        }
        
        /// <remarks/>
        public void LaunchDocumentAsync(System.Guid sessionId, string documentXml) {
            this.LaunchDocumentAsync(sessionId, documentXml, null);
        }
        
        /// <remarks/>
        public void LaunchDocumentAsync(System.Guid sessionId, string documentXml, object userState) {
            if ((this.LaunchDocumentOperationCompleted == null)) {
                this.LaunchDocumentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLaunchDocumentOperationCompleted);
            }
            this.InvokeAsync("LaunchDocument", new object[] {
                        sessionId,
                        documentXml}, this.LaunchDocumentOperationCompleted, userState);
        }
        
        private void OnLaunchDocumentOperationCompleted(object arg) {
            if ((this.LaunchDocumentCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LaunchDocumentCompleted(this, new LaunchDocumentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.gijima.com/Oculus/LaunchDocumentWithProcess", RequestNamespace="https://www.gijima.com/Oculus", ResponseNamespace="https://www.gijima.com/Oculus", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Guid LaunchDocumentWithProcess(System.Guid sessionId, string documentXml, string processXml) {
            object[] results = this.Invoke("LaunchDocumentWithProcess", new object[] {
                        sessionId,
                        documentXml,
                        processXml});
            return ((System.Guid)(results[0]));
        }
        
        /// <remarks/>
        public void LaunchDocumentWithProcessAsync(System.Guid sessionId, string documentXml, string processXml) {
            this.LaunchDocumentWithProcessAsync(sessionId, documentXml, processXml, null);
        }
        
        /// <remarks/>
        public void LaunchDocumentWithProcessAsync(System.Guid sessionId, string documentXml, string processXml, object userState) {
            if ((this.LaunchDocumentWithProcessOperationCompleted == null)) {
                this.LaunchDocumentWithProcessOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLaunchDocumentWithProcessOperationCompleted);
            }
            this.InvokeAsync("LaunchDocumentWithProcess", new object[] {
                        sessionId,
                        documentXml,
                        processXml}, this.LaunchDocumentWithProcessOperationCompleted, userState);
        }
        
        private void OnLaunchDocumentWithProcessOperationCompleted(object arg) {
            if ((this.LaunchDocumentWithProcessCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LaunchDocumentWithProcessCompleted(this, new LaunchDocumentWithProcessCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.gijima.com/Oculus/LaunchDocumentWithProcessId", RequestNamespace="https://www.gijima.com/Oculus", ResponseNamespace="https://www.gijima.com/Oculus", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Guid LaunchDocumentWithProcessId(System.Guid sessionId, string documentXml, System.Guid processId) {
            object[] results = this.Invoke("LaunchDocumentWithProcessId", new object[] {
                        sessionId,
                        documentXml,
                        processId});
            return ((System.Guid)(results[0]));
        }
        
        /// <remarks/>
        public void LaunchDocumentWithProcessIdAsync(System.Guid sessionId, string documentXml, System.Guid processId) {
            this.LaunchDocumentWithProcessIdAsync(sessionId, documentXml, processId, null);
        }
        
        /// <remarks/>
        public void LaunchDocumentWithProcessIdAsync(System.Guid sessionId, string documentXml, System.Guid processId, object userState) {
            if ((this.LaunchDocumentWithProcessIdOperationCompleted == null)) {
                this.LaunchDocumentWithProcessIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLaunchDocumentWithProcessIdOperationCompleted);
            }
            this.InvokeAsync("LaunchDocumentWithProcessId", new object[] {
                        sessionId,
                        documentXml,
                        processId}, this.LaunchDocumentWithProcessIdOperationCompleted, userState);
        }
        
        private void OnLaunchDocumentWithProcessIdOperationCompleted(object arg) {
            if ((this.LaunchDocumentWithProcessIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LaunchDocumentWithProcessIdCompleted(this, new LaunchDocumentWithProcessIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.gijima.com/Oculus/ProcessFragment", RequestNamespace="https://www.gijima.com/Oculus", ResponseNamespace="https://www.gijima.com/Oculus", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ProcessFragment(System.Guid sessionId, System.Guid documentUniqueId, string fragmentXml, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] fragmentData) {
            this.Invoke("ProcessFragment", new object[] {
                        sessionId,
                        documentUniqueId,
                        fragmentXml,
                        fragmentData});
        }
        
        /// <remarks/>
        public void ProcessFragmentAsync(System.Guid sessionId, System.Guid documentUniqueId, string fragmentXml, byte[] fragmentData) {
            this.ProcessFragmentAsync(sessionId, documentUniqueId, fragmentXml, fragmentData, null);
        }
        
        /// <remarks/>
        public void ProcessFragmentAsync(System.Guid sessionId, System.Guid documentUniqueId, string fragmentXml, byte[] fragmentData, object userState) {
            if ((this.ProcessFragmentOperationCompleted == null)) {
                this.ProcessFragmentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnProcessFragmentOperationCompleted);
            }
            this.InvokeAsync("ProcessFragment", new object[] {
                        sessionId,
                        documentUniqueId,
                        fragmentXml,
                        fragmentData}, this.ProcessFragmentOperationCompleted, userState);
        }
        
        private void OnProcessFragmentOperationCompleted(object arg) {
            if ((this.ProcessFragmentCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ProcessFragmentCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.gijima.com/Oculus/CompleteLaunch", RequestNamespace="https://www.gijima.com/Oculus", ResponseNamespace="https://www.gijima.com/Oculus", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void CompleteLaunch(System.Guid sessionId, System.Guid documentUniqueId) {
            this.Invoke("CompleteLaunch", new object[] {
                        sessionId,
                        documentUniqueId});
        }
        
        /// <remarks/>
        public void CompleteLaunchAsync(System.Guid sessionId, System.Guid documentUniqueId) {
            this.CompleteLaunchAsync(sessionId, documentUniqueId, null);
        }
        
        /// <remarks/>
        public void CompleteLaunchAsync(System.Guid sessionId, System.Guid documentUniqueId, object userState) {
            if ((this.CompleteLaunchOperationCompleted == null)) {
                this.CompleteLaunchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCompleteLaunchOperationCompleted);
            }
            this.InvokeAsync("CompleteLaunch", new object[] {
                        sessionId,
                        documentUniqueId}, this.CompleteLaunchOperationCompleted, userState);
        }
        
        private void OnCompleteLaunchOperationCompleted(object arg) {
            if ((this.CompleteLaunchCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CompleteLaunchCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.gijima.com/Oculus/CancelLaunch", RequestNamespace="https://www.gijima.com/Oculus", ResponseNamespace="https://www.gijima.com/Oculus", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void CancelLaunch(System.Guid sessionId, System.Guid documentUniqueId) {
            this.Invoke("CancelLaunch", new object[] {
                        sessionId,
                        documentUniqueId});
        }
        
        /// <remarks/>
        public void CancelLaunchAsync(System.Guid sessionId, System.Guid documentUniqueId) {
            this.CancelLaunchAsync(sessionId, documentUniqueId, null);
        }
        
        /// <remarks/>
        public void CancelLaunchAsync(System.Guid sessionId, System.Guid documentUniqueId, object userState) {
            if ((this.CancelLaunchOperationCompleted == null)) {
                this.CancelLaunchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCancelLaunchOperationCompleted);
            }
            this.InvokeAsync("CancelLaunch", new object[] {
                        sessionId,
                        documentUniqueId}, this.CancelLaunchOperationCompleted, userState);
        }
        
        private void OnCancelLaunchOperationCompleted(object arg) {
            if ((this.CancelLaunchCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CancelLaunchCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.gijima.com/Oculus/UploadDocuments", RequestNamespace="https://www.gijima.com/Oculus", ResponseNamespace="https://www.gijima.com/Oculus", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadDocuments(string sessionId, string profileID, string profileContextID, string processID, string[] documentFiles, string[] documentFileNames, string documentFileNameIndexes) {
            object[] results = this.Invoke("UploadDocuments", new object[] {
                        sessionId,
                        profileID,
                        profileContextID,
                        processID,
                        documentFiles,
                        documentFileNames,
                        documentFileNameIndexes});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadDocumentsAsync(string sessionId, string profileID, string profileContextID, string processID, string[] documentFiles, string[] documentFileNames, string documentFileNameIndexes) {
            this.UploadDocumentsAsync(sessionId, profileID, profileContextID, processID, documentFiles, documentFileNames, documentFileNameIndexes, null);
        }
        
        /// <remarks/>
        public void UploadDocumentsAsync(string sessionId, string profileID, string profileContextID, string processID, string[] documentFiles, string[] documentFileNames, string documentFileNameIndexes, object userState) {
            if ((this.UploadDocumentsOperationCompleted == null)) {
                this.UploadDocumentsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadDocumentsOperationCompleted);
            }
            this.InvokeAsync("UploadDocuments", new object[] {
                        sessionId,
                        profileID,
                        profileContextID,
                        processID,
                        documentFiles,
                        documentFileNames,
                        documentFileNameIndexes}, this.UploadDocumentsOperationCompleted, userState);
        }
        
        private void OnUploadDocumentsOperationCompleted(object arg) {
            if ((this.UploadDocumentsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadDocumentsCompleted(this, new UploadDocumentsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void LaunchDocumentCompletedEventHandler(object sender, LaunchDocumentCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LaunchDocumentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LaunchDocumentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Guid Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Guid)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void LaunchDocumentWithProcessCompletedEventHandler(object sender, LaunchDocumentWithProcessCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LaunchDocumentWithProcessCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LaunchDocumentWithProcessCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Guid Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Guid)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void LaunchDocumentWithProcessIdCompletedEventHandler(object sender, LaunchDocumentWithProcessIdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LaunchDocumentWithProcessIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LaunchDocumentWithProcessIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Guid Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Guid)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void ProcessFragmentCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void CompleteLaunchCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void CancelLaunchCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void UploadDocumentsCompletedEventHandler(object sender, UploadDocumentsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadDocumentsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadDocumentsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591