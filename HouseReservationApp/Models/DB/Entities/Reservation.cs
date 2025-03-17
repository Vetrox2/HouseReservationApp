using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HouseReservationApp.Models.DB.Entities
{
    public class Reservation : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HouseId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public ReservationStatus Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("HouseId")]
        public House House { get; set; }
    }
}
