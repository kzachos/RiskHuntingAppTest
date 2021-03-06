// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.17020
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace RiskHuntingAppTest.hofService {
    
    
    /// <remarks/>
    /// <remarks>
///The service combines and mixes a problem or idea with extreme persona selected from a static list of 100 personas or from the web. In particular, the service implements the Hall of Fame creativity technique which supports two types of creativity: (1) transformational creativity which encourages problem solvers to think about the space of possible ideas in a different (often larger) way; and  (2) exploratory creativity which enables problem solvers to explore new spaces more effectively with simple guidelines. 
///</remarks>
    [System.Web.Services.WebServiceBinding(Name="Web-Based_x0020_Hall_x0020_of_x0020_Fame_x0020_ServiceSoap", Namespace="http://10.200.51.10/")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class WebBasedHallofFameService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback RetrievePersonaSimpleOperationCompleted;
        
        private System.Threading.SendOrPostCallback RetrievePersonaSimpleBrokenOperationCompleted;
        
        private System.Threading.SendOrPostCallback RetrievePersonaFromTypeSimpleOperationCompleted;
        
        private System.Threading.SendOrPostCallback RetrieveSinglePersonaSimpleOperationCompleted;
        
        private System.Threading.SendOrPostCallback RetrievePersonaAdvancedOperationCompleted;
        
        private System.Threading.SendOrPostCallback RetrieveAllPersonasOperationCompleted;
        
        private System.Threading.SendOrPostCallback RetrieveAllPersonasFromTypeOperationCompleted;
        
        public WebBasedHallofFameService() {
            this.Url = "http://achernar.soi.city.ac.uk/HallOfFame/HallOfFameService/Service1.asmx";
        }
        
        public WebBasedHallofFameService(string url) {
            this.Url = url;
        }
        
        public event RetrievePersonaSimpleCompletedEventHandler RetrievePersonaSimpleCompleted;
        
        public event RetrievePersonaSimpleBrokenCompletedEventHandler RetrievePersonaSimpleBrokenCompleted;
        
        public event RetrievePersonaFromTypeSimpleCompletedEventHandler RetrievePersonaFromTypeSimpleCompleted;
        
        public event RetrieveSinglePersonaSimpleCompletedEventHandler RetrieveSinglePersonaSimpleCompleted;
        
        public event RetrievePersonaAdvancedCompletedEventHandler RetrievePersonaAdvancedCompleted;
        
        public event RetrieveAllPersonasCompletedEventHandler RetrieveAllPersonasCompleted;
        
        public event RetrieveAllPersonasFromTypeCompletedEventHandler RetrieveAllPersonasFromTypeCompleted;
        
        /// <remarks>
///<b>Input</b>:  No input required <br><b>Output</b>: returns a JSON containting one character with image(s) and natural language statements describing attributes of the character <br><b>Description</b>:   
///</remarks>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://10.200.51.10/RetrievePersonaSimple", RequestNamespace="http://10.200.51.10/", ResponseNamespace="http://10.200.51.10/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        public string RetrievePersonaSimple() {
            object[] results = this.Invoke("RetrievePersonaSimple", new object[0]);
            return ((string)(results[0]));
        }
        
        public System.IAsyncResult BeginRetrievePersonaSimple(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrievePersonaSimple", new object[0], callback, asyncState);
        }
        
        public string EndRetrievePersonaSimple(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        public void RetrievePersonaSimpleAsync() {
            this.RetrievePersonaSimpleAsync(null);
        }
        
        public void RetrievePersonaSimpleAsync(object userState) {
            if ((this.RetrievePersonaSimpleOperationCompleted == null)) {
                this.RetrievePersonaSimpleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRetrievePersonaSimpleCompleted);
            }
            this.InvokeAsync("RetrievePersonaSimple", new object[0], this.RetrievePersonaSimpleOperationCompleted, userState);
        }
        
        private void OnRetrievePersonaSimpleCompleted(object arg) {
            if ((this.RetrievePersonaSimpleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RetrievePersonaSimpleCompleted(this, new RetrievePersonaSimpleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks>
///<b>Input</b>:  No input required <br><b>Output</b>: returns a JSON containting one character with image(s) and natural language statements describing attributes of the character <br><b>Description</b>:   
///</remarks>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://10.200.51.10/RetrievePersonaSimpleBroken", RequestNamespace="http://10.200.51.10/", ResponseNamespace="http://10.200.51.10/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        public string RetrievePersonaSimpleBroken() {
            object[] results = this.Invoke("RetrievePersonaSimpleBroken", new object[0]);
            return ((string)(results[0]));
        }
        
        public System.IAsyncResult BeginRetrievePersonaSimpleBroken(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrievePersonaSimpleBroken", new object[0], callback, asyncState);
        }
        
        public string EndRetrievePersonaSimpleBroken(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        public void RetrievePersonaSimpleBrokenAsync() {
            this.RetrievePersonaSimpleBrokenAsync(null);
        }
        
        public void RetrievePersonaSimpleBrokenAsync(object userState) {
            if ((this.RetrievePersonaSimpleBrokenOperationCompleted == null)) {
                this.RetrievePersonaSimpleBrokenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRetrievePersonaSimpleBrokenCompleted);
            }
            this.InvokeAsync("RetrievePersonaSimpleBroken", new object[0], this.RetrievePersonaSimpleBrokenOperationCompleted, userState);
        }
        
        private void OnRetrievePersonaSimpleBrokenCompleted(object arg) {
            if ((this.RetrievePersonaSimpleBrokenCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RetrievePersonaSimpleBrokenCompleted(this, new RetrievePersonaSimpleBrokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks>
///<b>Input</b>:  Type of personas that should be returned, e.g. film or superheroes <br><b>Output</b>: returns a JSON containting one character with image(s) and natural language statements describing attributes of the character <br><b>Description</b>:   
///</remarks>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://10.200.51.10/RetrievePersonaFromTypeSimple", RequestNamespace="http://10.200.51.10/", ResponseNamespace="http://10.200.51.10/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        public string RetrievePersonaFromTypeSimple(string type) {
            object[] results = this.Invoke("RetrievePersonaFromTypeSimple", new object[] {
                        type});
            return ((string)(results[0]));
        }
        
        public System.IAsyncResult BeginRetrievePersonaFromTypeSimple(string type, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrievePersonaFromTypeSimple", new object[] {
                        type}, callback, asyncState);
        }
        
        public string EndRetrievePersonaFromTypeSimple(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        public void RetrievePersonaFromTypeSimpleAsync(string type) {
            this.RetrievePersonaFromTypeSimpleAsync(type, null);
        }
        
        public void RetrievePersonaFromTypeSimpleAsync(string type, object userState) {
            if ((this.RetrievePersonaFromTypeSimpleOperationCompleted == null)) {
                this.RetrievePersonaFromTypeSimpleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRetrievePersonaFromTypeSimpleCompleted);
            }
            this.InvokeAsync("RetrievePersonaFromTypeSimple", new object[] {
                        type}, this.RetrievePersonaFromTypeSimpleOperationCompleted, userState);
        }
        
        private void OnRetrievePersonaFromTypeSimpleCompleted(object arg) {
            if ((this.RetrievePersonaFromTypeSimpleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RetrievePersonaFromTypeSimpleCompleted(this, new RetrievePersonaFromTypeSimpleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks>
///<b>Input</b>:  Name of persona that should be returned, e.g. superman <br><b>Output</b>: returns a JSON containting one character with image(s) and natural language statements describing attributes of the character <br><b>Description</b>:   
///</remarks>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://10.200.51.10/RetrieveSinglePersonaSimple", RequestNamespace="http://10.200.51.10/", ResponseNamespace="http://10.200.51.10/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        public string RetrieveSinglePersonaSimple(string name) {
            object[] results = this.Invoke("RetrieveSinglePersonaSimple", new object[] {
                        name});
            return ((string)(results[0]));
        }
        
        public System.IAsyncResult BeginRetrieveSinglePersonaSimple(string name, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrieveSinglePersonaSimple", new object[] {
                        name}, callback, asyncState);
        }
        
        public string EndRetrieveSinglePersonaSimple(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        public void RetrieveSinglePersonaSimpleAsync(string name) {
            this.RetrieveSinglePersonaSimpleAsync(name, null);
        }
        
        public void RetrieveSinglePersonaSimpleAsync(string name, object userState) {
            if ((this.RetrieveSinglePersonaSimpleOperationCompleted == null)) {
                this.RetrieveSinglePersonaSimpleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRetrieveSinglePersonaSimpleCompleted);
            }
            this.InvokeAsync("RetrieveSinglePersonaSimple", new object[] {
                        name}, this.RetrieveSinglePersonaSimpleOperationCompleted, userState);
        }
        
        private void OnRetrieveSinglePersonaSimpleCompleted(object arg) {
            if ((this.RetrieveSinglePersonaSimpleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RetrieveSinglePersonaSimpleCompleted(this, new RetrieveSinglePersonaSimpleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks>
///<b>Input</b>:  Takes as input simple search criteria <br><b>Output</b>: returns a JSON containting one character with image(s) and natural language statements describing attributes of the character <br><b>Description</b>:   
///</remarks>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://10.200.51.10/RetrievePersonaAdvanced", RequestNamespace="http://10.200.51.10/", ResponseNamespace="http://10.200.51.10/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        public string RetrievePersonaAdvanced(string searchCriteria) {
            object[] results = this.Invoke("RetrievePersonaAdvanced", new object[] {
                        searchCriteria});
            return ((string)(results[0]));
        }
        
        public System.IAsyncResult BeginRetrievePersonaAdvanced(string searchCriteria, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrievePersonaAdvanced", new object[] {
                        searchCriteria}, callback, asyncState);
        }
        
        public string EndRetrievePersonaAdvanced(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        public void RetrievePersonaAdvancedAsync(string searchCriteria) {
            this.RetrievePersonaAdvancedAsync(searchCriteria, null);
        }
        
        public void RetrievePersonaAdvancedAsync(string searchCriteria, object userState) {
            if ((this.RetrievePersonaAdvancedOperationCompleted == null)) {
                this.RetrievePersonaAdvancedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRetrievePersonaAdvancedCompleted);
            }
            this.InvokeAsync("RetrievePersonaAdvanced", new object[] {
                        searchCriteria}, this.RetrievePersonaAdvancedOperationCompleted, userState);
        }
        
        private void OnRetrievePersonaAdvancedCompleted(object arg) {
            if ((this.RetrievePersonaAdvancedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RetrievePersonaAdvancedCompleted(this, new RetrievePersonaAdvancedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks>
///<b>Input</b>:  No input required <br><b>Output</b>: returns a JSON containting all current characters with image(s) and natural language statements describing attributes of the characters <br><b>Description</b>:   
///</remarks>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://10.200.51.10/RetrieveAllPersonas", RequestNamespace="http://10.200.51.10/", ResponseNamespace="http://10.200.51.10/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        public string RetrieveAllPersonas() {
            object[] results = this.Invoke("RetrieveAllPersonas", new object[0]);
            return ((string)(results[0]));
        }
        
        public System.IAsyncResult BeginRetrieveAllPersonas(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrieveAllPersonas", new object[0], callback, asyncState);
        }
        
        public string EndRetrieveAllPersonas(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        public void RetrieveAllPersonasAsync() {
            this.RetrieveAllPersonasAsync(null);
        }
        
        public void RetrieveAllPersonasAsync(object userState) {
            if ((this.RetrieveAllPersonasOperationCompleted == null)) {
                this.RetrieveAllPersonasOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRetrieveAllPersonasCompleted);
            }
            this.InvokeAsync("RetrieveAllPersonas", new object[0], this.RetrieveAllPersonasOperationCompleted, userState);
        }
        
        private void OnRetrieveAllPersonasCompleted(object arg) {
            if ((this.RetrieveAllPersonasCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RetrieveAllPersonasCompleted(this, new RetrieveAllPersonasCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks>
///<b>Input</b>:  No input required <br><b>Output</b>: returns a JSON containting all current characters with image(s) and natural language statements describing attributes of the characters <br><b>Description</b>:   
///</remarks>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://10.200.51.10/RetrieveAllPersonasFromType", RequestNamespace="http://10.200.51.10/", ResponseNamespace="http://10.200.51.10/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        public string RetrieveAllPersonasFromType(string type) {
            object[] results = this.Invoke("RetrieveAllPersonasFromType", new object[] {
                        type});
            return ((string)(results[0]));
        }
        
        public System.IAsyncResult BeginRetrieveAllPersonasFromType(string type, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrieveAllPersonasFromType", new object[] {
                        type}, callback, asyncState);
        }
        
        public string EndRetrieveAllPersonasFromType(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        public void RetrieveAllPersonasFromTypeAsync(string type) {
            this.RetrieveAllPersonasFromTypeAsync(type, null);
        }
        
        public void RetrieveAllPersonasFromTypeAsync(string type, object userState) {
            if ((this.RetrieveAllPersonasFromTypeOperationCompleted == null)) {
                this.RetrieveAllPersonasFromTypeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRetrieveAllPersonasFromTypeCompleted);
            }
            this.InvokeAsync("RetrieveAllPersonasFromType", new object[] {
                        type}, this.RetrieveAllPersonasFromTypeOperationCompleted, userState);
        }
        
        private void OnRetrieveAllPersonasFromTypeCompleted(object arg) {
            if ((this.RetrieveAllPersonasFromTypeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RetrieveAllPersonasFromTypeCompleted(this, new RetrieveAllPersonasFromTypeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
    }
    
    public partial class RetrievePersonaSimpleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RetrievePersonaSimpleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    public delegate void RetrievePersonaSimpleCompletedEventHandler(object sender, RetrievePersonaSimpleCompletedEventArgs args);
    
    public partial class RetrievePersonaSimpleBrokenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RetrievePersonaSimpleBrokenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    public delegate void RetrievePersonaSimpleBrokenCompletedEventHandler(object sender, RetrievePersonaSimpleBrokenCompletedEventArgs args);
    
    public partial class RetrievePersonaFromTypeSimpleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RetrievePersonaFromTypeSimpleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    public delegate void RetrievePersonaFromTypeSimpleCompletedEventHandler(object sender, RetrievePersonaFromTypeSimpleCompletedEventArgs args);
    
    public partial class RetrieveSinglePersonaSimpleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RetrieveSinglePersonaSimpleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    public delegate void RetrieveSinglePersonaSimpleCompletedEventHandler(object sender, RetrieveSinglePersonaSimpleCompletedEventArgs args);
    
    public partial class RetrievePersonaAdvancedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RetrievePersonaAdvancedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    public delegate void RetrievePersonaAdvancedCompletedEventHandler(object sender, RetrievePersonaAdvancedCompletedEventArgs args);
    
    public partial class RetrieveAllPersonasCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RetrieveAllPersonasCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    public delegate void RetrieveAllPersonasCompletedEventHandler(object sender, RetrieveAllPersonasCompletedEventArgs args);
    
    public partial class RetrieveAllPersonasFromTypeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RetrieveAllPersonasFromTypeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    public delegate void RetrieveAllPersonasFromTypeCompletedEventHandler(object sender, RetrieveAllPersonasFromTypeCompletedEventArgs args);
}
