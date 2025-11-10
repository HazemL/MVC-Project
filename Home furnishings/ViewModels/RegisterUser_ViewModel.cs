using System.ComponentModel.DataAnnotations;

namespace Home_furnishings.ViewModels
{
    public class RegisterUser_ViewModel
    {

                                 //validation attributes
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        //[DataType(DataType.EmailAddress)]//specifies that the data type is an email address
        [EmailAddress] //checks for valid email format
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
