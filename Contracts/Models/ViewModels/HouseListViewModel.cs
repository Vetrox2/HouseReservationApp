using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class HouseListViewModel
    {
        public PagedResult<HouseListItemViewModel> PagedHouses { get; set; }
        public HouseFilterViewModel Filter { get; set; }
    }
}
