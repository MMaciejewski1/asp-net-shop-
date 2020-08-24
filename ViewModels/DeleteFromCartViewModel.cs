using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testowo.ViewModels
{
    public class DeleteFromCartViewModel
    {

        public decimal TotalCartPrice { get; set; }
        public int CartCountPosition { get; set; }
        public int CountPositionToDelete { get; set; }
        public int IdPositionToDelete { get; set; }
    }
}