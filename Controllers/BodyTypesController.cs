using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarsLab;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;
using System.Text.RegularExpressions;

namespace CarsLab.Controllers
{
    public class BodyTypesController : Controller
    {
        private readonly AutomobilesDBContext _context;

        public BodyTypesController(AutomobilesDBContext context)
        {
            _context = context;
        }
        public bool CheckRange(string Price)
        {
            var result = true;
            if (Price.Contains('-'))
            {
                string[] tokens = Price.Split("-", StringSplitOptions.None);
                int a = Int32.Parse(tokens[0]);
                string[] b = tokens[1].Split('$', StringSplitOptions.None);
                int b1 = Int32.Parse(b[0]);
                if (a >= b1) result = false;
                return result;
            }
            return result;
        }

        // GET: BodyTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.BodyType.ToListAsync());
        }
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {

            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {

                    using var stream = new FileStream(fileExcel.FileName, FileMode.Create);
                    await fileExcel.CopyToAsync(stream);
                    using XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled);
                    List<List<string>> Errors = new List<List<string>>();
                    foreach (IXLWorksheet worksheet in workBook.Worksheets)
                    {
                        BodyType bodyType;
                        var g = (from type in _context.BodyType
                                 where type.TypeName.Contains(worksheet.Name)
                                 select type).ToList();
                        if (g.Count > 0)
                        {
                            bodyType = g[0];
                        }
                        else
                        {
                            bodyType = new BodyType
                            {
                                TypeName = worksheet.Name
                            };
                            _context.BodyType.Add(bodyType);

                        }
                        await _context.SaveChangesAsync();
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            ModelCar carModel = new ModelCar();
                            string name = row.Cell(1).Value.ToString();
                            if (name.Length > 50 || name.Length < 3) continue;
                            var f = (from car in _context.ModelCar where car.ModelName.Contains(name) select car).ToList();
                            if (f.Count() > 0)
                            {
                                carModel = f[0]; 
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                carModel.ModelName = row.Cell(1).Value.ToString();
                                carModel.IdBodyNavigation = bodyType;
                                string name1 = row.Cell(2).Value.ToString();
                                var match = Regex.Match(row.Cell(2).Value.ToString(), @"^[1-9]+[0-9]*(\-[1-9]+[0-9]*)*\$$");
                                if (!match.Success) continue;
                                if (name1.Length > 50 || name1.Length < 3) continue;
                                if(CheckRange(name1)==false)continue;
                                PriceCategory price;
                                var a = (from pr in _context.PriceCategory
                                         where pr.Price.Contains(row.Cell(2).Value.ToString())
                                         select pr).ToList();
                                if (a.Count() > 0)
                                {
                                    price = a[0];
                                    carModel.IdPriceNavigation = price;
                                }
                                else
                                {
                                    price = new PriceCategory
                                    {
                                        Price = row.Cell(2).Value.ToString()
                                    };
                                    _context.Add(price);
                                    carModel.IdPriceNavigation = price;
                                }
                                string name2 = row.Cell(3).Value.ToString();
                                var match1 = Regex.Match(row.Cell(3).Value.ToString(), @"^(0|[1-9]+[0-9]*)(\.[0-9]+( )?)?L$");
                                if (!match1.Success) continue;
                                if (name2.Length > 50 || name2.Length < 3) continue;
                                Engine eng;
                                var b = (from pr in _context.Engine
                                         where pr.EngineCapacity.Contains(row.Cell(3).Value.ToString())
                                         select pr).ToList();
                                if (b.Count() > 0)
                                {
                                    eng = b[0];
                                    carModel.IdEngineNavigation = eng;
                                }
                                else
                                {
                                    eng = new Engine
                                    {
                                        EngineCapacity = row.Cell(3).Value.ToString()
                                    };
                                    _context.Add(eng);
                                    carModel.IdEngineNavigation = eng;
                                }
                                _context.ModelCar.Add(carModel);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "ModelCars");
        }
        // GET: BodyTypes/Details/5
        public ActionResult Export(int? id)
        {
            using XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled);
            string bodyType = _context.BodyType.FirstOrDefault(m => m.Id == id).TypeName;
            var bodyTypecars = _context.ModelCar.Where(f => f.IdBody == id).Include(f => f.IdBodyNavigation).Include(f => f.IdEngineNavigation).Include(f => f.IdPriceNavigation).ToList();
            var worksheet = workbook.Worksheets.Add(bodyType);
            foreach (var g in bodyTypecars)
            {
                worksheet.Cell("A1").Value = "Назва моделі авто";
                worksheet.Cell("B1").Value = "Цінова категорія";
                worksheet.Cell("C1").Value = "Об'єм двигуна";
                worksheet.Cell("D1").Value = "Роки випуску";
                worksheet.Row(1).Style.Font.Bold = true;
                for (int i = 0; i < bodyTypecars.Count; i++)
                {
                    var carYears = _context.ModelCarYear.Where(c => c.IdCar == bodyTypecars[i].Id).Include(c => c.IdYearNavigation).Select(c=>c.IdYearNavigation.Year).ToList();
                    
                    var years = string.Join(" , ", carYears);
                    worksheet.Cell(i + 2, 1).Value = bodyTypecars[i].ModelName;
                    worksheet.Cell(i + 2, 2).Value = bodyTypecars[i].IdPriceNavigation.Price;
                    worksheet.Cell(i + 2, 3).Value = bodyTypecars[i].IdEngineNavigation.EngineCapacity;
                    worksheet.Cell(i + 2, 4).Value = years;
                }
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Flush();

            return new FileContentResult(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"IvanLab_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyType = await _context.BodyType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bodyType == null)
            {
                return NotFound();
            }
            int? body_id = id;

            return RedirectToAction("Index","ModelCars", new { body_id});
        }

        // GET: BodyTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BodyTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeName")] BodyType bodyType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bodyType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bodyType);
        }

        // GET: BodyTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyType = await _context.BodyType.FindAsync(id);
            if (bodyType == null)
            {
                return NotFound();
            }
            return View(bodyType);
        }

        // POST: BodyTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeName")] BodyType bodyType)
        {
            if (id != bodyType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bodyType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodyTypeExists(bodyType.Id))
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
            return View(bodyType);
        }

        // GET: BodyTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyType = await _context.BodyType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bodyType == null)
            {
                return NotFound();
            }

            return View(bodyType);
        }

        // POST: BodyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cars=_context.ModelCar.Where(c=>c.IdBody==id).Include(c => c.IdBodyNavigation).Include(c => c.IdEngineNavigation).Include(c => c.IdPriceNavigation).ToList();
            foreach(var c in cars)
            {
                var carB = _context.ModelCarYear.Where(d => d.IdCar ==c.Id).Include(d => d.IdCarNavigation).Include(d => d.IdYearNavigation).ToList();
                _context.ModelCarYear.RemoveRange(carB);
                await _context.SaveChangesAsync();
            }
            _context.ModelCar.RemoveRange(cars);
            await _context.SaveChangesAsync();
            var bodyType = await _context.BodyType.FindAsync(id);
            _context.BodyType.Remove(bodyType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BodyTypeExists(int id)
        {
            return _context.BodyType.Any(e => e.Id == id);
        }
    }
}
