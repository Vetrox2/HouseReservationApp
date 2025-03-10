using HouseReservationApp.Models.DB;
using HouseReservationApp.Models.DB.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HouseReservationApp.Controllers
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
            if(ModelState.IsValid){
                _db.Add(house);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
