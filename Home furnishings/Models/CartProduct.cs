using System.ComponentModel.DataAnnotations.Schema;

namespace Home_furnishings.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public Cart Cart { get; set; }

    }
}
