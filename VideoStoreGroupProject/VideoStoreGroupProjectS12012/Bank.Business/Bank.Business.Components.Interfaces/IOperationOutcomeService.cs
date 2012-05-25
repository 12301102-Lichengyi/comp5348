using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bank.Business.Entities;
using System.ServiceModel;

/**
 * videostore can use this interface
 */
namespace Bank.Business.Components.Interfaces
{
    [ServiceContract]
    public interface IOperationOutcomeService
    {
        // when using queues, use this
        [OperationContract(IsOneWay = true)]
        void NotifyOperationOutcome(OperationOutcome pOutcome); 
    }
}
