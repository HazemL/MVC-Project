using System.ComponentModel.DataAnnotations;

namespace Home_furnishings.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Address { get; set; }

        public string Mobile { get; set; }


        public List<Order> Orders { get; set; }

       
        public List<Cart> Carts { get; set; }
    }
}
