using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Home_furnishings.Models
{
    public class User:IdentityUser<int>
    {
        [Required]       
            
        public string FullName { get; set; }

        public List<Order> Orders { get; set; }

       
        public List<Cart> Carts { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
