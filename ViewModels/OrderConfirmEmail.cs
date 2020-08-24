using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testowo.Models;

namespace testowo.ViewModels
{
    public class OrderConfirmEmail : Postal.Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public decimal Value { get; set; }
        public int NrOrder { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}