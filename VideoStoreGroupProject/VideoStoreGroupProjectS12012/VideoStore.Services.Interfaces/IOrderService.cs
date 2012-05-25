using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Entities;
using System.ServiceModel;
using Bank.Business.Entities;

namespace VideoStore.Services.Interfaces
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        void SubmitOrder(Order pOrder);

        [OperationContract]
        void UpdateOrder(Order pOrder);

        [OperationContract]
        Order GetOrderByExternalId(String pId);
    }
}
