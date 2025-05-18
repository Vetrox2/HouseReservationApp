using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class HouseListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public decimal PricePerNight { get; set; }
        public string City { get; set; }
    }

}
