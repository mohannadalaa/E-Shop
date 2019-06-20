using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskBackEnd.DataObjects
{
    public class OrderDTO : EntityData
    {
        //public string UserName { get; set; }
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}