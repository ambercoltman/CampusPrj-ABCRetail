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

        //uploads a file stream to azure blob storage and returns the blob's uri
        public async Task<string> UploadAsync(Stream fileStream, string fileName)
        {
            var containerClient = _blobServiceCLient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream);
            return blobClient.Uri.ToString();
        }

        //deletes a blob from azure blob storage using its uri
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

////IIEVC School of Computer Science, 2025. CLDV6212. [video online]. Available at: <https://youtube.com/playlist?list=PL480DYS-b_kcZiyuCyHolh6Nad8J_Xnk7&si=W9pUZezCTxlrsDl2> [Accessed 27 August 2025].