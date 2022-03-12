using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Models
{
    public class SignUpDTO
    {

        [Required(ErrorMessage = "Please Enter First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Please Enter Your Email")]
        [Display(Name ="Email Address")]
        [EmailAddress(ErrorMessage ="Please Enter a Valid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Please Enter Strong Password")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="Please Confirm your Password")]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
