using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseReservation.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrManager")]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        //private readonly IHouseService _houseService;
        //private readonly IReservationService _reservationService;

        public DashboardController(
            IUserService userService)
            //IHouseService houseService,
            //IReservationService reservationService)
        {
            _userService = userService;
            //_houseService = houseService;
            //_reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            var userCount = (await _userService.GetAllUsersAsync()).Count();
            var houseCount = 0;//await _houseService.GetTotalHousesAsync();
            var totalMargin = 0;//await _reservationService.GetTotalMarginAsync();

            var vm = new DashboardViewModel
            {
                UserCount = userCount,
                HouseCount = houseCount,
                TotalMargin = totalMargin
            };

            return View(vm);
        }
    }
}
