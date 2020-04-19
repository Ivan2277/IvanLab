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
    public class PriceCategoriesController : Controller
    {
        private readonly AutomobilesDBContext _context;

        public PriceCategoriesController(AutomobilesDBContext context)
        {
            _context = context;
        }

        public JsonResult CheckRange(string Price)
        {
            var result = true;
            if (Price.Contains('-'))
            {
                string[] tokens = Price.Split("-", StringSplitOptions.None);
                int a = Int32.Parse(tokens[0]);
                string[] b = tokens[1].Split('$', StringSplitOptions.None);
                int b1 = Int32.Parse(b[0]);
                if (a >= b1) result = false;
                return Json(result);
            }
            return Json(result);
        }
        // GET: PriceCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.PriceCategory.ToListAsync());
        }

        // GET: PriceCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceCategory = await _context.PriceCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (priceCategory == null)
            {
                return NotFound();
            }
            int? price_id = id;

            return RedirectToAction("Index", "ModelCars", new { price_id });
        }

        // GET: PriceCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PriceCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price")] PriceCategory priceCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priceCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(priceCategory);
        }

        // GET: PriceCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceCategory = await _context.PriceCategory.FindAsync(id);
            if (priceCategory == null)
            {
                return NotFound();
            }
            return View(priceCategory);
        }

        // POST: PriceCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price")] PriceCategory priceCategory)
        {
            if (id != priceCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priceCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceCategoryExists(priceCategory.Id))
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
            return View(priceCategory);
        }

        // GET: PriceCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceCategory = await _context.PriceCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (priceCategory == null)
            {
                return NotFound();
            }

            return View(priceCategory);
        }

        // POST: PriceCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cars = _context.ModelCar.Where(c => c.IdPrice == id).Include(c => c.IdBodyNavigation).Include(c => c.IdEngineNavigation).Include(c => c.IdPriceNavigation).ToList();
            foreach (var c in cars)
            {
                var carB = _context.ModelCarYear.Where(d => d.IdCar == c.Id).Include(d => d.IdCarNavigation).Include(d => d.IdYearNavigation).ToList();
                _context.ModelCarYear.RemoveRange(carB);
                await _context.SaveChangesAsync();
            }
            _context.ModelCar.RemoveRange(cars);
            await _context.SaveChangesAsync();
            var priceCategory = await _context.PriceCategory.FindAsync(id);
            _context.PriceCategory.Remove(priceCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceCategoryExists(int id)
        {
            return _context.PriceCategory.Any(e => e.Id == id);
        }
    }
}
