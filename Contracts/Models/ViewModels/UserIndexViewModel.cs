namespace HouseReservation.Contracts.Models.ViewModels
{
    public class UserIndexViewModel
    {
        public PagedResult<UserDto> PagedResult { get; set; }
        public string? FirstNameFilter { get; set; }
        public string? LastNameFilter { get; set; }
        public string? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }
        public string? SortDirectionIcon => SortDirection is not null ? SortDirection == Models.SortDirection.Ascending ? "↑" : "↓" : null;
    }
}
