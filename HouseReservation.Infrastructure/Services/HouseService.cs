using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Contracts.Models;
using HouseReservation.Core.Models;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace HouseReservation.Infrastructure.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository<House> _houseRepo;
        public HouseService(IRepository<House> houseRepo)
        {
            _houseRepo = houseRepo;
        }

        public async Task<PagedResult<HouseListItemViewModel>> GetHousesAsync(HouseFilterViewModel filter)
        {
            var query = _houseRepo
                .GetAll()
                .Include(h => h.Reservations)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(filter.City))
                query = query.Where(h => h.City == filter.City);

            if (filter.MinPrice.HasValue)
                query = query.Where(h => h.PricePerNight >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(h => h.PricePerNight <= filter.MaxPrice.Value);

            if (filter.Bedrooms.HasValue)
                query = query.Where(h => h.Bedrooms == filter.Bedrooms.Value);

            if (filter.Bathrooms.HasValue)
                query = query.Where(h => h.Bathrooms == filter.Bathrooms.Value);

            if (filter.CheckIn.HasValue && filter.CheckOut.HasValue)
            {
                var ci = filter.CheckIn.Value.Date;
                var co = filter.CheckOut.Value.Date;
                query = query.Where(h =>
                    !h.Reservations.Any(r =>
                        r.CheckInDate < co &&
                        r.CheckOutDate > ci
                    )
                );
            }

            query = (filter.SortBy, filter.SortDesc) switch
            {
                ("PricePerNight", false) => query.OrderBy(h => h.PricePerNight),
                ("PricePerNight", true) => query.OrderByDescending(h => h.PricePerNight),

                ("SizeM2", false) => query.OrderBy(h => h.SizeM2),
                ("SizeM2", true) => query.OrderByDescending(h => h.SizeM2),

                ("CreatedAt", false) => query.OrderBy(h => h.CreatedAt),
                ("CreatedAt", true) => query.OrderByDescending(h => h.CreatedAt),

                _ => query.OrderBy(h => h.CreatedAt)
            };

            var paged = await _houseRepo.GetPaginatedAsync(filter.Page, filter.PageSize, query);
            var vmItems = paged.Items.Select(h => new HouseListItemViewModel
            {
                Id = h.Id,
                Title = h.Title,
                ImageUrl = h.ImageUrl,
                PricePerNight = h.PricePerNight,
                City = h.City
            });

            return new PagedResult<HouseListItemViewModel>(
                vmItems,
                paged.TotalCount,
                paged.CurrentPage,
                paged.PageSize
            );
        }


        public async Task<HouseDetailViewModel?> GetHouseDetailAsync(int id)
        {
            var h = await _houseRepo.GetByIdAsync(id);
            if (h == null) return null;
            return new HouseDetailViewModel
            {
                Id = h.Id,
                Title = h.Title,
                Description = h.Description,
                ImageUrl = h.ImageUrl,
                PricePerNight = h.PricePerNight,
                SizeM2 = h.SizeM2,
                Bedrooms = h.Bedrooms,
                Bathrooms = h.Bathrooms,
                Address = $"{h.StreetName} {h.StreetNumber}, {h.PostalCode} {h.State}, {h.Country}"
            };
        }

        public async Task<HouseEditViewModel?> GetHouseEditViewModelAsync(int id)
        {
            var h = await _houseRepo.GetByIdAsync(id);
            if (h == null) return null;

            return h.Adapt<HouseEditViewModel>();
        }

        public async Task AddHouseAsync(HouseEditViewModel model, int userId)
        {
            var h = new House
            {
                OwnerId = userId,
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerNight = model.PricePerNight,
                SizeM2 = model.SizeM2,
                Bedrooms = model.Bedrooms,
                Bathrooms = model.Bathrooms,
                Country = model.Country,
                StreetName = model.StreetName,
                StreetNumber = model.StreetNumber,
                PostalCode = model.PostalCode,
                City = model.City,
                State = model.State,
                CreatedAt = DateTime.UtcNow
            };
            await _houseRepo.AddAsync(h);
        }

        public async Task UpdateHouseAsync(HouseEditViewModel model, int userId)
        {
            var h = await _houseRepo.GetByIdAsync((int)model.Id);
            if (h == null) return;
            if(h.OwnerId != userId) throw new UnauthorizedAccessException("You are not the owner of this house.");

            model.Adapt(h);
            await _houseRepo.UpdateAsync(h);
        }

        public async Task DeleteHouseAsync(int id, int userId)
        {
            var h = await _houseRepo.GetByIdAsync(id);
            if (h == null) return;
            if (h.OwnerId != userId) throw new UnauthorizedAccessException("You are not the owner of this house.");

            await _houseRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<HouseListItemViewModel>> GetMyHousesAsync(int ownerId)
        {
            var list = _houseRepo.GetAll().Where(h => h.OwnerId == ownerId).AsNoTracking();
            return await list.Select(h => h.Adapt<HouseListItemViewModel>()).ToListAsync();
        }

        public async Task<int> GetTotalHousesAsync()
        {
            return await _houseRepo.GetAll().CountAsync();
        }

    }
}
