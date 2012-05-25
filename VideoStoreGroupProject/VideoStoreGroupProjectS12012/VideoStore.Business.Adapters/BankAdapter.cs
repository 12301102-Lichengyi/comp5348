using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageBus.Model;
using VideoStore.Common;
using VideoStore.Business.Entities;
using MessageBus.Interfaces;
using Microsoft.Practices.ServiceLocation;
using VideoStore.Business.Adapters.TransferService;
using VideoStore.Business.Components.Commands;
using VideoStore.Business.Components.Interfaces;
using Bank.Business.Components.Interfaces;
using Bank.Business.Entities;
//using System.Messaging;
using System.Transactions;
using System.ServiceModel;

namespace VideoStore.Business.Adapters
{
    public class BankAdapter : IAdapter, IOperationOutcomeService
    {
        /**
         * NEWLY ADDED 
         */
        private IEmailProvider EmailProvider
        {
            get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        }
        private IOrderProvider OrderProvider
        {
            get { return ServiceLocator.Current.GetInstance<IOrderProvider>(); }
        }
        
        /**
         * bank subscribe videostore
         */
        public void Start()
        {
            ISubscriptionService lSubscriber = ServiceLocator.Current.GetInstance<ISubscriptionService>();

            // the first argument is the same for all subscribers
            lSubscriber.Subscribe(typeof(Command).AssemblyQualifiedName, HandleMessage);

            HostBankReturnService();
        }


        /**
         * NEWLY ADDED 
         */
        private void HostBankReturnService()
        {
            EnsureResponeQueueExists();
            ServiceHost lHost = new ServiceHost(typeof(BankAdapter));
            NetMsmqBinding lBinding = new NetMsmqBinding();
            lBinding.Security.Mode = NetMsmqSecurityMode.None;

            lHost.AddServiceEndpoint(typeof(IOperationOutcomeService),
                lBinding,
                getWCFQueueName());

            lHost.Open();
        }




        public void HandleMessage(Message pMsg)
        {
            if (pMsg.GetType() == typeof(SubmitOrderCommand))
            {
                SubmitOrderCommand lCmd = pMsg as SubmitOrderCommand;
                Order lOrder = lCmd.Order;

                // how transfer is invoked, service reference
                TransferServiceClient lClient = new TransferServiceClient();
                lClient.Transfer((decimal)lOrder.Total, lOrder.Customer.BankAccountNumber, GetStoreAcctNumber(), lOrder.ExternalOrderId, getWCFQueueName());
            }
        }
        private int GetStoreAcctNumber()
        {
            return 123;
        }



        /**
         * NEWLY ADDED 
         */
        private String getQueueName()
        {
            return "BankResponseQueue";
        }
        private String getWCFQueueName()
        {
            return "net.msmq://localhost/private/" + getQueueName();
        }
        private String getMessagingQueueName()
        {
            return ".\\private$\\" + getQueueName();
        }
        private void EnsureResponeQueueExists()
        {
            String lQueuePath = getMessagingQueueName();
            // create the transacted MSMQ if necessary
            if (!System.Messaging.MessageQueue.Exists(lQueuePath))
            {
                System.Messaging.MessageQueue.Create(lQueuePath, true);
            }
        }

        public void NotifyOperationOutcome(Bank.Business.Entities.OperationOutcome pOutcome)
        {
            // transaction
            using (TransactionScope lScope = new TransactionScope())
            {
                Console.WriteLine("GUID - VideoStore:  " + pOutcome.OperationReference);
                // use the enternal id to get the order, HOW ?
                Order lOrder = ServiceLocator.Current.GetInstance<IOrderProvider>().GetOrderByExternalId(pOutcome.OperationReference);


                lOrder.StartTracking();
                if(pOutcome.Outcome == Bank.Business.Entities.OperationOutcome.OperationOutcomeResult.Successful)
                {
                    // anything that needs to happen if transfer is successful
                    EmailProvider.SendMessage(lOrder.Customer.Email, String.Format("Hi {0}, your order was successful", lOrder.Customer.Name));
                }
                if (pOutcome.Outcome == Bank.Business.Entities.OperationOutcome.OperationOutcomeResult.Failure)
                {
                    // anything that needs to happen if transfer fails

                    // rollback the stock level of each of the ordered item
                    lOrder.RollbackStockLevels();
                    // ?
                    OrderProvider.UpdateOrder(lOrder);

                    EmailProvider.SendMessage(lOrder.Customer.Email, String.Format("Hi {0}, your order failed because {1}", lOrder.Customer.Name, pOutcome.Message));
                }
                lScope.Complete();
            }
        }
    }
}
