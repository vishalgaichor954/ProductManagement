//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProductManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductImageMapping
    {
        public long ID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public string ProductImage { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        public string ImagePath { get; set; }
        public Nullable<int> SequenceNo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual ProductMaster ProductMaster { get; set; }
    }
}