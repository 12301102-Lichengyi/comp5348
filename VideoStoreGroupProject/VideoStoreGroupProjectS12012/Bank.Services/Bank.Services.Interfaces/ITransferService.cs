using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Bank.Services.Interfaces
{
    [ServiceContract]
    public interface ITransferService
    {
        //[OperationContract]
        [OperationContract(IsOneWay = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Transfer(decimal pAmount, int pFromAcctNumber, int pToAcctNumber, String pDescription, String pReturnAddress);
    }
}
