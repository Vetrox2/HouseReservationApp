using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class HouseDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal PricePerNight { get; set; }
        public int SizeM2 { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Address { get; set; }
        public bool IsAvailable(DateTime checkIn, DateTime checkOut) => true; // sprawdź w serwisie
    }
}
