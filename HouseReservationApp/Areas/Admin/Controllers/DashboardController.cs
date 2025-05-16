using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseReservation.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrManager")]
    public class DashboardController(IUserService userService, IHouseService houseService, IReservationService reservationService) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var userCount = (await userService.GetAllUsersAsync()).Count();
            var houseCount = await houseService.GetTotalHousesAsync();
            var earningsReport = await reservationService.GetEarningsReportAsync(DateTime.Today);
            var sureEarnings = earningsReport.Months.Sum(x => x.SureEarnings);
            var possibleEarnings = earningsReport.Months.Sum(x => x.PossibleEarnings);

            var vm = new DashboardViewModel
            {
                UserCount = userCount,
                HouseCount = houseCount,
                EarningsReport = earningsReport,
                SureEarnings = sureEarnings,
                PossibleEarnings = possibleEarnings
            };

            return View(vm);
        }

        public async Task<IActionResult> Earnings(DateTime? from, DateTime? to)
        {
            var report = await reservationService.GetEarningsReportAsync(from, to);
            return View(report);
        }
    }
}
