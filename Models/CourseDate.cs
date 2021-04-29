using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CourseApp.Models
{
    public class CourseDate
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
