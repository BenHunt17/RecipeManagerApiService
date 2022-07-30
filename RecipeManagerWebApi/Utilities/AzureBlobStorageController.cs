using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RecipeSchedulerApiService.Interfaces;
using System.IO;

namespace RecipeSchedulerApiService.Utilities
{
    public class AzureBlobStorageController : IBlobStorageController
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        public AzureBlobStorageController(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }

        public string GetUrlByFileName(string fileName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_configuration.GetValue<string>("AzureBlobStorage:ContainerName")); 
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            return blobClient.Uri.AbsoluteUri;
        }

        public void UploadFile(IFormFile formFile, string fileName)
        {
            //Uploads a file with a given filename to the azure blob storage. This abstracts away the file null checks from the services to keep them cleaner. Returns the url of the file

            if (formFile == null)
            {
                //Gaurd clause. If file is null then nothing needs to be uploaded
                return;
            }

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_configuration.GetValue<string>("AzureBlobStorage:ContainerName")); //Grabs the image container from remote azure blobl storage
            BlobClient blobClient = containerClient.GetBlobClient(fileName); //Gets a blob client object using new image file name

            using Stream stream = formFile.OpenReadStream();
            blobClient.Upload(stream, true); //Uploads the file to remote azure blob storage
        }

        public void DeleteFileIfExists(string fileName)
        {
            //Delete a file with a given file name from the storage container if it exists. Developer using this controller shouldn't need to care about wehever it exists or not. Just that it might

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_configuration.GetValue<string>("AzureBlobStorage:ContainerName"));

            containerClient.DeleteBlobIfExists(fileName); 
        }
    }
}
