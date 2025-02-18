using Microsoft.AspNetCore.Http;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IStorageService
{
    Task<string> UploadImage(IFormFile? file);

    Task<byte[]?> GetFile(string fileName);

    Task<string> UploadFile(IFormFile? file);
    
    Task<bool> DeleteImage(string avatarUri);
}