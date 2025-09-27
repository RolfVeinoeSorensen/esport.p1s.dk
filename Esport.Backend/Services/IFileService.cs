

using Esport.Backend.Dtos;
using Esport.Backend.Enums;

namespace Esport.Backend.Services
{
    public interface IFileService
    {
        Task<FileResponseDto> SaveFile(FileUploadType uploadType, int entityId, IFormFile file, string title);
        Task<IEnumerable<FileResponseDto>> GetFiles(FileUploadType uploadType, int entityId);
        Task DeleteFile(FileUploadType uploadType, int entityId, int imageId);
    }
}
