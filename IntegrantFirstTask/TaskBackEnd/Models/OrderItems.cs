using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskBackEnd.Models
{
    public class OrderItems : EntityData
    {
        //[Key, Column(Order = 0)]
        public string ItemID { get; set; }
        //[Key, Column(Order = 1)]
        public string OrderID { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
        public int ItemCount { get; set; }
    }
}