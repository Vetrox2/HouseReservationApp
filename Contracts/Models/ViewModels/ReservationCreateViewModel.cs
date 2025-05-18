using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class ReservationCreateViewModel
    {
        public int HouseId { get; set; }
        public decimal PricePerNight { get; set; }
        public int UserId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
