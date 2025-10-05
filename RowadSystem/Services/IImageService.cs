using CloudinaryDotNet.Actions;

namespace RowadSystem.Services;

public interface IImageService
{
    Task<List<(string PublicId, string ImageUrl, string? ThumbnailUrl)>> UploadImageAsync(List<byte[]> file, string folderName = "default");
    Task<DeletionResult> DeleteImageAsync(string imageUrl);
}
