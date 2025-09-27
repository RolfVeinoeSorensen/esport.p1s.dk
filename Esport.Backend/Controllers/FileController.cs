using Esport.Backend.Dtos;
using Esport.Backend.Enums;
using Esport.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esport.Backend.Controllers
{
    public class FileController(IFileService fileService) : ControllerBase
    {
        private readonly IFileService fileService = fileService;

        [HttpPost]
        [Route("uploadfile")]
        public async Task<FileResponseDto> SaveFile(FileUploadType uploadType, int entityId, IFormFile file, string title)
        {
            return await fileService.SaveFile(uploadType, entityId, file, title);
        }
        [HttpPost]
        [Route("getfiles")]
        public async Task<IEnumerable<FileResponseDto>> GetFiles(FileUploadType uploadType, int entityId)
        {
            return await fileService.GetFiles(uploadType, entityId);
        }

        [HttpPut]
        [Route("deletefile")]
        public async Task DeleteFile(FileUploadType uploadType, int entityId, int imageId)
        {
            await fileService.DeleteFile(uploadType, entityId, imageId);
        }
    }
}
