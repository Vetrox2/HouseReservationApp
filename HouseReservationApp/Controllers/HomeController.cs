using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HouseReservation.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHouseService _houseService;
        public HomeController(IHouseService houseService)
        {
            _houseService = houseService;
        }
        public async Task<IActionResult> Index(HouseFilterViewModel filter)
        {
            var vm = new HouseListViewModel
            {
                PagedHouses = await _houseService.GetHousesAsync(filter),
                Filter = filter
            };
            return View(vm);
        }
    }
}
