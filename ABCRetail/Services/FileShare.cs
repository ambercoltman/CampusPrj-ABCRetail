using ABCRetail.Models;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Files.Shares;
using Azure;

namespace ABCRetail.Services
{
    public class FileShare
    {
        private readonly string _connectionString;
        private readonly string _fileShareName;

        //initializes fileshare with connection string and file share name
       public FileShare(string coonnectionString, string  fileShareName)
        {
            _connectionString = coonnectionString??
                throw new ArgumentNullException(nameof(coonnectionString));
            _fileShareName = fileShareName??
                throw new ArgumentNullException(nameof(_fileShareName));
        }

        //uploads a file stream to a directory in azure files
       public async Task UploadFileAsync(string directoryName, string fileName, Stream fileStream)
        {
            try
            {
                var serviceClient = new ShareServiceClient(_connectionString);
                var shareClient = serviceClient.GetShareClient(_fileShareName);

                var directoryClient = shareClient.GetDirectoryClient(directoryName);
                await directoryClient.CreateIfNotExistsAsync();

                var fileClient = directoryClient.GetFileClient(fileName);

                await fileClient.CreateAsync(fileStream.Length);
                await fileClient.UploadRangeAsync(new HttpRange(0, fileStream.Length), fileStream);
            }
            catch (Exception ex)
            {
                throw new Exception("THere has been an error uploading the file: " + ex.Message, ex);
            }
        }

        //downloads the file from the directory in azure files
        public async Task<Stream> DownloadFileAsync(string directoryName, string fileName)
        {
            try
            {
                var serviceClient = new ShareServiceClient(_connectionString);
                var shareClient = serviceClient.GetShareClient(_fileShareName);
                var directoryClient = shareClient.GetDirectoryClient(directoryName);
                var fileClient = directoryClient.GetFileClient(fileName);
                var downloadInfo = await fileClient.DownloadAsync();
                return downloadInfo.Value.Content;
            }
            catch (Exception ex)
            {
                throw new Exception("There has been an error downloading file: " + ex.Message, ex);
            }

        }

        //lists all files and lists their details
        public async Task<List<FileModel>> ListFilesAsync(string directoryName)
        {
            var files = new List<FileModel>();
            try
            {
                var serviceClient = new ShareServiceClient(_connectionString);
                var shareClient = serviceClient.GetShareClient(_fileShareName);

                var directoryClient = shareClient.GetDirectoryClient(directoryName);

                await foreach (ShareFileItem item in directoryClient.GetFilesAndDirectoriesAsync())
                {
                    if(!item.IsDirectory)
                    {
                        var fileClient = directoryClient.GetFileClient(item.Name);
                        var properties = await fileClient.GetPropertiesAsync();
                        files.Add(new FileModel
                        {
                            Name = item.Name,
                            Size = properties.Value.ContentLength,
                            LastModified = properties.Value.LastModified,
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("There has been an error lsiting the files: " + ex.Message, ex);
            }
            return files;
        }
    }
}

////IIEVC School of Computer Science, 2025. CLDV6212. [video online]. Available at: <https://youtube.com/playlist?list=PL480DYS-b_kcZiyuCyHolh6Nad8J_Xnk7&si=W9pUZezCTxlrsDl2> [Accessed 28 August 2025].