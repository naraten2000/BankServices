using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankServices.Models
{
    public class Transaction
    {
        public int AccountId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}