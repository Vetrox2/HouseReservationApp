using HouseReservation.Contracts.Models;
using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Models;
using Mapster;

namespace HouseReservation.Mappers
{
    public static class UserMapper
    {
        public static UserEditViewModel ToUserEditViewModel(this User user)
            => user.Adapt<UserEditViewModel>();

        public static void UpdateFrom(this User user, UserEditViewModel viewModel)
            => viewModel.Adapt(user);

        public static User ToUser(this UserCreateViewModel viewModel, string passwordHash)
        {
            var user = viewModel.Adapt<User>();
            user.PasswordHash = passwordHash;
            return user;
        }

        public static UserDto ToUserDto(this User user)
            => user.Adapt<UserDto>();
    }
}
