using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testowo.Models;
namespace testowo.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category> Cat { get; set; }
        public IEnumerable<Course> News { get; set; }
        public IEnumerable<Course> Bestsellers { get; set; }

    }
}