using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace testowo.Models
{
    public class Category
    {

        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Wprowadz nazwę kategorii")]
        [StringLength(100)]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Wprowadz opis kategorii")]
        public string CategoryDesc { get; set; }
        public string NameFileIcon { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}