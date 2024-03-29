﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrantFirstTask.Models
{
    public class Order
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public bool SubmittedOnline { get; set; }
        public bool SubmittedOffline { get; set; }
        public User User { get; set; }
        public List<OrderItems> OrderItems { get; set; }
    }
}
