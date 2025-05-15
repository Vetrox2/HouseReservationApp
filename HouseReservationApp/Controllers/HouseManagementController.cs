using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Models;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseReservation.Web.Controllers
{
    [Authorize]
    public class HouseManagementController(IHouseService houseService) : Controller
    {
        public async Task<IActionResult> MyHouses()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Forbid();

            var list = await houseService.GetMyHousesAsync(userId);
            return View(new MyHousesViewModel { Houses = list });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await houseService.GetHouseEditViewModelAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HouseEditViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Forbid();

            await houseService.UpdateHouseAsync(vm, userId);
            return RedirectToAction(nameof(MyHouses));
        }

        public IActionResult Create() => View(new HouseEditViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HouseEditViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Forbid();

            await houseService.AddHouseAsync(vm, userId);
            return RedirectToAction(nameof(MyHouses));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Forbid();

            await houseService.DeleteHouseAsync(id, userId);
            return RedirectToAction(nameof(MyHouses));
        }
    }
}
