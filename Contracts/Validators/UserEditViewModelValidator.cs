using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HouseReservation.Contracts.Models.ViewModels;

namespace HouseReservation.Contracts.Validators
{
    public class UserEditViewModelValidator : AbstractValidator<UserEditViewModel>
    {
        public UserEditViewModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User Id must be greater than 0.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(100).WithMessage("First Name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone Number is required.")
                .MaximumLength(20).WithMessage("Phone Number cannot exceed 20 characters.")
                .Matches(@"^\+?[0-9\s\-()]+$").WithMessage("The Phone Number format is not valid.");

            RuleFor(x => x.BankAccount)
                .NotEmpty().WithMessage("Bank Account is required.")
                .MaximumLength(50).WithMessage("Bank Account cannot exceed 50 characters.");
        }
    }
}
