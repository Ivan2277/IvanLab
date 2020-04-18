
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarsLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly AutomobilesDBContext _context;
        public ChartsController(AutomobilesDBContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var bodyCar = _context.BodyType.Include(c => c.ModelCar).ToList();
            List<object> carBody = new List<object>();
            carBody.Add(new[] { "Тип кузовa", "Кількість авто" });
            foreach (var c in bodyCar)
            {
                carBody.Add(new object[] { c.TypeName, c.ModelCar.Count() });
            }
            return new JsonResult(carBody);
        }

        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
            var engines = _context.Engine.Include(c => c.ModelCar).ToList();
            List<object> carEng = new List<object>();
            carEng.Add(new[] { "Об'єм двигуна", "Кількість авто" });
            foreach (var c in engines)
            {
                carEng.Add(new object[] { c.EngineCapacity, c.ModelCar.Count() });
            }
            return new JsonResult(carEng);
        }
    }
}