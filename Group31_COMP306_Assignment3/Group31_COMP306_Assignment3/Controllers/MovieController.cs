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
        private COMP306LAB3Context _context;
        public MovieController(COMP306LAB3Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> List()
        {
            MovieListViewModel movieListViewModel = new MovieListViewModel();
            ListObjectsV2Request request = new ListObjectsV2Request();
            request.BucketName = bucketName;
            ListObjectsV2Response response;
            do
            {
                response = await s3Client.ListObjectsV2Async(request);

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
            movieListViewModel.ListOfMovies = response.S3Objects;
            Dictionary<string, double> ratings_dict = new Dictionary<string, double>();
            foreach (var movieObj in movieListViewModel.ListOfMovies)
            {
                List<Rating> ratings = await dBOperations.GetMovieRatings(movieObj.Key);
                double globalRating = 0;
                foreach (var rating in ratings)
                {
                    globalRating += rating.Value;
                }
                globalRating /= ratings.Count;
                ratings_dict.Add(movieObj.Key, globalRating);
            }
            movieListViewModel.RatingsDict = ratings_dict;

            return View(movieListViewModel);

        }

        public async Task<IActionResult> Page(string key)
        {
            List<Comment> comments = await dBOperations.GetMovieComments(key);

            var movieObject = await dBOperations.GetMovie(key);

            var ratings = await dBOperations.GetMovieRatings(key);

            //User user = _context.Users.Find(userId);
            string username = loggedUser == null ? "anonim" : loggedUser.Username;
            MoviePageViewModel moviePageViewModel = new MoviePageViewModel(key, username, comments, movieObject[0], ratings);
            moviePageViewModel.UserId = loggedUser == null ? 0 : loggedUser.Id;


            return View("Page", moviePageViewModel);
        }

        public IActionResult UploadMovie()
        {
            ViewData["signedIn"] = signedIn;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadMovie(FileUploadForm uploadMovie)
        {
            ViewData["signedIn"] = signedIn;

            using (var memoryStream = new MemoryStream())
            {
                await uploadMovie.UploadFile.CopyToAsync(memoryStream);

                string key = Path.GetFileName(uploadMovie.UploadFile.FileName);

                //string movieTitle = Path.GetFileNameWithoutExtension(uploadMovie.UploadFile.FileName);

                await S3Upload.UploadFileAsync(memoryStream, bucketName, key);

                await dBOperations.CreateMovieDescription(key, loggedUser.Id, uploadMovie.Description, uploadMovie.Director);
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> EditMovie(string movieTitle, int userId, string description, string director)
        {
            if(loggedUser?.Id == userId)
                await dBOperations.CreateMovieDescription(movieTitle, userId, description, director);

            return RedirectToAction("Page", "Movie", new { key = movieTitle });
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
