using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Group31_COMP306_Assignment3.Models
{
    public class FileUploadForm
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile UploadFile { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Director")]
        public string Director { get; set; }
    }
}