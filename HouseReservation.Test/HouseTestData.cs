using HouseReservation.Contracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Test
{
    public static class HouseTestData
    {
        public static readonly HouseEditViewModel ValidHouse = new()
        {
            Id = null,
            Title = "Charming Cottage by the Lake",
            Description = "A beautiful lakeside cottage with modern amenities.",
            ImageUrl = "https://example.com/image.jpg",
            PricePerNight = 150.00m,
            SizeM2 = 80,
            Bedrooms = 3,
            Bathrooms = 2,
            Country = "Poland",
            City = "Kraków",
            State = "Małopolskie",
            StreetName = "Lakeside Avenue",
            StreetNumber = "12A",
            PostalCode = "30-001"
        };

        public static TheoryData<HouseEditViewModel> InvalidHouses =>
        [ 
        // Missing Title
            new HouseEditViewModel
            {
                Description = "Nice place",
                ImageUrl = "https://example.com",
                PricePerNight = 100,
                SizeM2 = 50,
                Bedrooms = 2,
                Bathrooms = 1,
                Country = "Poland",
                StreetName = "Main Street",
                StreetNumber = "5",
                PostalCode = "00-001"
            },
        // Invalid Image URL
            new HouseEditViewModel
            {
                Title = "Cozy Cabin",
                Description = "Nice place",
                ImageUrl = "not-a-url",
                PricePerNight = 100,
                SizeM2 = 50,
                Bedrooms = 2,
                Bathrooms = 1,
                Country = "Poland",
                StreetName = "Main Street",
                StreetNumber = "5",
                PostalCode = "00-001"
            },
        // Price below minimum
            new HouseEditViewModel
            {
                Title = "Budget Room",
                Description = "Cheap place",
                ImageUrl = "https://example.com",
                PricePerNight = -5,
                SizeM2 = 30,
                Bedrooms = 1,
                Bathrooms = 1,
                Country = "Poland",
                StreetName = "Budget St",
                StreetNumber = "2",
                PostalCode = "00-002"
            },
        // Bedrooms too high
            new HouseEditViewModel
            {
                Title = "Huge House",
                Description = "Has many rooms",
                ImageUrl = "https://example.com",
                PricePerNight = 200,
                SizeM2 = 300,
                Bedrooms = 25,
                Bathrooms = 3,
                Country = "Poland",
                StreetName = "Rich Street",
                StreetNumber = "10",
                PostalCode = "00-003"
            },
        ];
    }
}
