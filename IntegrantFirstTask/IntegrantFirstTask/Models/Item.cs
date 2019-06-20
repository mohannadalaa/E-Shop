using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrantFirstTask.Models
{
    public class Item
    {
        public string ID { get; set; }
        public string Name { get; set; }
        private string _details;
        public string Details
        {
            get { return _details; }

            set {
                _details = value;
                if (value.Length > 51)
                    SmallDetails = value.Substring(0, 50);
                else
                    SmallDetails = value;
                }
        }
        public string SmallDetails { get; set; }
        public double Price { get; set; }
        public string ImgURL { get; set; }
        public double Count { get; set; }
    }
}
