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
    public class ModelCarsController : Controller
    {
        private readonly AutomobilesDBContext _context;

        public ModelCarsController(AutomobilesDBContext context)
        {
            _context = context;
        }

        // GET: ModelCars
        public async Task<IActionResult> Index(int? body_id, int? eng_id, int? price_id, int? car_id)
        {
            if (body_id != null)
            {
                var automobilesDBContext1 = _context.ModelCar.Where(m=>m.IdBody==body_id).Include(m => m.IdBodyNavigation).Include(m => m.IdEngineNavigation).Include(m => m.IdPriceNavigation);
                return View(await automobilesDBContext1.ToListAsync());
            };
            if (eng_id != null)
            {
                var automobilesDBContext1 = _context.ModelCar.Where(m => m.IdEngine == eng_id).Include(m => m.IdBodyNavigation).Include(m => m.IdEngineNavigation).Include(m => m.IdPriceNavigation);
                return View(await automobilesDBContext1.ToListAsync());
            };
            if (price_id != null)
            {
                var automobilesDBContext1 = _context.ModelCar.Where(m => m.IdPrice == price_id).Include(m => m.IdBodyNavigation).Include(m => m.IdEngineNavigation).Include(m => m.IdPriceNavigation);
                return View(await automobilesDBContext1.ToListAsync());
            };
            if (car_id != null)
            {
                var automobilesDBContext1 = _context.ModelCar.Where(m => m.Id == car_id).Include(m => m.IdBodyNavigation).Include(m => m.IdEngineNavigation).Include(m => m.IdPriceNavigation);
                return View(await automobilesDBContext1.ToListAsync());
            };
            
            var automobilesDBContext = _context.ModelCar.Include(m => m.IdBodyNavigation).Include(m => m.IdEngineNavigation).Include(m => m.IdPriceNavigation);
            return View(await automobilesDBContext.ToListAsync());
        }

        // GET: ModelCars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCar = await _context.ModelCar
                .Include(m => m.IdBodyNavigation)
                .Include(m => m.IdEngineNavigation)
                .Include(m => m.IdPriceNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelCar == null)
            {
                return NotFound();
            }
            int? car_id = id;
            return RedirectToAction("Index", "ModelCarYears",new { car_id});
        }

        // GET: ModelCars/Create
        public IActionResult Create()
        {
            ViewData["IdBody"] = new SelectList(_context.BodyType, "Id", "TypeName");
            ViewData["IdEngine"] = new SelectList(_context.Engine, "Id", "EngineCapacity");
            ViewData["IdPrice"] = new SelectList(_context.PriceCategory, "Id", "Price");
            return View();
        }

        // POST: ModelCars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelName,IdEngine,IdBody,IdPrice")] ModelCar modelCar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modelCar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBody"] = new SelectList(_context.BodyType, "Id", "TypeName", modelCar.IdBody);
            ViewData["IdEngine"] = new SelectList(_context.Engine, "Id", "EngineCapacity", modelCar.IdEngine);
            ViewData["IdPrice"] = new SelectList(_context.PriceCategory, "Id", "Price", modelCar.IdPrice);
            return View(modelCar);
        }

        // GET: ModelCars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCar = await _context.ModelCar.FindAsync(id);
            if (modelCar == null)
            {
                return NotFound();
            }
            ViewData["IdBody"] = new SelectList(_context.BodyType, "Id", "TypeName", modelCar.IdBody);
            ViewData["IdEngine"] = new SelectList(_context.Engine, "Id", "EngineCapacity", modelCar.IdEngine);
            ViewData["IdPrice"] = new SelectList(_context.PriceCategory, "Id", "Price", modelCar.IdPrice);
            return View(modelCar);
        }

        // POST: ModelCars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelName,IdEngine,IdBody,IdPrice")] ModelCar modelCar)
        {
            if (id != modelCar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelCar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelCarExists(modelCar.Id))
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
            ViewData["IdBody"] = new SelectList(_context.BodyType, "Id", "TypeName", modelCar.IdBody);
            ViewData["IdEngine"] = new SelectList(_context.Engine, "Id", "EngineCapacity", modelCar.IdEngine);
            ViewData["IdPrice"] = new SelectList(_context.PriceCategory, "Id", "Price", modelCar.IdPrice);
            return View(modelCar);
        }

        // GET: ModelCars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCar = await _context.ModelCar
                .Include(m => m.IdBodyNavigation)
                .Include(m => m.IdEngineNavigation)
                .Include(m => m.IdPriceNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelCar == null)
            {
                return NotFound();
            }

            return View(modelCar);
        }

        // POST: ModelCars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carB = _context.ModelCarYear.Where(d => d.IdCar == id).Include(d => d.IdCarNavigation).Include(d => d.IdYearNavigation).ToList();
            _context.ModelCarYear.RemoveRange(carB);
            await _context.SaveChangesAsync();
            var modelCar = await _context.ModelCar.FindAsync(id);
            _context.ModelCar.Remove(modelCar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelCarExists(int id)
        {
            return _context.ModelCar.Any(e => e.Id == id);
        }
    }
}
