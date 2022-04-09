using WorkTimeManager.Infrastructure.Models.GoogleCloud;
using WorkTimeManager.Infrastructure.Models.GoogleCloud.Base;

namespace WorkTimeManager.Infrastructure.Interfaces
{
    public interface IGoogleDriveCloudService
    {
        Task<bool> StartAsync();
        Task<GoogleCloudFile?> FileAsync(string id);
        Task<IList<GoogleCloudFile>?> FilesAsync();
        Task<Image?> UploadImageAsync(FileStream image);
        Task<bool> DeleteFileAsync(string id);
    }
}
