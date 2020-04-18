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
