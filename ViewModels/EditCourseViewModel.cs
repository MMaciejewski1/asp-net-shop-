using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testowo.Models;

namespace testowo.ViewModels
{
    public class EditCourseViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<Category> Cat { get; set; }
        public bool confirm { get; set; }
    }
}