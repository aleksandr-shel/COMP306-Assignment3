using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Group31_COMP306_Assignment3.Models
{
    public class FileUploadForm
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile UploadFile { get; set; }
    }
}