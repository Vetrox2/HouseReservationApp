namespace HouseReservationApp.Models
{
    public class ServiceResult
    {
        public bool Succeeded { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; } = [];
    }
}
