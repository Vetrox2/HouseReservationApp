using HouseReservation.Core.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HouseReservation.Core.Models
{
    public class User : IdentityUser<int>, IEntity
    {
        [Key]
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        [DisplayName("Email")]
        public override string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Bank Account")]
        public string BankAccount { get; set; }

        [Required]
        [DisplayName("Password")]
        public override string PasswordHash { get; set; }

        [Required]
        [DisplayName("Created at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<House> Houses { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = [];

    }
}
