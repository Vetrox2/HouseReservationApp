﻿using HouseReservation.Contracts.Models;

namespace HouseReservation.Core.Utilities
{
    public record UserIndexParams(
        string? FirstName = null,
        string? LastName = null,
        int Page = 1,
        int PageSize = 10,
        string? SortBy = null,
        SortDirection? SortDirection = null
    );
}
