using Shared;

namespace PropertyService.Services.ImageService;

public class ImageService (IWebHostEnvironment hostEnvironment) : IImageService
{
    public async Task<ServiceResponse<List<string>>> AddImages(List<IFormFile> images)
    {
        if (images == null || images.Count == 0)
            return new ServiceResponse<List<string>> { Success = false, Message = "No images provided." };

        var saveTasks = images.Select(async image =>
        {
            return await SaveFile(image); // Returns the saved file name
        });

        var fileNames = await Task.WhenAll(saveTasks);

        // You can store the fileNames in DB here, associated with productId if needed

        return new ServiceResponse<List<string>>
        {
            Data = fileNames.ToList(),
            Success = true,
            Message = "Images saved successfully."
        };
    }

    public Task<ServiceResponse<int>> DeleteImage(int id)
    {
        throw new NotImplementedException();
    }
    
    private async Task<string> SaveFile(IFormFile formFile)
    {
        string fileName = new String(Path.GetFileNameWithoutExtension(formFile.FileName).Take(10).ToArray()).Replace(" ", "-");
        fileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(formFile.FileName);
        var filePath = Path.Combine(hostEnvironment.ContentRootPath, "Images", fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await formFile.CopyToAsync(fileStream);
        }

        return fileName;
    }

    private void DeleteFile(string fileName)
    {
        var filePath = Path.Combine(hostEnvironment.ContentRootPath, "Images", fileName);
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);
    }
}