using CourseApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.Models
{
    public class JsonCourse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DateTime> Dates { get; set; }
    }

    public class SeedData
    {     
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CourseAppDbContext(serviceProvider.GetRequiredService<DbContextOptions<CourseAppDbContext>>()))
            {
                if (context.Course.Any() || context.Application.Any() || context.CourseDate.Any())
                {
                    return;
                }

                using (StreamReader r = new StreamReader(@"Data/courses.json"))
                {
                    string json = r.ReadToEnd();
                    foreach (var course in JsonConvert.DeserializeObject<List<JsonCourse>>(json))
                    {
                        context.Course.Add(new Course() { Id = course.Id, Name = course.Name });
                        foreach (var date in course.Dates)
                            context.CourseDate.Add(new CourseDate() { CourseId = course.Id, Date = date.Date });
                    }
                }

                context.SaveChanges();

            }
        }
    }
}