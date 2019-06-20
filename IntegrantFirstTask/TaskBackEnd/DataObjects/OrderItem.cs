using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskBackEnd.DataObjects
{
    public class OrderItem:EntityData
    {
        public string ItemID { get; set; }
        public double ItemCount { get; set; }
    }
}