namespace Imdb.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        bool IsImageFileValid(IFormFile imageFile);

        Task<string> UploudImageAsync(IFormFile file);
    }
}
