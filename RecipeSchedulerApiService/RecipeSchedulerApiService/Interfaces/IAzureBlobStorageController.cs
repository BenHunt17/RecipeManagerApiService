using Microsoft.AspNetCore.Http;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IAzureBlobStorageController
    {
        //Provides an interface for uplaoding / deleting files to / from azure blob storage container 

        string UploadFile(IFormFile formFile, string fileName);

        void DeleteFileIfExists(string fileName);
    }
}
