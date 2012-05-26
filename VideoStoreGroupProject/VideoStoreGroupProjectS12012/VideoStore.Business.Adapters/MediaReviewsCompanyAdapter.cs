using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageBus.Interfaces;
using Microsoft.Practices.ServiceLocation;
using MessageBus.Model;
using VideoStore.Common;
using VideoStore.Business.Entities;
using MediaRevCo.Business.Components.Interfaces;
using VideoStore.Business.Adapters.Transformations;
using MediaRevCo.Services.Interfaces;
using VideoStore.Business.Adapters.ReviewSubscriptionService;
using VideoStore.Business.Adapters;
using System.ServiceModel;


namespace VideoStore.Business.Adapters
{
    public class MediaReviewsCompanyAdapter : IAdapter, IReviewSubscriber
    {

        //private const String cSubscriberServiceAddress = "net.tcp://localhost:9010/SubscriberService";

        // this queue is for transfer review to the VideoStore
        private const String cSubscriberServiceAddress = "net.msmq://localhost/private/ReviewQueue";

        public void Start()
        {
            ISubscriptionService lSubscriber = ServiceLocator.Current.GetInstance<ISubscriptionService>();
            lSubscriber.Subscribe(typeof(Command).AssemblyQualifiedName, HandleMessage);
            HostReviewSubscriberService();
        }

        private void HostReviewSubscriberService()
        {
            // create review queue
            EnsureReviewQueueExists();


            ServiceHost lHost = new ServiceHost(typeof(MediaReviewsCompanyAdapter));

            /**
            lHost.AddServiceEndpoint(typeof(IReviewSubscriber), 
                new NetTcpBinding(),
                cSubscriberServiceAddress);
            */


            /**
             * NEWLY ADDED
             * host the service of review queue
             */
            
            NetMsmqBinding lBinding = new NetMsmqBinding();
            lBinding.Security.Mode = NetMsmqSecurityMode.None;
            lHost.AddServiceEndpoint(typeof(IReviewSubscriber),
                lBinding,
                getWCFQueueName());
            

            lHost.Open();
        }

        public void HandleMessage(Message pMsg)
        {
            if (pMsg.GetType() == typeof(EntityInsertCommand<Media>))
            {
                EntityInsertCommand<Media> lCmd = pMsg as EntityInsertCommand<Media>;
                ReviewSubscriptionServiceClient lClient = new ReviewSubscriptionServiceClient();
                lClient.SubscribeForReviews(cSubscriberServiceAddress, lCmd.Entity.UPC);
            }
        }


        /**
         * Implement the function in IReviewSubscriber
         */
        public void ReceiveReview(MediaRevCo.Business.Entities.Review pReview)
        {
            ReviewTransformVisitor lVis = new ReviewTransformVisitor();
            pReview.Accept(lVis);
            ServiceLocator.Current.GetInstance<IPublisherService>().Publish(
                CommandFactory.Instance.GetEntityInsertCommand<VideoStore.Business.Entities.Review>(lVis.Result)
            );
        }


        /**
         * NEWLY ADDED 
         */
        private String getQueueName()
        {
            return "ReviewQueue";
        }
        private String getWCFQueueName()
        {
            return "net.msmq://localhost/private/" + getQueueName();
        }
        private String getMessagingQueueName()
        {
            return ".\\private$\\" + getQueueName();
        }
        private void EnsureReviewQueueExists()
        {
            String lQueuePath = getMessagingQueueName();
            // create the transacted MSMQ if necessary
            if (!System.Messaging.MessageQueue.Exists(lQueuePath))
            {
                System.Messaging.MessageQueue.Create(lQueuePath, true);
            }
        }
    }
}
