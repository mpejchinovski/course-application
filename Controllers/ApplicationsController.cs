using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseApp.Data;
using CourseApp.Models;
using System.Text.RegularExpressions;

namespace CourseApp.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly CourseAppDbContext _context;

        public ApplicationsController(CourseAppDbContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            var courseAppDbContext = _context.Application.Include(a => a.CourseDate).ThenInclude(cd => cd.Course);
            return View(await courseAppDbContext.ToListAsync());
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .Include(a => a.CourseDate)
                .ThenInclude(cd => cd.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name");
            ViewData["CourseDateId"] = new SelectList(string.Empty, "Id", "Date");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseDateId,CompanyName,PhoneNumber,Email")] Application application)
        {
            if (ModelState.IsValid)
            {
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name");
            return View(application);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var application = _context.Application.Where(a => a.Id == id).AsQueryable();
            if (application == null)
            {
                return NotFound();
            }

            var course = await application.Select(p => p.CourseDate.Course).FirstAsync();

            IEnumerable<SelectListItem> dateSelectList =
                from cd in _context.CourseDate.Where(cd => cd.CourseId == course.Id)
                select new SelectListItem
                {
                    Selected = (cd.Id == id),
                    Text = cd.Date.ToShortDateString(),
                    Value = String.Format("{0}", cd.Id)
                };

            if (id == null)
            {
                return NotFound();
            }

            ViewData["CourseName"] = course.Name;
            ViewData["CourseDateId"] = dateSelectList;
            ViewData["CompanyName"] = application.First().CompanyName;
            return View(await application.FirstAsync());
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseDateId,CompanyName,PhoneNumber,Email")] Application application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .Include(a => a.CourseDate)
                .ThenInclude(cd => cd.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Application.FindAsync(id);
            _context.Application.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
            return _context.Application.Any(e => e.Id == id);
        }
    }
}
