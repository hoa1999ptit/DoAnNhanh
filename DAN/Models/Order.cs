//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAN.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public long Id { get; set; }
        public string Prefix { get; set; }
        public string OId { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal TotalPrice { get; set; }
        public string Code { get; set; }
        public System.DateTime CreatOn { get; set; }
        public bool Status { get; set; }
        public string PaidInfo { get; set; }
        public bool Paid { get; set; }
    }
}