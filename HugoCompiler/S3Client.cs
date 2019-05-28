using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HugoCompiler
{
    public class S3Client : IDisposable
    {
        private readonly string AWS_S3BucketName;
        private readonly AmazonS3Client client;
        
        public S3Client(string bucketName, string region, string apiKey, string apiSecret)
        {
            AWS_S3BucketName = bucketName;
            client = new AmazonS3Client(
                apiKey, 
                apiSecret, 
                new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(region)
                });
        }

        public async Task<List<S3Object>> GetObjects()
        {
            var request = new ListObjectsRequest { BucketName = AWS_S3BucketName };
            var response = await client.ListObjectsAsync(request);

            return response.S3Objects;
        }

        public async Task DownloadObject(string key, string folder)
        {
            var request = new GetObjectRequest { BucketName = AWS_S3BucketName, Key = key };
            var response = await client.GetObjectAsync(request);

            var filePath = $@"{folder}\{key}";
            

            await response.WriteResponseStreamToFileAsync(filePath, false, CancellationToken.None);
        }

        public async Task UploadObjects(string contentFolder)
        {
            var transfer = new TransferUtility(client);
            await transfer.UploadDirectoryAsync(contentFolder, AWS_S3BucketName, "*", SearchOption.AllDirectories);
        }

        public void Dispose() => client.Dispose();
    }
}
