using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;
using System.Transactions;
using VideoStore.Common;
using Microsoft.Practices.ServiceLocation;
using MessageBus.Interfaces;
using Bank.Business.Entities;

namespace VideoStore.Business.Components
{
    public class OrderProvider : IOrderProvider
    {
        private IEmailProvider EmailProvider
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IEmailProvider>();
            }
        }


        /**
         * NEWLY ADDED
         * a dictionary storing all the submitted orders (not successed)
         */
        /**
        private static Dictionary<String, Order> sOrderPool =
            new Dictionary<string, Order>();
        private void addOrder(Entities.Order pOrder)
        {
            if (!sOrderPool.ContainsKey(pOrder.ExternalOrderId))
            {
                sOrderPool.Add(pOrder.ExternalOrderId, pOrder);
            }
        }
        private void removeOrder(Entities.Order pOrder)
        {
            if (sOrderPool.ContainsKey(pOrder.ExternalOrderId))
            {
                sOrderPool.Remove(pOrder.ExternalOrderId);
            }
        }
        private Order RetireveOrder(String pKey)
        {
            if (sOrderPool.ContainsKey(pKey))
            {
                return sOrderPool[pKey];
            }
            return null;
        }
        */


        public void SubmitOrder(Entities.Order pOrder)
        {
            String lMessage = String.Format("Your order has been submitted successfully");

            // When the user submits an order, add this order to the order pool
            //addOrder(pOrder);



            try
            {
                ServiceLocator.Current.GetInstance<IPublisherService>().Publish(
                    CommandFactory.Instance.GetSubmitOrderCommand(pOrder)
                );
            }
            catch (Exception lException)
            {
                lMessage = lException.Message;
                // THROW should be removed
                //throw;
            }
            finally
            {
                EmailProvider.SendMessage(pOrder.Customer.Email, lMessage);
            }
        }


        // ?
        public void UpdateOrder(Entities.Order pOrder)
        {
            // remove the order from the pool when the order fails
            //removeOrder(pOrder);
        }

        public Order GetOrderByExternalId(String pId)
        {
            //return RetireveOrder(pId);

            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                Order lOrder = lContainer.Orders.Where((pOrder) => pOrder.ExternalId == pId).FirstOrDefault();
                return lOrder;
            }
        }
    }
}
