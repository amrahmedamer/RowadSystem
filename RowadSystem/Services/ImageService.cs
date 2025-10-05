using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RowadSystem.Settings;

namespace RowadSystem.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<ImageService> _logger;

    public ImageService(IOptions<CloudinarySettings> options, ILogger<ImageService> logger)
    {
        Account acc = new Account(
           options.Value.CloudName,
           options.Value.ApiKey,
           options.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);
        _logger = logger;
    }

    public async Task<List<(string? PublicId, string ImageUrl, string ThumbnailUrl)>> UploadImageAsync(List<byte[]> files, string folderName = "default")
    {
        var results = new List<(string? PublicId, string ImageUrl, string ThumbnailUrl)>();
        foreach (var file in files)
        {

            if (file == null || file.Length == 0)
                continue;

            using var stream = new MemoryStream(file);
            var fileName = $"{Guid.NewGuid()}.jpg";

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = folderName,
                PublicId = $"{folderName}/{Path.GetFileNameWithoutExtension(fileName)}",
                Transformation = new Transformation()
                    .Width(800)
                    .Height(800)
                    .Crop("fill")
                    .Gravity("auto"),
                Overwrite = true,
                UseFilename = true,
                UniqueFilename = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                _logger.LogError($"Cloudinary upload error: {uploadResult.Error.Message}");

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var imageUrl = uploadResult.SecureUrl.ToString();
                var thumbnailUrl = uploadResult.SecureUrl.ToString().Replace("upload", "upload/c_thumb,g_face,h_200,w_200");
                results.Add((uploadParams.PublicId, imageUrl, thumbnailUrl));
            }
        }
        return results;
    }
    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        var delete = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
        return delete;
    }


}
