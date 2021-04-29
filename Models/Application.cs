using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "You must provide a company name")]
        public String CompanyName { get; set; }
        [Required(ErrorMessage = "You must provide a phone number")]
        public String PhoneNumber { get; set; }
        [Required(ErrorMessage = "You must provide an email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public String Email { get; set; }
    }
}
