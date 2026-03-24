namespace RealEstateApp.Application.DTOs.Favorite
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
        public string PropertyCity { get; set; } = string.Empty;
        public string PropertyCountry { get; set; } = string.Empty;
        public decimal PropertyPrice { get; set; }
        public string? PrimaryImageUrl { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal Area { get; set; }
        public DateTime AddedAt { get; set; }
    }
}