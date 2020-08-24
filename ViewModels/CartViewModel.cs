using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testowo.Models;

namespace testowo.ViewModels
{
    public class CartViewModel
    {
        public List<CartPosition> OrderItem { get; set; }
        public decimal Totalprice{ get; set; }

}
}