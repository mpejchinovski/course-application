using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CourseApp.Models
{
    public class Course
    {
        public int Id { get; set; }
        public String Name { get; set; }

        [JsonIgnore]
        public ICollection<CourseDate> Dates { get; set; }
    }
}
