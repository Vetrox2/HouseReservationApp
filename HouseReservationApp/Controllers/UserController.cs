using HouseReservationApp.Models.DB;
using HouseReservationApp.Models.DB.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseReservationApp.Controllers
{
    public class UserController(HouseReservationContext dbContext) : Controller
    {
        private readonly HouseReservationContext _dbContext = dbContext;

        public async Task<IActionResult> Index()
        {
            var users = await _dbContext.Users.ToListAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }
    }
}
