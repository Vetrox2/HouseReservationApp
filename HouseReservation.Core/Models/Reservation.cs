using HouseReservation.Core.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseReservation.Core.Models
{
    public class Reservation : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HouseId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        public ReservationStatus Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("HouseId")]
        public House House { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
