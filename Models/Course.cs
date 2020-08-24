using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace testowo.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Wprowadz nazwę kursu")]
        [StringLength(100)]
        public string TitleCourse { get; set; }
        [Required(ErrorMessage = "Wprowadz nazwę autora")]
        [StringLength(100)]
        public string AutorCourse { get; set; }
        public DateTime DateAdd { get; set; }
        [StringLength(100)]
        public string NamePicture { get; set; }
        public string DescCourse { get; set; }
        public decimal PriceCourse { get; set; }
        public bool Bestseller { get; set; }
        public bool Hidden { get; set; }
        public string ShortDesc { get; set; }
        public string ShortDesc2 { get; set; }
        public virtual Category Category { get; set; }
    }
}