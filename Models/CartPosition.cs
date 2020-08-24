using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testowo.Models
{
    public class CartPosition
    {
        public Course course { get; set; }
        public int count { get; set; }
        public double value { get; set; }
    }
}