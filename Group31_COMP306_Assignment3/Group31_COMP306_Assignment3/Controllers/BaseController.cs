using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Controllers
{
    public class BaseController : Controller
    {
        protected static AmazonS3Client s3Client;
        protected static bool signedIn = false;
        protected static int userId = 0;
        protected static string accessKey = "AKIATVAQEU4Y2MEXAAH2";
        protected static string secretKey = "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";

        public BaseController()
        {
            s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.CACentral1);
        }
    }
}
