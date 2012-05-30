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

            

            // added transaction when submit an order
            try
            {
                using (TransactionScope lScope = new TransactionScope()) 
                {
                    ServiceLocator.Current.GetInstance<IPublisherService>().Publish(
                        CommandFactory.Instance.GetSubmitOrderCommand(pOrder)
                    );
                    lScope.Complete();
                }
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


        /**
         * NEWLY ADDED
         * ACCESS THE DATABASE TO SAVE THE ROLLBACK
         */ 
        public void UpdateOrder(Entities.Order pOrder)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                lContainer.ApplyChanges(typeof(Order).Name + "s", pOrder);
                lContainer.SaveChanges();
                lScope.Complete();
            }
        }

        /**
         * NEWLY ADDED 
         * IMPLEMENT GET ORDER BY EXTERNAL ID
         */
        public Order GetOrderByExternalId(String pId)
        {
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                Order lOrder = lContainer.Orders.Where((pOrder) => pOrder.ExternalId == pId).FirstOrDefault();
                User lUser = lContainer.Users.Where((pUser) => pUser.Id == lOrder.UserId).FirstOrDefault();
                lOrder.Customer = lUser;

                List<OrderItem> lOrderItems = lContainer.OrderItems.Where((pOrderItem) => pOrderItem.order_Id == lOrder.ExternalOrderId).ToList<OrderItem>();
                TrackableCollection<OrderItem> lTrackableOrderItems = new TrackableCollection<OrderItem>();

                foreach(OrderItem lItem in lOrderItems)
                {
                    Media lMedia = lContainer.Medias.Where((pMedia) => pMedia.Id == lItem.MediaId).FirstOrDefault();
                    Stock lStock = lContainer.Stocks.Where((pStock) => pStock.MediaId == lItem.MediaId).FirstOrDefault();
                    lMedia.Stocks = lStock;
                    lItem.Media = lMedia;

                    lTrackableOrderItems.Add(lItem);
                }

                lOrder.OrderItems = lTrackableOrderItems;

                return lOrder;
            }
        }
    }
}
