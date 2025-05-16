using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseReservation.Web.Controllers
{
    public class HouseController(IHouseService houseService, IReservationService reservationService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var house = await houseService.GetHouseDetailAsync(id);
            if (house == null) return NotFound();

            var vm = new HouseReservationViewModel
            {
                House = house,
                Reservation = new ReservationCreateViewModel
                {
                    HouseId = id,
                    PricePerNight = house.PricePerNight
                }
            };

            ViewData["ExistingReservations"] =
                await reservationService.GetReservationsForHouseAsync(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(HouseReservationViewModel vm)
        {
            if (vm.Reservation.CheckInDate >= vm.Reservation.CheckOutDate)
            {
                ModelState.AddModelError("", "Check-out must be after check-in. You cannot book for less than one night.");
            }

            var overlap = await reservationService
                .IsOverlappingAsync(vm.Reservation.HouseId,
                                   vm.Reservation.CheckInDate,
                                   vm.Reservation.CheckOutDate);
            if (overlap)
                ModelState.AddModelError("", "These dates are already booked.");

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Forbid();
            vm.Reservation.UserId = userId;

            if (!ModelState.IsValid)
            {
                vm.House = await houseService.GetHouseDetailAsync(vm.Reservation.HouseId);
                ViewData["ExistingReservations"] =
                    await reservationService.GetReservationsForHouseAsync(vm.Reservation.HouseId);

                return View("Details", vm);
            }

            await reservationService.ReserveHouseAsync(vm.Reservation);
            return RedirectToAction(nameof(Details), new { id = vm.Reservation.HouseId });
        }
    }
}
