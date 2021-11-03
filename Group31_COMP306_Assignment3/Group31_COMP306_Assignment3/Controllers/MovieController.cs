using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
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
    public class MovieController : BaseController
    {
        private string bucketName = "moviescomp306";
        public MovieController()
        {
        }
        public async Task<IActionResult> Index()
        {
            List<S3Object> list = new List<S3Object>();
            ListObjectsV2Request request = new ListObjectsV2Request();
            request.BucketName = "moviescomp306";
            ListObjectsV2Response response;
            do
            {
                response = await s3Client.ListObjectsV2Async(request);

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
            response.S3Objects.ForEach(x => { list.Add(x); });
            
            return View(list);

        }

        public IActionResult UploadMovie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadMovie(FileUploadForm uploadMovie)
        {


            using (var memoryStream = new MemoryStream())
            {
                await uploadMovie.UploadFile.CopyToAsync(memoryStream);

                string key = Path.GetFileNameWithoutExtension(uploadMovie.UploadFile.FileName);

                await S3Upload.UploadFileAsync(memoryStream, bucketName, key);
            }

            return View();
        }

    }
}
