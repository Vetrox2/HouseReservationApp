using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Models;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _resRepo;
        public ReservationService(IRepository<Reservation> resRepo)
        {
            _resRepo = resRepo;
        }

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
            await _resRepo.AddAsync(r);
        }

        public async Task<IEnumerable<Reservation>> GetMyReservationsAsync(int userId)
        {
            return await _resRepo.GetAll()
                .Where(r => r.UserId == userId)
                .Include(r => r.House)
                .ToListAsync();
        }

        public async Task CancelReservationAsync(int reservationId, int userId)
        {
            var r = await _resRepo.GetByIdAsync(reservationId);
            if (r == null) return;
            if (r.UserId != userId) return;

            r.Status = ReservationStatus.Cancelled;
            await _resRepo.UpdateAsync(r);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsForHouseAsync(int houseId)
        {
            return await _resRepo.GetAll()
                .Where(r => r.HouseId == houseId && r.Status != ReservationStatus.Cancelled)
                .ToListAsync();
        }

        public async Task<bool> IsOverlappingAsync(int houseId, DateTime ci, DateTime co)
        {
            return await _resRepo.GetAll()
                .AnyAsync(r =>
                    r.HouseId == houseId &&
                    r.Status != ReservationStatus.Cancelled &&
                    r.CheckInDate < co &&
                    r.CheckOutDate > ci);
        }

    }
}
