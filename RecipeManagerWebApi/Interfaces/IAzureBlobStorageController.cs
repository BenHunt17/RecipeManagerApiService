using Microsoft.AspNetCore.Http;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IBlobStorageController
    {
        string UploadFile(IFormFile formFile, string fileName);

        void DeleteFileIfExists(string fileName);
    }
}
