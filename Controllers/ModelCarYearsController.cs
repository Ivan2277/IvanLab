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
    public class ModelCarYearsController : Controller
    {
        private readonly AutomobilesDBContext _context;

        public ModelCarYearsController(AutomobilesDBContext context)
        {
            _context = context;
        }

        // GET: ModelCarYears
        public async Task<IActionResult> Index(int? year_id, int? car_id)
        {
            if (year_id != null)
            {
                var automobilesDBContext = _context.ModelCarYear.Where(m=>m.IdYear==year_id).Include(m => m.IdCarNavigation).Include(m => m.IdYearNavigation);
                return View(await automobilesDBContext.ToListAsync());
            }
            else if (car_id != null)
            {
                var automobilesDBContext = _context.ModelCarYear.Where(m => m.IdCar == car_id).Include(m => m.IdCarNavigation).Include(m => m.IdYearNavigation);
                return View(await automobilesDBContext.ToListAsync());
            }
            else
            {
                var automobilesDBContext = _context.ModelCarYear.Include(m => m.IdCarNavigation).Include(m => m.IdYearNavigation);
                return View(await automobilesDBContext.ToListAsync());
            }
        }

        // GET: ModelCarYears/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCarYear = await _context.ModelCarYear
                .Include(m => m.IdCarNavigation)
                .Include(m => m.IdYearNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelCarYear == null)
            {
                return NotFound();
            }
            int? car_id = modelCarYear.IdCar;

            return RedirectToAction("Index", "ModelCars", new { car_id });
        }

        // GET: ModelCarYears/Create
        public IActionResult Create()
        {
            ViewData["IdCar"] = new SelectList(_context.ModelCar, "Id", "ModelName");
            ViewData["IdYear"] = new SelectList(_context.YearOfIssue, "Id", "Id");
            return View();
        }

        // POST: ModelCarYears/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCar,IdYear")] ModelCarYear modelCarYear)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modelCarYear);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCar"] = new SelectList(_context.ModelCar, "Id", "ModelName", modelCarYear.IdCar);
            ViewData["IdYear"] = new SelectList(_context.YearOfIssue, "Id", "Id", modelCarYear.IdYear);
            return View(modelCarYear);
        }

        // GET: ModelCarYears/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCarYear = await _context.ModelCarYear.FindAsync(id);
            if (modelCarYear == null)
            {
                return NotFound();
            }
            ViewData["IdCar"] = new SelectList(_context.ModelCar, "Id", "ModelName", modelCarYear.IdCar);
            ViewData["IdYear"] = new SelectList(_context.YearOfIssue, "Id", "Id", modelCarYear.IdYear);
            return View(modelCarYear);
        }

        // POST: ModelCarYears/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCar,IdYear")] ModelCarYear modelCarYear)
        {
            if (id != modelCarYear.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelCarYear);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelCarYearExists(modelCarYear.Id))
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
            ViewData["IdCar"] = new SelectList(_context.ModelCar, "Id", "ModelName", modelCarYear.IdCar);
            ViewData["IdYear"] = new SelectList(_context.YearOfIssue, "Id", "Id", modelCarYear.IdYear);
            return View(modelCarYear);
        }

        // GET: ModelCarYears/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCarYear = await _context.ModelCarYear
                .Include(m => m.IdCarNavigation)
                .Include(m => m.IdYearNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelCarYear == null)
            {
                return NotFound();
            }

            return View(modelCarYear);
        }

        // POST: ModelCarYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modelCarYear = await _context.ModelCarYear.FindAsync(id);
            _context.ModelCarYear.Remove(modelCarYear);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelCarYearExists(int id)
        {
            return _context.ModelCarYear.Any(e => e.Id == id);
        }
    }
}
