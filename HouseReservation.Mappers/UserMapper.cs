using HouseReservation.Contracts.Models;
using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Models;

namespace HouseReservation.Mappers
{
    public static class UserMapper
    {
        public static UserEditViewModel ToUserEditViewModel(this User user)
        {
            return new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                BankAccount = user.BankAccount
            };
        }

        public static void UpdateFrom(this User user, UserEditViewModel viewModel)
        {
            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Phone = viewModel.Phone;
            user.BankAccount = viewModel.BankAccount;
        }

        public static User ToUser(this UserCreateViewModel viewModel, string passwordHash)
        {
            return new User
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                BankAccount = viewModel.BankAccount,
                PasswordHash = passwordHash
            };
        }

        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                BankAccount = user.BankAccount,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
