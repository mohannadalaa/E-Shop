﻿using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskBackEnd.DataObjects
{
    public class User : EntityData
    {
        public string Name { get; set; }
    }
}