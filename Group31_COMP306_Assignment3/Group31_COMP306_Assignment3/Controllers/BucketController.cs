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

        //code to browse through the system and find files
        /*private void BrowseSysem()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();

            if (result != null)
            {
                file = openFileDialog.FileName;

                try
                {
                    string allText = File.ReadAllText(file);
                    FilePathText.Text = file;
                    FilePathText.Text = Path.GetFileName(Path.GetDirectoryName(file));
                }
                catch (Exception exception)
                {
                    //MessageBox.Show($"{exception}");
                }
            }
        }*/

        /*private async void UploadMovie()
        {
            //get file path
            string filePath = FilePathText.Text;

            if (bucketName.Length > 0 && bucketName != null && filePath != null && filePath.Length > 0)
            {
                string key = Path.GetFileName(filePath);

                //code to upload a file (documentation)
                try
                {
                    AccessAppSettingsFile();

                    using AmazonS3Client s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
                    var transferUtility = new TransferUtility(s3Client);

                    await transferUtility.UploadAsync(filePath, bucketName, key);
                }
                catch (Exception exception)
                {
                    //MessageBox.Show($"{exception}");
                }
            }
        }*/

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
