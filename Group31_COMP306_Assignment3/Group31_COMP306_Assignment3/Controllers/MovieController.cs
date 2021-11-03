using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Controllers
{
    public class MovieController : BaseController
    {
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
    }
}
