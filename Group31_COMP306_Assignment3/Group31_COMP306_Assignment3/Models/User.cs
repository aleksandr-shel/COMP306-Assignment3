using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Group31_COMP306_Assignment3.Models
{
    public partial class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Your Username ")]
        [Display(Name = "UserName")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password ")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
