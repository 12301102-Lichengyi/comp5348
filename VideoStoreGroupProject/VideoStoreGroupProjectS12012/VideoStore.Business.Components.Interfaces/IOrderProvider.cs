﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Entities;
using Bank.Business.Entities;

namespace VideoStore.Business.Components.Interfaces
{
    public interface IOrderProvider
    {
        void SubmitOrder(Order pOrder);
        void UpdateOrder(Order pOrder);
        Order GetOrderByExternalId(String pId);
    }
}
