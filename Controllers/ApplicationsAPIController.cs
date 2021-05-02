using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseApp.Data;
using CourseApp.Models;

namespace CourseApp.Controllers
{
    public class JsonApplication
    {
        public int Id { get; set; }
        public int CourseDateId { get; set; }
        public string CourseName { get; set; }
        public string Date { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public JsonApplication(int Id, int CourseNameId, string CourseName, string Date, string CompanyName, string PhoneNumber, string Email)
        {
            this.Id = Id;
            this.CourseDateId = CourseNameId;
            this.CourseName = CourseName;
            this.Date = Date;
            this.CompanyName = CompanyName;
            this.PhoneNumber = PhoneNumber;
            this.Email = Email;
        }
    }

    [Route("api/applications")]
    [ApiController]
    public class ApplicationsAPIController : ControllerBase
    {
        private readonly CourseAppDbContext _context;

        public ApplicationsAPIController(CourseAppDbContext context)
        {
            _context = context;
        }

        // GET: api/Applications
        [HttpGet]
        public ActionResult<IEnumerable<JsonApplication>> GetApplication()
        {
            var apps = _context.Application.AsQueryable();
            IEnumerable<JsonApplication> jsonApps =
                from app in apps
                select new JsonApplication(app.Id, app.CourseDateId, app.CourseDate.Course.Name,
                    app.CourseDate.Date.ToShortDateString(), app.CompanyName, app.PhoneNumber, app.Email);
                    
            return Ok(jsonApps);
        }

        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JsonApplication>> GetApplication(int id)
        {
            var app = await _context.Application.Where(a => a.Id == id).Include(a => a.CourseDate).ThenInclude(cd => cd.Course).FirstOrDefaultAsync();

            if (app == null)
            {
                return NotFound();
            }

            return new JsonApplication(app.Id, app.CourseDateId, app.CourseDate.Course.Name,
                    app.CourseDate.Date.ToShortDateString(), app.CompanyName, app.PhoneNumber, app.Email);
        }

        // PUT: api/Applications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(int id, Application application)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Applications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Application>> PostApplication(Application application)
        {
            _context.Application.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplication", new { id = application.Id }, application);
        }

        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Application>> DeleteApplication(int id)
        {
            var application = await _context.Application.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Application.Remove(application);
            await _context.SaveChangesAsync();

            return application;
        }

        private bool ApplicationExists(int id)
        {
            return _context.Application.Any(e => e.Id == id);
        }
    }
}
