//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankServices.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransactionLog
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public string Success { get; set; }
        public int AccountId { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public System.DateTime ModDate { get; set; }
    }
}
