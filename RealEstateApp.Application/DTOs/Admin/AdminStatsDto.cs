namespace RealEstateApp.Application.DTOs.Admin
{
    public class AdminStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalProperties { get; set; }
        public int TotalBookings { get; set; }
        public int TotalReviews { get; set; }
        public int TotalFavorites { get; set; }


        public int NewUsersThisMonth { get; set; }
        public int NewPropertiesThisMonth { get; set; }
        public int BookingsThisMonth { get; set; }


        public int PendingBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int CancelledBookings { get; set; }


        public Dictionary<string, int> PropertiesByType { get; set; } = new();
        public Dictionary<string, int> PropertiesByCity { get; set; } = new();
        public Dictionary<string, int> PropertiesByListingType { get; set; } = new();

        public List<TopRatedPropertyDto> TopRatedProperties { get; set; } = new();
    }

    public class TopRatedPropertyDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}