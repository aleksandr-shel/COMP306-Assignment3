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
            movieListViewModel.ListOfMoviesObject = response.S3Objects;
            Dictionary<string, double> ratings_dict = new Dictionary<string, double>();
            foreach (var movieObj in movieListViewModel.ListOfMoviesObject)
            {
                List<Rating> ratings = await dBOperations.GetMovieRatings(movieObj.Key);
                double globalRating = 0;
                foreach (var rating in ratings)
                {
                    globalRating += rating.Value;
                }
                if (globalRating == 0)
                {
                    ratings_dict.Add(movieObj.Key, 0);
                }
                else
                {
                    globalRating /= ratings.Count;
                    ratings_dict.Add(movieObj.Key, globalRating);
                }
            }
            movieListViewModel.RatingsDict = ratings_dict;
            List<Movie> movies = await dBOperations.GetAllMovies();
            movieListViewModel.MoviesDict = movies.ToDictionary(x => x.MovieTitle, x => x);
            movieListViewModel.UserId = loggedUser == null ? 0 : loggedUser.Id;

            ViewData["username"] = loggedUser?.Username;
            return View(movieListViewModel);
        }
        [HttpGet]
        public async Task<MovieListViewModel> SortedList(string order)
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
            movieListViewModel.ListOfMoviesObject = response.S3Objects;
            Dictionary<string, double> ratings_dict = new Dictionary<string, double>();
            foreach (var movieObj in movieListViewModel.ListOfMoviesObject)
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
            if (order.Equals("ascending"))
            {
                movieListViewModel.RatingsDict = ratings_dict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            else
            {
                movieListViewModel.RatingsDict = ratings_dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            movieListViewModel.UserId = loggedUser == null ? 0 : loggedUser.Id;

            return movieListViewModel;
        }

        public async Task<IActionResult> Page(string key)
        {
            List<Comment> comments = await dBOperations.GetMovieComments(key);

            var movieObject = await dBOperations.GetMovies(key);

            var ratings = await dBOperations.GetMovieRatings(key);

            //User user = _context.Users.Find(userId);
            string username = loggedUser == null ? "anonim" : loggedUser.Username;
            MoviePageViewModel moviePageViewModel = new MoviePageViewModel(key, username, comments, movieObject[0], ratings);
            moviePageViewModel.UserId = loggedUser == null ? 0 : loggedUser.Id;

            ViewData["username"] = loggedUser?.Username;
            return View("Page", moviePageViewModel);
        }

        public IActionResult UploadMovie()
        {
            ViewData["signedIn"] = signedIn;
            ViewData["username"] = loggedUser?.Username;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadMovie(FileUploadForm uploadMovie)
        {
            ViewData["signedIn"] = signedIn;
            ViewData["username"] = loggedUser?.Username;

            using (var memoryStream = new MemoryStream())
            {
                await uploadMovie.UploadFile.CopyToAsync(memoryStream);

                string key = loggedUser?.Username + " " + Path.GetFileName(uploadMovie.UploadFile.FileName);

                //string movieTitle = Path.GetFileNameWithoutExtension(uploadMovie.UploadFile.FileName);

                string movieExtension = Path.GetExtension(uploadMovie.UploadFile.FileName);

                if (movieExtension == ".mp4")
                {
                    var existingMovie = dBOperations.GetMovie(key, loggedUser.Id);

                    if (existingMovie != null)
                    {
                        await dBOperations.DeleteMovieComments(key);
                        await dBOperations.DeleteMovieRatings(key);
                    }

                    await S3Upload.UploadFileAsync(memoryStream, bucketName, key);

                    await dBOperations.CreateMovieDescription(key, loggedUser.Id, uploadMovie.Description, uploadMovie.Director);

                    return RedirectToAction("UploadMovie", "Movie");
                }
            }

            return RedirectToAction("UploadMovie", "Movie");
        }


        [HttpPost]
        public async Task<IActionResult> EditMovie(string movieTitle, int userId, string description, string director)
        {
            if (loggedUser?.Id == userId)
                await dBOperations.CreateMovieDescription(movieTitle, userId, description, director);

            ViewData["username"] = loggedUser?.Username;
            return RedirectToAction("Page", "Movie", new { key = movieTitle });
        }


        public async Task<IActionResult> Delete(string key, int userid)
        {
            if (userid != 0)
            {
                Movie movie = await dBOperations.GetMovie(key, userid);
                if (loggedUser?.Id == movie?.UserId)
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
                    await dBOperations.DeleteMovie(key, movie.UserId);
                    await dBOperations.DeleteMovieComments(key);
                    await dBOperations.DeleteMovieRatings(key);

                }
            }
            ViewData["username"] = loggedUser?.Username;
            return RedirectToAction(nameof(List));
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
