using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Controllers
{
    public class S3Upload : BaseController
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.CACentral1;

        public static async Task UploadFileAsync(Stream FileStream, string bucketName, string keyName)
        {
            /*var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);*/

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            using AmazonS3Client s3Client = new AmazonS3Client(credentials, bucketRegion);

            var fileTransferUtility = new TransferUtility(s3Client);
            await fileTransferUtility.UploadAsync(FileStream, bucketName, keyName);
        }
    }
}