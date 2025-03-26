using HouseReservationApp.Models;
using HouseReservationApp.Models.DB.Entities;
using HouseReservationApp.Models.ViewModels;
using HouseReservationApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HouseReservationApp.Controllers
{
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        public async Task<IActionResult> Index(string firstName, string lastName, int page = 1, int pageSize = 10, string? sortBy = null, string? sortDirection = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            SortDirection? direction = null;
            if (!string.IsNullOrWhiteSpace(sortDirection))
            {
                direction = sortDirection.ToLower() == "desc"
                    ? SortDirection.Descending
                    : SortDirection.Ascending;
            }

            var pagedResult = await _userService.GetPaginatedUsersAsync(firstName, lastName, page, pageSize, sortBy, direction);

            ViewData["FirstName"] = firstName;
            ViewData["LastName"] = lastName;
            ViewData["SortBy"] = sortBy;
            ViewData["SortDirection"] = sortDirection;

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