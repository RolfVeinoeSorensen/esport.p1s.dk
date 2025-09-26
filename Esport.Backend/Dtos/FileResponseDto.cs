using Esport.Backend.Enums;

namespace Esport.Backend.Dtos
{
    public class FileResponseDto
    {
        public required string FileName { get; set; }
        public FileState FileState { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
