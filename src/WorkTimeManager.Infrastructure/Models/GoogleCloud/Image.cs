using WorkTimeManager.Infrastructure.Models.GoogleCloud.Base;

namespace WorkTimeManager.Infrastructure.Models.GoogleCloud
{
    public class Image : GoogleCloudFile
    {
        public string WebContentLink { get; set; } = string.Empty;
    }
}
