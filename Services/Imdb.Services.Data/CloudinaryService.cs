namespace Imdb.Services.Data.Contracts
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Imdb.Common;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        // TODO: Delete Picture
        public bool IsImageFileValid(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return false;
            }

            string[] validImageTypes = new string[]
            {
                "image/x-png",
                "image/gif",
                "image/jpeg",
                "image/jpg",
                "image/png",
                "image/gif",
                "image/svg",
            };

            if (!validImageTypes.Contains(imageFile.ContentType))
            {
                return false;
            }

            return true;
        }

        public async Task<string> UploudImageAsync(IFormFile file)
        {
            if (!this.IsImageFileValid(file))
            {
                return null;
            }

            string url;
            byte[] destinationImage;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            using (var destinationStream = new MemoryStream(destinationImage))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, destinationStream),
                };

                var uploadResult = await this.cloudinary.UploadAsync(uploadParams);

                url = uploadResult.Uri.AbsoluteUri;
            }

            url = url.Replace(GlobalConstants.BaseDeliveryImageUrl, string.Empty);

            return url;
        }
    }
}
