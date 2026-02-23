namespace RealEstateApp.Application.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}