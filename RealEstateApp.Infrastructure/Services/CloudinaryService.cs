using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Infrastructure.Services
{
    public class CloudinaryService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IConfiguration configuration)
        {
            var settings = configuration.GetSection("CloudinarySettings");

            var account = new Account(
                settings["CloudName"],
                settings["ApiKey"],
                settings["ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, imageStream),
                Folder = "real-estate",
                Transformation = new Transformation()
                    .Width(1200)
                    .Height(720)
                    .Crop("limit")
                    .Quality("auto")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if(result.Error != null)
                throw new Exception($"Image upload failed: {result.Error.Message}");

            return result.SecureUrl.ToString();
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            var publicId = GetPublicIdFromUrl(imageUrl);

            var deleteParams = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(deleteParams);
        }

        private string GetPublicIdFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segments = uri.Segments;
            var uploadIndex = Array.IndexOf(segments, "upload./");
            var publicIdWithVersion = string.Join("", segments.Skip(uploadIndex + 2));
            return Path.GetFileNameWithoutExtension(publicIdWithVersion);
        }
    }
}