using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Core.Services.Interfaces
{
    public interface IReservationService
    {
        Task ReserveHouseAsync(ReservationCreateViewModel model);
        Task<IEnumerable<Reservation>> GetMyReservationsAsync(int userId);
        Task CancelReservationAsync(int reservationId, int userId);
        Task<IEnumerable<Reservation>> GetReservationsForHouseAsync(int houseId);
        Task<bool> IsOverlappingAsync(int houseId, DateTime ci, DateTime co);
        Task<EarningsReportViewModel> GetEarningsReportAsync(DateTime? from = null, DateTime? to = null);
    }
}
