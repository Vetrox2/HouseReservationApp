using HouseReservationApp.Models.DB;
using HouseReservationApp.Models.DB.Entities;
using HouseReservationApp.Models.ViewModels;
using HouseReservationApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseReservationApp.Controllers
{
    public class UserController(IRepository<User> repository) : Controller
    {
        private readonly IRepository<User> _repository = repository;

        public async Task<IActionResult> Index()
        {
            var users = await _repository.GetAll().ToListAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,BankAccount,Phone,Email,PasswordHash")] User user)
        {
            if (await _repository.ExistsAsync(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email address is already taken.");
                return View(user);
            }

            if (await _repository.ExistsAsync(u => u.BankAccount == user.BankAccount))
            {
                ModelState.AddModelError("BankAccount", "Bank account is already taken.");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                user.PasswordHash = HashService.HashPassword(user.PasswordHash);

                await _repository.AddAsync(user);
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return NotFound();

            var viewModel = new UserEditViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BankAccount = user.BankAccount,
                Phone = user.Phone
            };

            ViewData["UserId"] = id;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,BankAccount,Phone")] UserEditViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            if (await _repository.ExistsAsync(u => u.BankAccount == viewModel.BankAccount))
            {
                ModelState.AddModelError("BankAccount", "Bank account is already taken.");
                return View(viewModel);
            }

            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            existingUser.FirstName = viewModel.FirstName;
            existingUser.LastName = viewModel.LastName;
            existingUser.BankAccount = viewModel.BankAccount;
            existingUser.Phone = viewModel.Phone;

            await _repository.UpdateAsync(existingUser);
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) => await _repository.DeleteAsync(id) ? RedirectToAction("Index") : NotFound();
    }
}
