using Amazon;
using Amazon.S3;
using Group31_COMP306_Assignment3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Controllers
{
    public class BaseController : Controller
    {
        protected static AmazonS3Client s3Client;
        protected static DynamoDBOperations dBOperations;
        protected static bool signedIn = false;
        protected static string accessKey = "";
        protected static string secretKey = "";
        protected static User loggedUser = null;
        public BaseController()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            accessKey = builder.Build().GetSection("AWSCredentials").GetSection("AccessKeyID").Value;
            secretKey = builder.Build().GetSection("AWSCredentials").GetSection("SecretAccessKey").Value;

            dBOperations = new DynamoDBOperations();
            s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.CACentral1);
        }
    }
}
