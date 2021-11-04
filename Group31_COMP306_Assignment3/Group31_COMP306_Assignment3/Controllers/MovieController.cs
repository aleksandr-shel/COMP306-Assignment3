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
        public async Task<IActionResult> List()
        {
            List<S3Object> list = new List<S3Object>();
            ListObjectsV2Request request = new ListObjectsV2Request();
            request.BucketName = bucketName;
            ListObjectsV2Response response;
            do
            {
                response = await s3Client.ListObjectsV2Async(request);

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
            response.S3Objects.ForEach(x => { list.Add(x); });
            
            return View(list);

        }

        public async Task<IActionResult> Page(string key)
        {
            List<Comment> comments = await dBOperations.GetMovieComments(key);
            MoviePageViewModel moviePageViewModel = new MoviePageViewModel(key, userId, comments);
            return View("Page",moviePageViewModel);
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

        [Route("/movie/delete/{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            try
            {
                DeleteObjectRequest deleteRequest = new DeleteObjectRequest()
                {
                    BucketName = bucketName,
                    Key = key
                };
                DeleteObjectResponse response = await s3Client.DeleteObjectAsync(deleteRequest);
            }
            catch (AmazonS3Exception e)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(List));
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
