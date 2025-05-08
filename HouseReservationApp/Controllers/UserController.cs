using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseReservation.Web.Controllers
{
    [Authorize(Policy = "AtLeastUser")]
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        public async Task<IActionResult> Index(UserIndexParams parameters)
        {
            if (parameters.Page < 1) parameters = parameters with { Page = 1 };
            if (parameters.PageSize < 1) parameters = parameters with { PageSize = 10 };

            var viewModel = await _userService.GetUserIndexViewModelAsync(parameters);
            return View(viewModel);
        }

        [Authorize(Policy = "AdminOrManager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOrManager")]
        public async Task<IActionResult> Create(UserCreateViewModel viewModel)
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

        [Authorize(Policy = "AdminOrManager")]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _userService.GetUserEditViewModelAsync(id);
            if (viewModel == null) return NotFound();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOrManager")]
        public async Task<IActionResult> Edit(UserEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(viewModel);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
            => await _userService.DeleteUserAsync(id) ? RedirectToAction("Index") : NotFound();

    }
}