using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Medi_Connect.Application.Interfaces.ISerives;

namespace Medi_Connect.Application.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUDNAME");
            var apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_APIKEY");
            var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_APISECRET");

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
                throw new Exception("Cloudinary credentials are not set");

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);

        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "YourFolder"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.StatusCode == HttpStatusCode.OK)
                return result.SecureUrl.ToString();

            throw new ApplicationException("Image upload failed");
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }

        public async Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> files)
        {
            var uploadedUrls = new List<string>();

            foreach (var file in files)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "Certificates"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.StatusCode == HttpStatusCode.OK)
                    uploadedUrls.Add(result.SecureUrl.ToString());
                else
                    throw new ApplicationException($"Upload failed for {file.FileName}");
            }

            return uploadedUrls;
        }

    }

}
