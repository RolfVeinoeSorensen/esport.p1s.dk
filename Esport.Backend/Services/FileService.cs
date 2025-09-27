using Esport.Backend.Dtos;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Esport.Backend.Services
{
    public class FileService(
        DataContext context) : IFileService
    {
        private readonly DataContext db = context;
        private const string basePath = "/var/opt/uploads/";
        private readonly List<int> sizes =
        [
            1000,
            500,
            200
        ];
        public async Task DeleteFile(FileUploadType uploadType, int entityId, int imageId)
        {
            string fileName = string.Empty;
            if (uploadType == FileUploadType.News)
            {
                var item = await db.News.SingleOrDefaultAsync(x=>x.ImageId.Equals(imageId) && x.Id.Equals(entityId));
                if (item == null) return;
                var image = await db.Files.FirstOrDefaultAsync(x => x.Id.Equals(imageId));
                if (image == null) return;
                item.ImageId = null;
                db.Update(item);
                db.Remove(image);
                
            }
            else if (uploadType == FileUploadType.User)
            {
                var item = await db.AuthUsers.SingleOrDefaultAsync(x => x.ImageId.Equals(imageId) && x.Id.Equals(entityId));
                if (item == null) return;
                var image = await db.Files.FirstOrDefaultAsync(x => x.Id.Equals(imageId));
                if (image == null) return;
                item.ImageId = null;
                db.Update(item);
                db.Remove(image);

            } else if (uploadType == FileUploadType.Gallery)
            {
                var item = await db.Galleries.Include(x=>x.Images).SingleOrDefaultAsync(x => x.Images.Equals(imageId) && x.Id.Equals(entityId));
                if (item == null) return;
                var image = await db.Files.FirstOrDefaultAsync(x => x.Id.Equals(imageId));
                if (image == null) return;
                item.Images.Remove(image);
                db.Remove(image);

            }
            else
            {
                return;
            }
            await db.SaveChangesAsync();
            var uploadFolder = $@"{basePath}{uploadType}/{entityId}/";
            var fileNameWithPath = Path.Combine(uploadFolder, fileName);
            var fileInfo = new FileInfo(fileNameWithPath);
            foreach (var size in sizes)
            {
                System.IO.File.Delete(Path.Combine(uploadFolder, "thumbnail-" + size.ToString() + "-" + fileName));
            }

            System.IO.File.Delete(fileNameWithPath);

            return;            
        }

        public async Task<IEnumerable<FileResponseDto>> GetFiles(FileUploadType uploadType, int entityId)
        {
            var res = new List<FileResponseDto>();
            if(uploadType == FileUploadType.News)
            {
                foreach (var item in await db.Files.Include(f => f.News).Where(n => n.News != null).ToListAsync())
                {
                    var f = new FileResponseDto
                    {
                        FileName = item.Filename,
                        FileState = FileState.Success,
                    };
                    res.Add(f);
                }
            }
            else if (uploadType == FileUploadType.User)
            {
                foreach (var item in await db.Files.Include(f => f.AuthUsers).Where(n => n.AuthUsers != null).ToListAsync())
                {
                    var f = new FileResponseDto
                    {
                        FileName = item.Filename,
                        FileState = FileState.Success,
                    };
                    res.Add(f);
                }
            }
            else if (uploadType == FileUploadType.User)
            {
                foreach (var item in await db.Galleries.Include(f => f.Images).Where(n => n.Images != null).Select(i => i.Images).ToListAsync())
                {
                    foreach(var img in item.ToList())
                    {
                        var f = new FileResponseDto
                        {
                            FileName = img.Filename,
                            FileState = FileState.Success,
                        };
                        res.Add(f);
                    }

                }
            }
            return res;
        }

        public async Task<FileResponseDto> SaveFile(FileUploadType uploadType, int entityId, IFormFile file, string title)
        {
            var result = new FileResponseDto { FileName = file.FileName.Replace(" ", "-") , FileState = FileState.Failed};
            result.FileName = file.FileName.Replace(" ", "-");
            if (file.Length <= 0)
            {
                result.FileState = FileState.Failed;
                result.ErrorMessage = "File size was 0. Nothing uploaded";
                return result;
            }

            //Save file here
            var uploadFolder = $"{basePath}{uploadType}/{entityId}/";
            var directory = new DirectoryInfo(uploadFolder);
            if (!directory.Exists)
            {
                directory.Create();
            }
            var fileNameWithPath = Path.Combine(uploadFolder, result.FileName);
            var fileInfo = new FileInfo(fileNameWithPath);
            var exist = fileInfo.Exists;
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            await CreateThumbnails(uploadType, entityId, result.FileName);
            if (exist) //allow overwrite file but do not alter entries
            {

                result.FileState = FileState.Success;
                return result;
            }

            var image = new Entities.File
            {
                Filename = result.FileName,
                Title = title,
                Type = FileType.Image
            };

            db.Add(image);
            await db.SaveChangesAsync();

            //Save in DB
            if (uploadType == FileUploadType.News)
            {
                var news = db.News.FirstOrDefault(x => x.Id == entityId);
                if (news != null) {
                    news.ImageId = image.Id;
                    db.Update(news);
                }
            }          
            await db.SaveChangesAsync();

            result.FileState = FileState.Success;
            return result;
        }

        private async Task CreateThumbnails(FileUploadType uploadType, int entityId, string imageName)
        {
            foreach(var size in sizes)
            {
                await Task.Run(() => CreateThumbnail(uploadType, entityId, imageName, size));
            }            
        }


        private static void CreateThumbnail(FileUploadType uploadType, int entityId, string imageName, int size)
        {
            var uploadFolder = $"{basePath}{uploadType}/{entityId}/";
            var inStream = uploadFolder + imageName;
            var outStream = uploadFolder + "thumbnail-" + size.ToString() + "-" + imageName;


            using Image image = Image.Load(inStream);
            // Resize the given image in place and return it for chaining.
            // 'x' signifies the current image processing context.
            using Image clone = image.Clone(x => x.Resize(new ResizeOptions
            {
                Size = new Size(int.MaxValue, size),
                Mode = ResizeMode.Max,
                Sampler = KnownResamplers.Lanczos3,
            }));

            if (Path.GetExtension(inStream) == ".png")
            {
                PngMetadata s = clone.Metadata.GetPngMetadata();
                PngMetadata m = clone.Metadata.GetPngMetadata();
                m.ColorType = PngColorType.RgbWithAlpha;
                m.TransparentColor = s.TransparentColor;
                image.Save(outStream, new PngEncoder
                {
                    BitDepth = s.BitDepth,
                    TransparentColorMode = PngTransparentColorMode.Preserve
                });
            } else
            {
                image.Save(outStream, new JpegEncoder
                {
                    Quality = 75,
                    ColorType = JpegEncodingColor.YCbCrRatio444
                });
            }


            // Dispose - releasing memory into a memory pool ready for the next image you wish to process.
            //return outStream;
        }
    }
}
