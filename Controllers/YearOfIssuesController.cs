using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarsLab;

namespace CarsLab.Controllers
{
    public class YearOfIssuesController : Controller
    {
        private readonly AutomobilesDBContext _context;

        public YearOfIssuesController(AutomobilesDBContext context)
        {
            _context = context;
        }

        // GET: YearOfIssues
        public async Task<IActionResult> Index()
        {
            return View(await _context.YearOfIssue.ToListAsync());
        }

        // GET: YearOfIssues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearOfIssue = await _context.YearOfIssue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yearOfIssue == null)
            {
                return NotFound();
            }
            int? year_id = id;

            return RedirectToAction("Index", "ModelCarYears", new { year_id });
        }

        // GET: YearOfIssues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: YearOfIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year")] YearOfIssue yearOfIssue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yearOfIssue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(yearOfIssue);
        }

        // GET: YearOfIssues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearOfIssue = await _context.YearOfIssue.FindAsync(id);
            if (yearOfIssue == null)
            {
                return NotFound();
            }
            return View(yearOfIssue);
        }

        // POST: YearOfIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Year")] YearOfIssue yearOfIssue)
        {
            if (id != yearOfIssue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yearOfIssue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearOfIssueExists(yearOfIssue.Id))
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
            return View(yearOfIssue);
        }

        // GET: YearOfIssues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearOfIssue = await _context.YearOfIssue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yearOfIssue == null)
            {
                return NotFound();
            }

            return View(yearOfIssue);
        }

        // POST: YearOfIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var carB = _context.ModelCarYear.Where(d => d.IdYear == id).Include(d => d.IdCarNavigation).Include(d => d.IdYearNavigation).ToList();
             _context.ModelCarYear.RemoveRange(carB);
            await _context.SaveChangesAsync();
            var yearOfIssue = await _context.YearOfIssue.FindAsync(id);
            _context.YearOfIssue.Remove(yearOfIssue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YearOfIssueExists(int id)
        {
            return _context.YearOfIssue.Any(e => e.Id == id);
        }
    }
}
