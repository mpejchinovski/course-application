using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CourseApp.Models
{
    public class Application
    {
        public int Id { get; set; }
        [Required]
        public int CourseDateId { get; set; }
        [JsonIgnore]
        public CourseDate CourseDate { get; set; }
        [Required(ErrorMessage = "You must provide a company name")]
        [Display(Name = "Company name")]
        public String CompanyName { get; set; }
        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Phone number")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "Invalid phone number")]
        public String PhoneNumber { get; set; }
        [Required(ErrorMessage = "You must provide an email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email address")]
        public String Email { get; set; }
    }
}
