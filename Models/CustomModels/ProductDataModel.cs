using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagement.Models.CustomModels
{
    public class ProductDataModel
    {
        public long ID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductKeyword { get; set; }
        public string ProductMetadata { get; set; }
        public Nullable<decimal> ProductPrice { get; set; }
        public string productImageFile { get; set; }
        public string productImageName { get; set; }
    //    
    }
}