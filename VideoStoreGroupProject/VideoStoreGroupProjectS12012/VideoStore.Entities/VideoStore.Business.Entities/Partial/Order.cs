using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoStore.Business.Entities
{
    public partial class Order
    {
        //public String ExternalId;


        public double Total
        {
            get
            {
                double lTotal = 0.0;
                foreach (OrderItem lItem in this.OrderItems)
                {
                    lTotal = lTotal + (double)lItem.Media.Price;
                }
                return lTotal;
            }
        }

        public void UpdateStockLevels()
        {
            foreach (OrderItem lItem in this.OrderItems)
            {
                if (lItem.Media.Stocks.Quantity - lItem.Quantity >= 0)
                {
                    lItem.Media.Stocks.Quantity -= lItem.Quantity;
                }
                else
                {
                    throw new Exception("Cannot place an order - no more stock for media item");
                }
            }
        }


        /**
         * RALL BACK THE STOCK LEVEL
         * author: Yu Zhao
         */
        public void RollbackStockLevels()
        {
            this.MarkAsModified();
            foreach (OrderItem lItem in this.OrderItems)
            {
                lItem.MarkAsModified();
                lItem.Media.MarkAsModified();
                lItem.Media.Stocks.MarkAsModified();

                lItem.Media.Stocks.Quantity += lItem.Quantity;
            }
        }
        public String ExternalOrderId
        {
            get 
            {
                if (ExternalId == null)
                {
                    ExternalId = Guid.NewGuid().ToString();
                }
                return ExternalId;
            }
        }
    }
}
