using Microsoft.AspNetCore.Mvc;
using ECommerceCase.UI.Models;

public class CartController : Controller
{
    private static List<CartItemViewModel> _cart = new List<CartItemViewModel>();

    public IActionResult Index()
    {
        var totalPrice = _cart.Sum(item => item.Price * item.Quantity);
        ViewBag.TotalPrice = totalPrice;
        return View(_cart);
    }

    [HttpPost]
    public IActionResult AddToCart(string productId)
    {
        // Example logic to add a product to the cart
        var product = new ProductViewModel { ProductId = productId, Name = "Product A", Price = 100, Stock = 10 };
        var cartItem = _cart.FirstOrDefault(item => item.ProductId == productId);

        if (cartItem == null)
        {
            _cart.Add(new CartItemViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1
            });
        }
        else
        {
            cartItem.Quantity++;
        }

        return RedirectToAction("Index");
    }
}