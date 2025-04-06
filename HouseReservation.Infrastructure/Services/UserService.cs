using HouseReservation.Contracts.Models;
using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Models;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Core.Utilities;
using HouseReservation.Infrastructure.Repositories;
using HouseReservation.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HouseReservation.Infrastructure.Services
{
    public class UserService(IRepository<User> repository) : IUserService
    {
        private readonly IRepository<User> _repository = repository;

        private static readonly Dictionary<string, Expression<Func<User, object>>> _propertyAccessors = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", u => u.Id },
            { "FirstName", u => u.FirstName },
            { "LastName", u => u.LastName },
            { "Email", u => u.Email },
            { "Phone", u => u.Phone },
            { "BankAccount", u => u.BankAccount },
            { "CreatedAt", u => u.CreatedAt }
        };

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<UserEditViewModel?> GetUserEditViewModelAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            return user.ToUserEditViewModel();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _repository.GetAll().FirstOrDefaultAsync(u => u.Email == email);
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

            var user = viewModel.ToUser(HashService.HashPassword(viewModel.Password));
            await _repository.AddAsync(user);

            result.Succeeded = true;
            return result;
        }

        public async Task<ServiceResult> UpdateUserAsync(UserEditViewModel viewModel)
        {
            var result = new ServiceResult();
            var existingUser = await _repository.GetByIdAsync(viewModel.Id);

            if (existingUser == null)
            {
                result.Errors.Add("", ["User not found."]);
                return result;
            }
            if (await _repository.ExistsAsync(u => u.BankAccount == viewModel.BankAccount && u.Id != viewModel.Id))
                result.Errors.Add("BankAccount", ["Bank account is already taken."]);
            if (result.Errors.Count != 0)
                return result;

            existingUser.UpdateFrom(viewModel);
            await _repository.UpdateAsync(existingUser);

            result.Succeeded = true;
            return result;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<UserIndexViewModel> GetUserIndexViewModelAsync(UserIndexParams parameters)
        {
            var pagedResult = await GetPaginatedUsersAsync(parameters);
            var viewModel = new UserIndexViewModel
            {
                PagedResult = pagedResult,
                FirstNameFilter = parameters.FirstName,
                LastNameFilter = parameters.LastName,
                SortBy = parameters.SortBy,
                SortDirection = parameters.SortDirection
            };

            return viewModel;
        }

        private async Task<PagedResult<UserDto>> GetPaginatedUsersAsync(UserIndexParams parameters)
        {
            var query = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(parameters.FirstName))
                query = query.Where(u => u.FirstName.ToLower().Contains(parameters.FirstName.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.LastName))
                query = query.Where(u => u.LastName.ToLower().Contains(parameters.LastName.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.SortBy) && parameters.SortDirection.HasValue && _propertyAccessors.TryGetValue(parameters.SortBy, out var accessor))
            {
                query = parameters.SortDirection == SortDirection.Ascending
                ? query.OrderBy(accessor)
                : query.OrderByDescending(accessor);
            }

            var pagedResult = await _repository.GetPaginatedAsync(parameters.Page, parameters.PageSize, query);
            var userDtos = pagedResult.Items.Select(u => u.ToUserDto());
            var result = new PagedResult<UserDto>(userDtos, pagedResult.TotalCount, pagedResult.CurrentPage, pagedResult.PageSize);

            return result;
        }
    }
}
