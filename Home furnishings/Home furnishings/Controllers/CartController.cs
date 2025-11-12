// Controllers/CartController.cs
using Home_furnishings.Models;
using Home_furnishings.Repository;
using Home_furnishings.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Home_furnishings.Controllers
{
  //  [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            UserManager<ApplicationUser> userManager)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userManager = userManager;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _cartRepository.GetCartWithProducts(user.Id);

            if (cart == null  || cart.Products == null|| !cart.Products.Any())
            {
                return View(new CartViewModel
                {
                    Items = new List<CartItemViewModel>(),
                    TotalPrice = 0,
                    TotalItems = 0
                });
            }

            var viewModel = new CartViewModel
            {
                CartId = cart.CartId,
                Items = cart.Products.Select(p => new CartItemViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Quantity = 1, // Since your model doesn't track quantity, default to 1
                    IsInStock = p.IsActive && p.Quantity > 0,
                    AvailableStock = p.Quantity
                }).ToList(),


                TotalItems = cart.Products.Count,
                TotalPrice = cart.Products.Sum(p => p.Price)
            };

            return View(viewModel);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data" });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Please login first", redirectToLogin = true });
            }

            var product = _productRepository.GetById(model.ProductId);
            if (product == null || !product.IsActive || product.Quantity < 1)
            {
                return Json(new { success = false, message = "Product not available" });
            }

            var cart = _cartRepository.GetCartByUserId(user.Id);

            // Check if product already in cart
            if (cart.Products.Any(p => p.ProductId == model.ProductId))
            {
                return Json(new { success = false, message = "Product already in cart" });
            }

            _cartRepository.AddProductToCart(cart.CartId, model.ProductId);

            int cartCount = _cartRepository.GetCartItemCount(user.Id);
            return Json(new { success = true, message = "Product added to cart", cartCount });
        }

        // POST: Cart/Remove
        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Please login first" });
            }

            var cart = _cartRepository.GetCartByUserId(user.Id);
            _cartRepository.RemoveProductFromCart(cart.CartId, productId);

            int cartCount = _cartRepository.GetCartItemCount(user.Id);
            var updatedCart = _cartRepository.GetCartWithProducts(user.Id);
            float totalPrice = updatedCart?.Products.Sum(p => p.Price) ?? 0;

            return Json(new { success = true, cartCount, totalPrice });
        }

        // POST: Cart/Clear
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Please login first" });
            }

            _cartRepository.ClearCart(user.Id);

            return Json(new { success = true });
        }

        // GET: Cart/GetCartCount
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { count = 0 });
            }

            int count = _cartRepository.GetCartItemCount(user.Id);
            return Json(new { count });
        }
    }
}