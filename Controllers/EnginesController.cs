﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarsLab;

namespace CarsLab.Controllers
{
    public class EnginesController : Controller
    {
        private readonly AutomobilesDBContext _context;

        public EnginesController(AutomobilesDBContext context)
        {
            _context = context;
        }

        // GET: Engines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Engine.ToListAsync());
        }

        // GET: Engines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engine = await _context.Engine
                .FirstOrDefaultAsync(m => m.Id == id);
            if (engine == null)
            {
                return NotFound();
            }
            int? eng_id = id;

            return RedirectToAction("Index", "ModelCars", new { eng_id });
        }

        // GET: Engines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Engines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EngineCapacity")] Engine engine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(engine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(engine);
        }

        // GET: Engines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engine = await _context.Engine.FindAsync(id);
            if (engine == null)
            {
                return NotFound();
            }
            return View(engine);
        }

        // POST: Engines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EngineCapacity")] Engine engine)
        {
            if (id != engine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(engine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EngineExists(engine.Id))
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
            return View(engine);
        }

        // GET: Engines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engine = await _context.Engine
                .FirstOrDefaultAsync(m => m.Id == id);
            if (engine == null)
            {
                return NotFound();
            }

            return View(engine);
        }

        // POST: Engines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cars = _context.ModelCar.Where(c => c.IdEngine == id).Include(c => c.IdBodyNavigation).Include(c => c.IdEngineNavigation).Include(c => c.IdPriceNavigation).ToList();
            foreach (var c in cars)
            {
                var carB = _context.ModelCarYear.Where(d => d.IdCar == c.Id).Include(d => d.IdCarNavigation).Include(d => d.IdYearNavigation).ToList();
                _context.ModelCarYear.RemoveRange(carB);
                await _context.SaveChangesAsync();
            }
            _context.ModelCar.RemoveRange(cars);
            await _context.SaveChangesAsync();
            var engine = await _context.Engine.FindAsync(id);
            _context.Engine.Remove(engine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EngineExists(int id)
        {
            return _context.Engine.Any(e => e.Id == id);
        }
    }
}
