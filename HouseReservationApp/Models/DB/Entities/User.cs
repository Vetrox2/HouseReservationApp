using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HouseReservationApp.Models.DB.Entities
{
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }

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
        public string Email { get; set; }

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
        public string PasswordHash { get; set; }

        [Required]
        [DisplayName("Created at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<House> Houses { get; set; } = [];
    }
}
