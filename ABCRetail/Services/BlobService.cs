using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ABCRetail.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceCLient;
        private readonly string _containerName = "products";

        public BlobService(string connectionString)
        {
            _blobServiceCLient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadAsync(Stream fileStream, string Name)
        {
            var containerClient = _blobServiceCLient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(Name);
            await blobClient.UploadAsync(fileStream);
            return blobClient.Uri.ToString();
        }

        public async Task DeleteBlobAsync(string blobUri)
        {
            Uri u = new Uri(blobUri);
            string blobName = u.Segments[^1];
            var containerClient = _blobServiceCLient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
    }
}
