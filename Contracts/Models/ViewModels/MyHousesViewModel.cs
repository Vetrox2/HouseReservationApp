using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class MyHousesViewModel
    {
        public IEnumerable<HouseListItemViewModel> Houses { get; set; }
    }
}
