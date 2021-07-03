using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductManagement.Models.CustomModels
{
    public class ProductDataModel
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public long ID { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public string ProductKeyword { get; set; }
        [Required]
        public string ProductMetadata { get; set; }
        [Required]
        public Nullable<decimal> ProductPrice { get; set; }
        [Required]
        public string productImageFile { get; set; }
        [Required]
        public string productImageName { get; set; }
        [Required]
        public long CreatedBy { get; set; }

        public Nullable<long> ProductCategory { get; set; }
        public Nullable<long> ProductType { get; set; }


        public int IsImageEdited { get; set; }
        public string OldImageName { get; set; }

        //    
    }
}