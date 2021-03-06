﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 5.0.61118.0
// 
namespace SilverlightApplication1.ReactiveService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReactiveService.ILongRunningService")]
    public interface ILongRunningService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ILongRunningService/DoWork", ReplyAction="http://tempuri.org/ILongRunningService/DoWorkResponse")]
        System.IAsyncResult BeginDoWork(int seconds, System.AsyncCallback callback, object asyncState);
        
        int EndDoWork(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ILongRunningService/SortIt", ReplyAction="http://tempuri.org/ILongRunningService/SortItResponse")]
        System.IAsyncResult BeginSortIt(string value, System.AsyncCallback callback, object asyncState);
        
        string EndSortIt(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILongRunningServiceChannel : SilverlightApplication1.ReactiveService.ILongRunningService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DoWorkCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public DoWorkCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public int Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SortItCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public SortItCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LongRunningServiceClient : System.ServiceModel.ClientBase<SilverlightApplication1.ReactiveService.ILongRunningService>, SilverlightApplication1.ReactiveService.ILongRunningService {
        
        private BeginOperationDelegate onBeginDoWorkDelegate;
        
        private EndOperationDelegate onEndDoWorkDelegate;
        
        private System.Threading.SendOrPostCallback onDoWorkCompletedDelegate;
        
        private BeginOperationDelegate onBeginSortItDelegate;
        
        private EndOperationDelegate onEndSortItDelegate;
        
        private System.Threading.SendOrPostCallback onSortItCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public LongRunningServiceClient() {
        }
        
        public LongRunningServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LongRunningServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LongRunningServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LongRunningServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<DoWorkCompletedEventArgs> DoWorkCompleted;
        
        public event System.EventHandler<SortItCompletedEventArgs> SortItCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SilverlightApplication1.ReactiveService.ILongRunningService.BeginDoWork(int seconds, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginDoWork(seconds, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        int SilverlightApplication1.ReactiveService.ILongRunningService.EndDoWork(System.IAsyncResult result) {
            return base.Channel.EndDoWork(result);
        }
        
        private System.IAsyncResult OnBeginDoWork(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int seconds = ((int)(inValues[0]));
            return ((SilverlightApplication1.ReactiveService.ILongRunningService)(this)).BeginDoWork(seconds, callback, asyncState);
        }
        
        private object[] OnEndDoWork(System.IAsyncResult result) {
            int retVal = ((SilverlightApplication1.ReactiveService.ILongRunningService)(this)).EndDoWork(result);
            return new object[] {
                    retVal};
        }
        
        private void OnDoWorkCompleted(object state) {
            if ((this.DoWorkCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.DoWorkCompleted(this, new DoWorkCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void DoWorkAsync(int seconds) {
            this.DoWorkAsync(seconds, null);
        }
        
        public void DoWorkAsync(int seconds, object userState) {
            if ((this.onBeginDoWorkDelegate == null)) {
                this.onBeginDoWorkDelegate = new BeginOperationDelegate(this.OnBeginDoWork);
            }
            if ((this.onEndDoWorkDelegate == null)) {
                this.onEndDoWorkDelegate = new EndOperationDelegate(this.OnEndDoWork);
            }
            if ((this.onDoWorkCompletedDelegate == null)) {
                this.onDoWorkCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnDoWorkCompleted);
            }
            base.InvokeAsync(this.onBeginDoWorkDelegate, new object[] {
                        seconds}, this.onEndDoWorkDelegate, this.onDoWorkCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SilverlightApplication1.ReactiveService.ILongRunningService.BeginSortIt(string value, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSortIt(value, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string SilverlightApplication1.ReactiveService.ILongRunningService.EndSortIt(System.IAsyncResult result) {
            return base.Channel.EndSortIt(result);
        }
        
        private System.IAsyncResult OnBeginSortIt(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string value = ((string)(inValues[0]));
            return ((SilverlightApplication1.ReactiveService.ILongRunningService)(this)).BeginSortIt(value, callback, asyncState);
        }
        
        private object[] OnEndSortIt(System.IAsyncResult result) {
            string retVal = ((SilverlightApplication1.ReactiveService.ILongRunningService)(this)).EndSortIt(result);
            return new object[] {
                    retVal};
        }
        
        private void OnSortItCompleted(object state) {
            if ((this.SortItCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SortItCompleted(this, new SortItCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SortItAsync(string value) {
            this.SortItAsync(value, null);
        }
        
        public void SortItAsync(string value, object userState) {
            if ((this.onBeginSortItDelegate == null)) {
                this.onBeginSortItDelegate = new BeginOperationDelegate(this.OnBeginSortIt);
            }
            if ((this.onEndSortItDelegate == null)) {
                this.onEndSortItDelegate = new EndOperationDelegate(this.OnEndSortIt);
            }
            if ((this.onSortItCompletedDelegate == null)) {
                this.onSortItCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSortItCompleted);
            }
            base.InvokeAsync(this.onBeginSortItDelegate, new object[] {
                        value}, this.onEndSortItDelegate, this.onSortItCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override SilverlightApplication1.ReactiveService.ILongRunningService CreateChannel() {
            return new LongRunningServiceClientChannel(this);
        }
        
        private class LongRunningServiceClientChannel : ChannelBase<SilverlightApplication1.ReactiveService.ILongRunningService>, SilverlightApplication1.ReactiveService.ILongRunningService {
            
            public LongRunningServiceClientChannel(System.ServiceModel.ClientBase<SilverlightApplication1.ReactiveService.ILongRunningService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginDoWork(int seconds, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = seconds;
                System.IAsyncResult _result = base.BeginInvoke("DoWork", _args, callback, asyncState);
                return _result;
            }
            
            public int EndDoWork(System.IAsyncResult result) {
                object[] _args = new object[0];
                int _result = ((int)(base.EndInvoke("DoWork", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginSortIt(string value, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = value;
                System.IAsyncResult _result = base.BeginInvoke("SortIt", _args, callback, asyncState);
                return _result;
            }
            
            public string EndSortIt(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("SortIt", _args, result)));
                return _result;
            }
        }
    }
}
