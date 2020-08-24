using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testowo.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderID { get; set; }
        public int CourseId { get; set; }
        public int Amount { get; set; }
        public decimal PurchasePrice { get; set; }

        public virtual Course course { get; set; }
        public virtual Order order { get; set; }


    }
}