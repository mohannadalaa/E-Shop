using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskBackEnd.Models
{
    public class Order : EntityData
    {
        [ForeignKey("User")]
        public string UserID { get; set; }
        public bool SubmittedOnline { get; set; }
        public bool SubmittedOffline { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}