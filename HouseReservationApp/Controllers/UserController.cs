using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HouseReservation.Web.Controllers
{
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        public async Task<IActionResult> Index(UserIndexParams parameters)
        {
            if (parameters.Page < 1) parameters = parameters with { Page = 1 };
            if (parameters.PageSize < 1) parameters = parameters with { PageSize = 10 };

            var pagedResult = await _userService.GetPaginatedUsersAsync(parameters);

            ViewData["FirstName"] = parameters.FirstName;
            ViewData["LastName"] = parameters.LastName;
            ViewData["SortBy"] = parameters.SortBy;
            ViewData["SortDirection"] = parameters.SortDirection?.ToString();

            return View(pagedResult);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,BankAccount,Phone,Email,Password,ConfirmPassword")] UserCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(viewModel);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        foreach (var msg in error.Value)
                        {
                            ModelState.AddModelError(error.Key, msg);
                        }
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _userService.GetUserEditViewModelAsync(id);
            if (viewModel == null) return NotFound();
            ViewData["UserId"] = id;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,BankAccount,Phone")] UserEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(id, viewModel);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        foreach (var msg in error.Value)
                        {
                            ModelState.AddModelError(error.Key, msg);
                        }
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["UserId"] = id;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
            => await _userService.DeleteUserAsync(id) ? RedirectToAction("Index") : NotFound();

    }
}