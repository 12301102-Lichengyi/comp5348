using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoStore.Business.Entities
{
    public partial class Stock
    {
        public Stock()
        {
            this.Id = Guid.NewGuid();
        }

        public void setMediaId(int Id)
        {
            this.MediaId = Id;
        }
    }
}
