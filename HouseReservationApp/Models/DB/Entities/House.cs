namespace HouseReservationApp.Models.DB.Entities
{
    public class House
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int SizeM2 { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Country { get; set; }
        public string? State { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }
        public DateTime CreatedAt { get; set; }

        public User Owner { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
