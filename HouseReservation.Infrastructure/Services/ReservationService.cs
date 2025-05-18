using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Models;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Infrastructure.Services
{
    public class ReservationService(IRepository<Reservation> resRepo) : IReservationService
    {
        private static decimal Commission = 0.05M;

        public async Task ReserveHouseAsync(ReservationCreateViewModel model)
        {
            var days = model.CheckOutDate - model.CheckInDate;
            var price = days.Days * model.PricePerNight;
            var r = new Reservation
            {
                HouseId = model.HouseId,
                UserId = model.UserId,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                Price = price,
                Status = ReservationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            await resRepo.AddAsync(r);
        }

        public async Task<IEnumerable<Reservation>> GetMyReservationsAsync(int userId)
        {
            return await resRepo.GetAll()
                .Where(r => r.UserId == userId)
                .Include(r => r.House)
                .ToListAsync();
        }

        public async Task CancelReservationAsync(int reservationId, int userId)
        {
            var r = await resRepo.GetByIdAsync(reservationId);
            if (r == null) return;
            if (r.UserId != userId) return;

            r.Status = ReservationStatus.Cancelled;
            await resRepo.UpdateAsync(r);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsForHouseAsync(int houseId)
        {
            return await resRepo.GetAll()
                .Where(r => r.HouseId == houseId && r.Status != ReservationStatus.Cancelled)
                .ToListAsync();
        }

        public async Task<bool> IsOverlappingAsync(int houseId, DateTime ci, DateTime co)
        {
            return await resRepo.GetAll()
                .AnyAsync(r =>
                    r.HouseId == houseId &&
                    r.Status != ReservationStatus.Cancelled &&
                    r.CheckInDate < co &&
                    r.CheckOutDate > ci);
        }

        public async Task<EarningsReportViewModel> GetEarningsReportAsync(DateTime? from = null, DateTime? to = null)
        {
            var today = DateTime.Today;

            var start = from.HasValue
                ? new DateTime(from.Value.Year, from.Value.Month, 1)
                : DateTime.Today.AddMonths(-5).AddDays(1 - DateTime.Today.Day);

            var end = to.HasValue
                ? new DateTime(to.Value.Year, to.Value.Month, 1).AddMonths(1).AddDays(-1)
                : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);

            var all = await resRepo.GetAll()
                .Where(r => r.Status != ReservationStatus.Cancelled &&
                            r.CheckInDate <= end && r.CheckInDate >= start)
                .AsNoTracking()
                .ToListAsync();

            var byMonth = all
                .Select(r =>
                {
                    var margin = r.Price * Commission;
                    var key = new { r.CheckInDate.Year, r.CheckInDate.Month };

                    bool isSure =
                        r.Status == ReservationStatus.Confirmed
                        || r.CheckInDate <= today;

                    bool isPossible =
                        !isSure
                        && r.Status == ReservationStatus.Pending
                        && r.CheckInDate > today;

                    return new
                    {
                        key.Year,
                        key.Month, 
                        Sure = isSure ? margin : 0,
                        Possible = isPossible ? margin : 0
                    };
                })
                .GroupBy(x => new { x.Year, x.Month })
                .Select(g => new MonthlyEarningsViewModel
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    SureEarnings = g.Sum(x => x.Sure),
                    PossibleEarnings = g.Sum(x => x.Possible)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();

            return new EarningsReportViewModel
            {
                Months = byMonth
            };
        }

    }
}
