

using Esport.Backend.Dtos;

namespace Esport.Backend.Services
{
    public interface IFileService
    {
        Task<FileResponseDto> SaveFile(string uploadType, int entityId, IFormFile file, string title);
        Task<IEnumerable<FileResponseDto>> GetFiles(string uploadType, int entityId);
        Task DeleteFile(string uploadType, int entityId, int imageId);
    }
}
