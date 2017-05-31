﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test.SSG.WebService {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PdfRequestStatus", Namespace="http://www.softsquaregroup.com/")]
    public enum PdfRequestStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        New = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InProcess = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Success = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Fail = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Name="PDF Creator Web ServiceSoap", Namespace="http://www.softsquaregroup.com/", ConfigurationName="SSG.WebService.PDFCreatorWebServiceSoap")]
    public interface PDFCreatorWebServiceSoap {
        
        // CODEGEN: Generating message contract since element name strContent from namespace http://www.softsquaregroup.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.softsquaregroup.com/GeneratePDFFromContent", ReplyAction="*")]
        Test.SSG.WebService.GeneratePDFFromContentResponse GeneratePDFFromContent(Test.SSG.WebService.GeneratePDFFromContentRequest request);
        
        // CODEGEN: Generating message contract since element name strID from namespace http://www.softsquaregroup.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.softsquaregroup.com/GeneratePDFFromFile", ReplyAction="*")]
        Test.SSG.WebService.GeneratePDFFromFileResponse GeneratePDFFromFile(Test.SSG.WebService.GeneratePDFFromFileRequest request);
        
        // CODEGEN: Generating message contract since element name strID from namespace http://www.softsquaregroup.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.softsquaregroup.com/GetStatus", ReplyAction="*")]
        Test.SSG.WebService.GetStatusResponse GetStatus(Test.SSG.WebService.GetStatusRequest request);
        
        // CODEGEN: Generating message contract since element name strID from namespace http://www.softsquaregroup.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.softsquaregroup.com/GetPdfUri", ReplyAction="*")]
        Test.SSG.WebService.GetPdfUriResponse GetPdfUri(Test.SSG.WebService.GetPdfUriRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GeneratePDFFromContentRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GeneratePDFFromContent", Namespace="http://www.softsquaregroup.com/", Order=0)]
        public Test.SSG.WebService.GeneratePDFFromContentRequestBody Body;
        
        public GeneratePDFFromContentRequest() {
        }
        
        public GeneratePDFFromContentRequest(Test.SSG.WebService.GeneratePDFFromContentRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.softsquaregroup.com/")]
    public partial class GeneratePDFFromContentRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string strContent;
        
        public GeneratePDFFromContentRequestBody() {
        }
        
        public GeneratePDFFromContentRequestBody(string strContent) {
            this.strContent = strContent;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GeneratePDFFromContentResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GeneratePDFFromContentResponse", Namespace="http://www.softsquaregroup.com/", Order=0)]
        public Test.SSG.WebService.GeneratePDFFromContentResponseBody Body;
        
        public GeneratePDFFromContentResponse() {
        }
        
        public GeneratePDFFromContentResponse(Test.SSG.WebService.GeneratePDFFromContentResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.softsquaregroup.com/")]
    public partial class GeneratePDFFromContentResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public System.Guid GeneratePDFFromContentResult;
        
        public GeneratePDFFromContentResponseBody() {
        }
        
        public GeneratePDFFromContentResponseBody(System.Guid GeneratePDFFromContentResult) {
            this.GeneratePDFFromContentResult = GeneratePDFFromContentResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GeneratePDFFromFileRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GeneratePDFFromFile", Namespace="http://www.softsquaregroup.com/", Order=0)]
        public Test.SSG.WebService.GeneratePDFFromFileRequestBody Body;
        
        public GeneratePDFFromFileRequest() {
        }
        
        public GeneratePDFFromFileRequest(Test.SSG.WebService.GeneratePDFFromFileRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.softsquaregroup.com/")]
    public partial class GeneratePDFFromFileRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string strID;
        
        public GeneratePDFFromFileRequestBody() {
        }
        
        public GeneratePDFFromFileRequestBody(string strID) {
            this.strID = strID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GeneratePDFFromFileResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GeneratePDFFromFileResponse", Namespace="http://www.softsquaregroup.com/", Order=0)]
        public Test.SSG.WebService.GeneratePDFFromFileResponseBody Body;
        
        public GeneratePDFFromFileResponse() {
        }
        
        public GeneratePDFFromFileResponse(Test.SSG.WebService.GeneratePDFFromFileResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.softsquaregroup.com/")]
    public partial class GeneratePDFFromFileResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public System.Guid GeneratePDFFromFileResult;
        
        public GeneratePDFFromFileResponseBody() {
        }
        
        public GeneratePDFFromFileResponseBody(System.Guid GeneratePDFFromFileResult) {
            this.GeneratePDFFromFileResult = GeneratePDFFromFileResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetStatusRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetStatus", Namespace="http://www.softsquaregroup.com/", Order=0)]
        public Test.SSG.WebService.GetStatusRequestBody Body;
        
        public GetStatusRequest() {
        }
        
        public GetStatusRequest(Test.SSG.WebService.GetStatusRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.softsquaregroup.com/")]
    public partial class GetStatusRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string strID;
        
        public GetStatusRequestBody() {
        }
        
        public GetStatusRequestBody(string strID) {
            this.strID = strID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetStatusResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetStatusResponse", Namespace="http://www.softsquaregroup.com/", Order=0)]
        public Test.SSG.WebService.GetStatusResponseBody Body;
        
        public GetStatusResponse() {
        }
        
        public GetStatusResponse(Test.SSG.WebService.GetStatusResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.softsquaregroup.com/")]
    public partial class GetStatusResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public Test.SSG.WebService.PdfRequestStatus GetStatusResult;
        
        public GetStatusResponseBody() {
        }
        
        public GetStatusResponseBody(Test.SSG.WebService.PdfRequestStatus GetStatusResult) {
            this.GetStatusResult = GetStatusResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetPdfUriRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetPdfUri", Namespace="http://www.softsquaregroup.com/", Order=0)]
        public Test.SSG.WebService.GetPdfUriRequestBody Body;
        
        public GetPdfUriRequest() {
        }
        
        public GetPdfUriRequest(Test.SSG.WebService.GetPdfUriRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.softsquaregroup.com/")]
    public partial class GetPdfUriRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string strID;
        
        public GetPdfUriRequestBody() {
        }
        
        public GetPdfUriRequestBody(string strID) {
            this.strID = strID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetPdfUriResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetPdfUriResponse", Namespace="http://www.softsquaregroup.com/", Order=0)]
        public Test.SSG.WebService.GetPdfUriResponseBody Body;
        
        public GetPdfUriResponse() {
        }
        
        public GetPdfUriResponse(Test.SSG.WebService.GetPdfUriResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.softsquaregroup.com/")]
    public partial class GetPdfUriResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetPdfUriResult;
        
        public GetPdfUriResponseBody() {
        }
        
        public GetPdfUriResponseBody(string GetPdfUriResult) {
            this.GetPdfUriResult = GetPdfUriResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface PDFCreatorWebServiceSoapChannel : Test.SSG.WebService.PDFCreatorWebServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class PDFCreatorWebServiceSoapClient : System.ServiceModel.ClientBase<Test.SSG.WebService.PDFCreatorWebServiceSoap>, Test.SSG.WebService.PDFCreatorWebServiceSoap {
        
        public PDFCreatorWebServiceSoapClient() {
        }
        
        public PDFCreatorWebServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PDFCreatorWebServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PDFCreatorWebServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PDFCreatorWebServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Test.SSG.WebService.GeneratePDFFromContentResponse Test.SSG.WebService.PDFCreatorWebServiceSoap.GeneratePDFFromContent(Test.SSG.WebService.GeneratePDFFromContentRequest request) {
            return base.Channel.GeneratePDFFromContent(request);
        }
        
        public System.Guid GeneratePDFFromContent(string strContent) {
            Test.SSG.WebService.GeneratePDFFromContentRequest inValue = new Test.SSG.WebService.GeneratePDFFromContentRequest();
            inValue.Body = new Test.SSG.WebService.GeneratePDFFromContentRequestBody();
            inValue.Body.strContent = strContent;
            Test.SSG.WebService.GeneratePDFFromContentResponse retVal = ((Test.SSG.WebService.PDFCreatorWebServiceSoap)(this)).GeneratePDFFromContent(inValue);
            return retVal.Body.GeneratePDFFromContentResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Test.SSG.WebService.GeneratePDFFromFileResponse Test.SSG.WebService.PDFCreatorWebServiceSoap.GeneratePDFFromFile(Test.SSG.WebService.GeneratePDFFromFileRequest request) {
            return base.Channel.GeneratePDFFromFile(request);
        }
        
        public System.Guid GeneratePDFFromFile(string strID) {
            Test.SSG.WebService.GeneratePDFFromFileRequest inValue = new Test.SSG.WebService.GeneratePDFFromFileRequest();
            inValue.Body = new Test.SSG.WebService.GeneratePDFFromFileRequestBody();
            inValue.Body.strID = strID;
            Test.SSG.WebService.GeneratePDFFromFileResponse retVal = ((Test.SSG.WebService.PDFCreatorWebServiceSoap)(this)).GeneratePDFFromFile(inValue);
            return retVal.Body.GeneratePDFFromFileResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Test.SSG.WebService.GetStatusResponse Test.SSG.WebService.PDFCreatorWebServiceSoap.GetStatus(Test.SSG.WebService.GetStatusRequest request) {
            return base.Channel.GetStatus(request);
        }
        
        public Test.SSG.WebService.PdfRequestStatus GetStatus(string strID) {
            Test.SSG.WebService.GetStatusRequest inValue = new Test.SSG.WebService.GetStatusRequest();
            inValue.Body = new Test.SSG.WebService.GetStatusRequestBody();
            inValue.Body.strID = strID;
            Test.SSG.WebService.GetStatusResponse retVal = ((Test.SSG.WebService.PDFCreatorWebServiceSoap)(this)).GetStatus(inValue);
            return retVal.Body.GetStatusResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Test.SSG.WebService.GetPdfUriResponse Test.SSG.WebService.PDFCreatorWebServiceSoap.GetPdfUri(Test.SSG.WebService.GetPdfUriRequest request) {
            return base.Channel.GetPdfUri(request);
        }
        
        public string GetPdfUri(string strID) {
            Test.SSG.WebService.GetPdfUriRequest inValue = new Test.SSG.WebService.GetPdfUriRequest();
            inValue.Body = new Test.SSG.WebService.GetPdfUriRequestBody();
            inValue.Body.strID = strID;
            Test.SSG.WebService.GetPdfUriResponse retVal = ((Test.SSG.WebService.PDFCreatorWebServiceSoap)(this)).GetPdfUri(inValue);
            return retVal.Body.GetPdfUriResult;
        }
    }
}
