using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class HouseEditViewModel
    {
        public int? Id { get; set; } 
        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, Url]
        public string ImageUrl { get; set; }

        [Required, Range(0, 10000)]
        public decimal PricePerNight { get; set; }

        [Required, Range(1, 10000)]
        public int SizeM2 { get; set; }

        [Required, Range(0, 20)]
        public int Bedrooms { get; set; }

        [Required, Range(0, 20)]
        public int Bathrooms { get; set; }

        [Required, StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [Required, StringLength(255)]
        public string StreetName { get; set; }

        [Required, StringLength(20)]
        public string StreetNumber { get; set; }

        [Required, StringLength(20)]
        public string PostalCode { get; set; }
    }
}
