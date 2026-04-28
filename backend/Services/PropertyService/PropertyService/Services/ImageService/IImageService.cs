using Shared;

namespace PropertyService.Services.ImageService;

public interface IImageService
{
    Task<ServiceResponse<List<string>>> AddImages(List<IFormFile> images);
    Task<ServiceResponse<int>> DeleteImage(int id);
}