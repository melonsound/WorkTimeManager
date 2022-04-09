using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using System.Net.Mime;
using WorkTimeManager.Infrastructure.Interfaces;
using WorkTimeManager.Infrastructure.Models.GoogleCloud;
using WorkTimeManager.Infrastructure.Models.GoogleCloud.Base;

namespace WorkTimeManager.Infrastructure.Services
{
    public class GoogleDriveCloudService : IGoogleDriveCloudService
    {
        public string ApplicationName { get; private set; } = string.Empty;
        public string FolderId { get; private set; } = string.Empty;
        public string[] Scopes { get; set; } = { DriveService.Scope.Drive };
        public string[] MimeTypes { get; set; } = Array.Empty<string>();
        public UserCredential? UserCredential { get; private set; }
        public DriveService? DriveService { get; private set; }

        private string _credentialsPath;

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentialsPath">path to client secret JSON file</param>
        /// <param name="applicationName"></param>
        /// <param name="folderId"></param>
        /// <exception cref="ArgumentException"></exception>
        public GoogleDriveCloudService(string credentialsPath,
                                       string applicationName,
                                       string folderId)
        {
            if (string.IsNullOrWhiteSpace(credentialsPath))
                throw new ArgumentException($"'{nameof(credentialsPath)}' cannot be null or whitespace.", nameof(credentialsPath));

            if (string.IsNullOrWhiteSpace(applicationName))
                throw new ArgumentException($"'{nameof(applicationName)}' cannot be null or whitespace.", nameof(applicationName));

            if (string.IsNullOrWhiteSpace(folderId))
                throw new ArgumentException($"'{nameof(folderId)}' cannot be null or whitespace.", nameof(folderId));

            _credentialsPath = credentialsPath;
            ApplicationName = applicationName;
            FolderId = folderId;
        }
        #endregion

        public async Task<bool> StartAsync()
        {
            var credentialsFolder = "Credentials";

            if (!File.Exists(_credentialsPath))
                throw new FileNotFoundException($"'{nameof(_credentialsPath)}' cannot find file.");

            var credentialsPath = Path.Combine(Environment.CurrentDirectory,
                                               credentialsFolder,
                                               _credentialsPath);

            try
            {
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    var rootPath = Environment.CurrentDirectory;
                    var generatedCredentialPath = Path.Combine(rootPath, credentialsFolder, "google-drive-credentials");
                    UserCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                         Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(generatedCredentialPath, true)
                        ).Result;
                }

                var driveService = new DriveService(new Google.Apis.Services.BaseClientService.Initializer
                {
                    HttpClientInitializer = UserCredential,
                    ApplicationName = ApplicationName
                });

                DriveService = driveService;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region CRUD Functions
        public Task<bool> DeleteFileAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<GoogleCloudFile?> FileAsync(string id)
        {
            var request = DriveService.Files.List();
            request.Fields = "nextPageToken, files(id, name, mimeType)";
            request.Q = $"parents in '{FolderId}'";

            var result = await request.ExecuteAsync();
            if (result == null)
                return null;

            var file = result.Files.SingleOrDefault(x => x.Id == id);
            if (file == null)
                return null;

            var googleCloudFile = new GoogleCloudFile
            {
                Id = file.Id,
                Name = file.Name,
                MimeType = file.MimeType
            };

            return googleCloudFile;
        }

        public async Task<IList<GoogleCloudFile>?> FilesAsync()
        {
            var request = DriveService.Files.List();
            request.Fields = "nextPageToken, files(id, name, mimeType)";
            request.Q = $"parents in '{FolderId}'";

            var result = await request.ExecuteAsync();
            if (result == null)
                return null;

            IList<GoogleCloudFile> googleCloudFiles = new List<GoogleCloudFile>();
            googleCloudFiles = result.Files.Select(
                x => new GoogleCloudFile 
                {
                    Id = x.Id, 
                    MimeType = x.MimeType, 
                    Name = x.Name 
                }).ToList();

            return googleCloudFiles;
        }

        public async Task<Image?> UploadImageAsync(FileStream image)
        {
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            var fileMimeType = FileMimeType(image);

            driveFile.Name = Path.GetRandomFileName();
            driveFile.Description = "";
            driveFile.MimeType = fileMimeType;
            driveFile.Parents = new string[] { FolderId };

            var request = DriveService.Files.Create(driveFile, image, fileMimeType);
            request.Fields = "id";

            var response = await request.UploadAsync();
            var responseImage = new Image
            {
                Id = request.ResponseBody.Id,
                MimeType = request.ResponseBody.MimeType,
                Name = request.ResponseBody.Name,
                WebContentLink = request.ResponseBody.WebContentLink
            };

            return responseImage;
        }
        #endregion

        #region Helpers

        private string FileMimeType(FileStream file)
        {
            var fileExtension = Path.GetExtension(file.Name);
            switch (fileExtension)
            {
                case "jpg":
                    return MediaTypeNames.Image.Jpeg;

                case "jpeg":
                    return MediaTypeNames.Image.Jpeg;

                //case "png":
                //    return MediaTypeNames.Image.
            }
            return String.Empty;
        }
        #endregion
    }
}
