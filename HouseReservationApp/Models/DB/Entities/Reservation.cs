namespace HouseReservationApp.Models.DB.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public House House { get; set; }
    }
}
