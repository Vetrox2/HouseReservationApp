using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Models;
using HouseReservation.Core.Utilities;

namespace HouseReservation.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<UserEditViewModel?> GetUserEditViewModelAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<ServiceResult> CreateUserAsync(UserCreateViewModel viewModel);
        Task<ServiceResult> UpdateUserAsync(UserEditViewModel viewModel);
        Task<bool> DeleteUserAsync(int id);
        Task<UserIndexViewModel> GetUserIndexViewModelAsync(UserIndexParams parameters);
    }
}
