using HouseReservationApp.Models.DB.Entities;
using HouseReservationApp.Models.ViewModels;
using HouseReservationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseReservationApp.Services
{
    public class UserService(IRepository<User> repository) : IUserService
    {
        private readonly IRepository<User> _repository = repository;

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<UserEditViewModel?> GetUserEditViewModelAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserEditViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BankAccount = user.BankAccount,
                Phone = user.Phone
            };
        }

        public async Task<ServiceResult> CreateUserAsync(UserCreateViewModel viewModel)
        {
            var result = new ServiceResult();

            if (await _repository.ExistsAsync(u => u.Email == viewModel.Email))
                result.Errors.Add("Email", ["Email address is already taken."]);
            if (await _repository.ExistsAsync(u => u.BankAccount == viewModel.BankAccount))
                result.Errors.Add("BankAccount", ["Bank account is already taken."]);
            if (result.Errors.Count != 0)
                return result;

            var user = new User
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                BankAccount = viewModel.BankAccount,
                Phone = viewModel.Phone,
                Email = viewModel.Email,
                PasswordHash = HashService.HashPassword(viewModel.Password)
            };
            await _repository.AddAsync(user);

            result.Succeeded = true;
            return result;
        }

        public async Task<ServiceResult> UpdateUserAsync(int id, UserEditViewModel viewModel)
        {
            var result = new ServiceResult();
            var existingUser = await _repository.GetByIdAsync(id);

            if (existingUser == null)
            {
                result.Errors.Add("", ["User not found."]);
                return result;
            }
            if (await _repository.ExistsAsync(u => u.BankAccount == viewModel.BankAccount && u.Id != id))
                result.Errors.Add("BankAccount", ["Bank account is already taken."]);
            if (result.Errors.Count != 0)
                return result;

            existingUser.FirstName = viewModel.FirstName;
            existingUser.LastName = viewModel.LastName;
            existingUser.BankAccount = viewModel.BankAccount;
            existingUser.Phone = viewModel.Phone;
            await _repository.UpdateAsync(existingUser);

            result.Succeeded = true;
            return result;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<PagedResult<User>> GetPaginatedUsersAsync(string firstName, string lastName, int page, int pageSize)
        {
            var query = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(u => u.FirstName.ToLower().Contains(firstName.ToLower()));

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(u => u.LastName.ToLower().Contains(lastName.ToLower()));

            return await _repository.GetPaginatedAsync(page, pageSize, query);
        }
    }
}
