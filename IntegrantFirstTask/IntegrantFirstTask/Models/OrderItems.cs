using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrantFirstTask.Models
{
    public class OrderItems
    {
        public string Id { get; set; }
        public string ItemID { get; set; }
        public string OrderID { get; set; }
        public double ItemCount { get; set; }
    }
}
