﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VideoStore.Business.Adapters.ReviewSubscriptionService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReviewSubscriptionService.IReviewSubscriptionService")]
    public interface IReviewSubscriptionService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReviewSubscriptionService/SubscribeForReviews", ReplyAction="http://tempuri.org/IReviewSubscriptionService/SubscribeForReviewsResponse")]
        void SubscribeForReviews(string pAddress, string pUPC);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IReviewSubscriptionServiceChannel : VideoStore.Business.Adapters.ReviewSubscriptionService.IReviewSubscriptionService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ReviewSubscriptionServiceClient : System.ServiceModel.ClientBase<VideoStore.Business.Adapters.ReviewSubscriptionService.IReviewSubscriptionService>, VideoStore.Business.Adapters.ReviewSubscriptionService.IReviewSubscriptionService {
        
        public ReviewSubscriptionServiceClient() {
        }
        
        public ReviewSubscriptionServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ReviewSubscriptionServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReviewSubscriptionServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReviewSubscriptionServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void SubscribeForReviews(string pAddress, string pUPC) {
            base.Channel.SubscribeForReviews(pAddress, pUPC);
        }
    }
}
