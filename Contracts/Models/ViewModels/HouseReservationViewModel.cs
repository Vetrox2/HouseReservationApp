using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class HouseReservationViewModel
    {
        [ValidateNever]
        public HouseDetailViewModel House { get; set; }
        public ReservationCreateViewModel Reservation { get; set; }
    }

}
