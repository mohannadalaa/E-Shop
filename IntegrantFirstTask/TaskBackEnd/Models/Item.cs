using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskBackEnd.Models
{
    public class Item : EntityData
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImgURL { get; set; }
        public string Details { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }

    }
}