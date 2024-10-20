using Microsoft.AspNetCore.Mvc;

namespace WEB_253501_LAVRIV.Components
{
    public class CartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartItems = new CartViewModel
            {
                TotalAmount = 100.0,
                ItemCount = 5
            };

            return View("Default", cartItems);
        }
    }

    public class CartViewModel
    {
        public double TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }
}
