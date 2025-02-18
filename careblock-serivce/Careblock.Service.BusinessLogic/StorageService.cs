using System.Net;
using Azure.Storage.Blobs;
using Careblock.Model.Shared.Common;
using Careblock.Service.BusinessLogic.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Careblock.Service.BusinessLogic;

public class StorageService : IStorageService
{
    private readonly Cloudinary _cloudinary;
    private readonly BlobServiceClient _blobServiceClient;
    private string _azureConnectionStrings = string.Empty;
    
    public StorageService(IConfiguration configuration)
    {
        _cloudinary = new Cloudinary(new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]));
        _azureConnectionStrings = configuration["AzureConnectionStrings"];
        _blobServiceClient = new BlobServiceClient(configuration["AzureConnectionStrings"]);
    }
    
    public async Task<string> UploadImage(IFormFile? file)
    {
        try
        {
            if (file == null || file.Length == 0) return string.Empty;
            
            await using var stream = file.OpenReadStream();
            var publicId = Guid.NewGuid().ToString();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName + "_" + publicId, stream),
                Folder = Constants.AccountAvatarFolder
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode == HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.AbsoluteUri;
            }
            return string.Empty;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return String.Empty;
        }
    }

    public async Task<byte[]?> GetFile(string fileName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(Constants.AzureFileContainerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        if (await blobClient.ExistsAsync())
        {
            var content = await blobClient.DownloadContentAsync();
            return content.Value.Content.ToArray();
        }
        else
        {
            return null;
        }
    }

    public async Task<string> UploadFile(IFormFile? file)
    {
        try
        {
            if (file == null || file.Length == 0) return string.Empty;

            var blobClient = new BlobClient(_azureConnectionStrings, Constants.AzureFileContainerName, file.FileName);
            using (var stream = file.OpenReadStream())
            {
                var res = await blobClient.UploadAsync(stream, true);
                
                if (res.GetRawResponse().Status == (int)HttpStatusCode.Created)
                {
                    return blobClient.Uri.ToString();
                }
            }

            return string.Empty;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return string.Empty;
        }
    }

    public async Task<bool> DeleteImage(string avatarUri)
    {
        try
        {
            if (string.IsNullOrEmpty(avatarUri)) return false;

            var publicId = avatarUri.Split("_").Last();
            var deletionResult = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            if (deletionResult.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}