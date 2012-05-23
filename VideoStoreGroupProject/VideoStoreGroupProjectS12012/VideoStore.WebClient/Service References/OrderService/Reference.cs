﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VideoStore.WebClient.OrderService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ExtendedPropertiesDictionary", Namespace="http://schemas.datacontract.org/2004/07/VideoStore.Business.Entities", ItemName="ExtendedProperties", KeyName="Name", ValueName="ExtendedProperty")]
    [System.SerializableAttribute()]
    public class ExtendedPropertiesDictionary : System.Collections.Generic.Dictionary<string, object> {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ObjectsAddedToCollectionProperties", Namespace="http://schemas.datacontract.org/2004/07/VideoStore.Business.Entities", ItemName="AddedObjectsForProperty", KeyName="CollectionPropertyName", ValueName="AddedObjects")]
    [System.SerializableAttribute()]
    public class ObjectsAddedToCollectionProperties : System.Collections.Generic.Dictionary<string, VideoStore.WebClient.OrderService.ObjectList> {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ObjectList", Namespace="http://schemas.datacontract.org/2004/07/VideoStore.Business.Entities", ItemName="ObjectValue")]
    [System.SerializableAttribute()]
    public class ObjectList : System.Collections.Generic.List<object> {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ObjectsRemovedFromCollectionProperties", Namespace="http://schemas.datacontract.org/2004/07/VideoStore.Business.Entities", ItemName="DeletedObjectsForProperty", KeyName="CollectionPropertyName", ValueName="DeletedObjects")]
    [System.SerializableAttribute()]
    public class ObjectsRemovedFromCollectionProperties : System.Collections.Generic.Dictionary<string, VideoStore.WebClient.OrderService.ObjectList> {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="OriginalValuesDictionary", Namespace="http://schemas.datacontract.org/2004/07/VideoStore.Business.Entities", ItemName="OriginalValues", KeyName="Name", ValueName="OriginalValue")]
    [System.SerializableAttribute()]
    public class OriginalValuesDictionary : System.Collections.Generic.Dictionary<string, object> {
    }
    






    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="OrderService.IOrderService")]
    public interface IOrderService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/SubmitOrder", ReplyAction="http://tempuri.org/IOrderService/SubmitOrderResponse")]
        void SubmitOrder(VideoStore.Business.Entities.Order pOrder);
    }
    












    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IOrderServiceChannel : VideoStore.WebClient.OrderService.IOrderService, System.ServiceModel.IClientChannel {
    }
    















    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OrderServiceClient : System.ServiceModel.ClientBase<VideoStore.WebClient.OrderService.IOrderService>, VideoStore.WebClient.OrderService.IOrderService {
        
        public OrderServiceClient() {
        }
        
        public OrderServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OrderServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OrderServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OrderServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        


        public void SubmitOrder(VideoStore.Business.Entities.Order pOrder) {
            base.Channel.SubmitOrder(pOrder);
        }
    }
}
