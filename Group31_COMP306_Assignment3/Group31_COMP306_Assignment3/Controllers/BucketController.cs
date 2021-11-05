using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Group31_COMP306_Assignment3
{
    public class BucketController : Controller
    {
        private IConfigurationBuilder builder;
        private string accessKeyID;
        private string secretKey;
        private BasicAWSCredentials credentials;
        private string file;
        private string bucketName = "moviescomp306";

        private async void DataGridShowSelections()
        {
            if (bucketName.Length > 0 && bucketName != null)
            {
                AccessAppSettingsFile();

                //boilerplate code to get a list of objects (documentation)
                using AmazonS3Client s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
                var listRequest = new ListObjectsRequest();
                List<Object> objects = new List<Object>();
                listRequest.BucketName = bucketName;
                var listResponse = await s3Client.ListObjectsAsync(listRequest);
            }
        }

        //code from the given ppt presentation to access AppSetting.json
        private void AccessAppSettingsFile()
        {
            try
            {
                builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

                accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccessKeyID").Value;
                secretKey = builder.Build().GetSection("AWSCredentials").GetSection("SecretAccessKey").Value;

                credentials = new BasicAWSCredentials(accessKeyID, secretKey);
            }
            catch (Exception exception)
            {
                //MessageBox.Show($"{exception}");
            }
        }
    }
}
