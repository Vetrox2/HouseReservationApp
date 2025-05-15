using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Core.Services.Interfaces
{
    public interface IHouseService
    {
        Task<PagedResult<HouseListItemViewModel>> GetHousesAsync(HouseFilterViewModel filter);
        Task<HouseDetailViewModel?> GetHouseDetailAsync(int id);
        Task AddHouseAsync(HouseEditViewModel model, int ownerId);
        Task UpdateHouseAsync(HouseEditViewModel model, int userId);
        Task DeleteHouseAsync(int id, int userId);
        Task<IEnumerable<HouseListItemViewModel>> GetMyHousesAsync(int ownerId);
        Task<HouseEditViewModel?> GetHouseEditViewModelAsync(int id);
    }
}
