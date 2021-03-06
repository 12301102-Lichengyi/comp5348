﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaRevCo.Business.Components.Interfaces;
using System.IO;
using Common;

namespace MediaRevCo.Business.Components
{
    public class ReviewSubscriptionProvider : IReviewSubscriptionProvider
    {
        /// NOT THREAD SAFE!!!
        /// Kept here as a static for simplicity
        /// Ideally this would be persisted
        private static Dictionary<String, List<String>> sSubscriptions = null;
        private const String cRegistryFile = "Registry.txt";


        // Initiate the sSubscriptions, read from file "Registry.txt"
        static ReviewSubscriptionProvider()
        {
            sSubscriptions = new Dictionary<string, List<string>>();
            if (File.Exists(cRegistryFile))
            {
                using (Stream stream = new FileStream(cRegistryFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    sSubscriptions = (Dictionary<String, List<String>>)formatter.Deserialize(stream);
                }
            }
        }


        /**
         * MAIN
         * implement the interfact function
         */
        public void SubscribeForReviews(string pAddress, string pUPC)
        {
            if(!sSubscriptions.ContainsKey(pUPC))
            {
                sSubscriptions.Add(pUPC, new List<string>());
            }
            if (!sSubscriptions[pUPC].Contains(pAddress))
            {
                sSubscriptions[pUPC].Add(pAddress);
                UpdatePersistedRegistry();
            }
        }


        /**
         * use the UPC as the key to find the address
         */
        public void PublishReview(Entities.Review pReview)
        {
            if(sSubscriptions.ContainsKey(pReview.UPC))
            {
                foreach(String lSubscriberAddress in sSubscriptions[pReview.UPC])
                {
                    // use the address to get the Subscriber
                    IReviewSubscriber lSubscriber = ServiceFactory.GetService<IReviewSubscriber>(lSubscriberAddress);
                    lSubscriber.ReceiveReview(pReview);
                }
            }           
        }


        // Write to the file
        private void UpdatePersistedRegistry()
        {
            using (Stream stream = new FileStream(cRegistryFile, FileMode.Create, FileAccess.Write, FileShare.Write))
            {

                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, sSubscriptions);
            }
        }
    }
}
