namespace RealEstateApp.Application.Interfaces
{
    public interface IImageService
    {
        // Upload the photo and return URL
        Task<string> UploadImageAsync(Stream imageStream, string fileName);

        // Delete the photo form cloudinary
        Task DeleteImageAsync(string imageUrl);
    }
}