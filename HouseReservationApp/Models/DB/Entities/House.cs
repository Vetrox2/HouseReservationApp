namespace HouseReservationApp.Models.DB.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class House : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "House name")]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        [Required]
        public int SizeM2 { get; set; }

        [Required]
        public int Bedrooms { get; set; }

        [Required]
        public int Bathrooms { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [Required]
        [StringLength(255)]
        public string StreetName { get; set; }

        [Required]
        [StringLength(20)]
        public string StreetNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = [];
    }

}
