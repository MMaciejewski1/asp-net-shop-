using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace testowo.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Wprowadz imię")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Wprowadz nazwisko")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Wprowadz adres")]
        [StringLength(100)]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Wprowadz miasto")]
        [StringLength(100)]
        public string Town { get; set; }

        [Required(ErrorMessage = "Wprowadz kod pocztowy")]
        [StringLength(6)]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Musisz wprowadzić numer telefonu")]
        [StringLength(20)]
        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Błędny format numeru telefonu.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Wprowadź swój adres e-mail.")]
        [EmailAddress(ErrorMessage = "Błędny format adresu e-mail.")]
        public string Email { get; set; }

        public string Commentary { get; set; }

        public DateTime AddDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public decimal OrderValue { get; set; }

        public List<OrderItem> OrderItem { get; set; }
    }


    public enum OrderStatus
    {
        New,
        Realized
    }
}