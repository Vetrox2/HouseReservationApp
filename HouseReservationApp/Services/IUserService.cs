using HouseReservationApp.Models;
using HouseReservationApp.Models.DB.Entities;
using HouseReservationApp.Models.ViewModels;

namespace HouseReservationApp.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<UserEditViewModel?> GetUserEditViewModelAsync(int id);
        Task<ServiceResult> CreateUserAsync(UserCreateViewModel viewModel);
        Task<ServiceResult> UpdateUserAsync(int id, UserEditViewModel viewModel);
        Task<bool> DeleteUserAsync(int id);
        Task<PagedResult<User>> GetPaginatedUsersAsync(int page, int pageSize);
    }
}
