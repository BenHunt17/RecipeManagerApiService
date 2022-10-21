using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RecipeManagerWebApi.Interfaces;
using System;
using System.IO;

namespace RecipeManagerWebApi.Utilities
{
    public class AzureBlobStorageController : IBlobStorageController
    {
        private readonly ILogger<AzureBlobStorageController> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        public AzureBlobStorageController(ILogger<AzureBlobStorageController> logger, BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }

        public string UploadFile(IFormFile formFile, string fileName)
        {
            //Uploads a file with a given filename to the azure blob storage. This abstracts away the file null checks from the services to keep them cleaner. Returns the url of the file

            if (formFile == null)
            {
                _logger.LogInformation("No image file needing to be uploaded");
                return null;
            }

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_configuration.GetValue<string>("AzureBlobStorage:ContainerName")); //Grabs the image container from remote azure blobl storage
            
            if (containerClient == null)
            {
                _logger.LogWarning("The azure blob container client was not found");
                return null;
            }

            BlobClient blobClient = containerClient.GetBlobClient(fileName); //Gets a blob client object using new image file name

            using Stream stream = formFile.OpenReadStream();

            try
            {
                blobClient.Upload(stream, true); //Uploads the file to remote azure blob storage
                return blobClient.Uri.AbsoluteUri;
            }
            catch (RequestFailedException exception)
            {
                //It's bad practise to catch all exceptions but failing here shouldn't crash the application and should return null instead
                _logger.LogWarning(exception.Message);
                return null; 
            }
        }

        public void DeleteFileIfExists(string uri)
        {
            //Delete a file with a given file name from the storage container if it exists. Developer using this controller shouldn't need to care about if it exists or not. Just that it might

            if (uri == null)
            {
                _logger.LogInformation("No image url given");
                return;
            }

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_configuration.GetValue<string>("AzureBlobStorage:ContainerName"));

            if(containerClient == null)
            {
                _logger.LogWarning("The azure blob container client was not found");
                return;
            }

            try
            {
                string localPath = new Uri(uri).LocalPath;
                string blobClientName = localPath.Split("/", StringSplitOptions.RemoveEmptyEntries)[0];

                containerClient.DeleteBlobIfExists(blobClientName);
                return;
            }
            catch (RequestFailedException exception)
            {
                _logger.LogWarning(exception.Message);
                return;
            }
            catch (UriFormatException exception)
            {
                _logger.LogWarning(exception.Message);
                return;
            }
        }
    }
}
