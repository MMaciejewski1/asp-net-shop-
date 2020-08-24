using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace testowo.Models
{
    public class UserData
    {

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Adress { get; set; }

        public string Town { get; set; }

        public string ZipCode { get; set; }

        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Błędny format numeru telefonu.")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Błędny format adresu e-mail.")]
        public string Email { get; set; }


    }
}