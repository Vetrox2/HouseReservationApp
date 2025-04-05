using HouseReservation.Core.Models;
using HouseReservation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace HouseReservation.Web.Controllers
{
    public class HouseController : Controller
    {
        private readonly HouseReservationContext _db;
        public HouseController(HouseReservationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(House house)
        {
            if (ModelState.IsValid)
            {
                _db.Add(house);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
