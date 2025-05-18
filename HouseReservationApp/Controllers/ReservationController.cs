using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseReservation.Web.Controllers
{
    [Authorize(Policy ="AtLeastUser")]
    public class ReservationController(IReservationService reservationService) : Controller
    {

        public async Task<IActionResult> MyReservations()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Forbid();

            var list = await reservationService.GetMyReservationsAsync(userId);
            return View(list);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Forbid();

            await reservationService.CancelReservationAsync(id, userId);
            return RedirectToAction(nameof(MyReservations));
        }
    }
}
