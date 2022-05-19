using Microsoft.AspNetCore.Http;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IBlobStorageController
    {
        //Provides an interface for uplaoding / deleting files to / from azure blob storage container 

        string GetUrlByFileName(string fileName);

        void UploadFile(IFormFile formFile, string fileName);

        void DeleteFileIfExists(string fileName);
    }
}
