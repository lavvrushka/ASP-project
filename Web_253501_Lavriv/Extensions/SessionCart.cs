using Newtonsoft.Json;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;

namespace WEB_253501_LAVRIV.Extensions
{
    public class SessionCart : Cart
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionCart(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            LoadFromSession(); 
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        public override void AddToCart(Detail detail)
        {
            base.AddToCart(detail);
            SaveToSession();
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            SaveToSession();
        }

        public override void ClearAll()
        {
            base.ClearAll();
            SaveToSession();
        }

        private void SaveToSession()
        {
            // Сериализация CartItems и сохранение в сессию
            var cartData = JsonConvert.SerializeObject(CartItems);
            Session.SetString("Cart", cartData);
        }

        private void LoadFromSession()
        {
            // Десериализация CartItems из сессии
            var cartData = Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cartData))
            {
                var cartItems = JsonConvert.DeserializeObject<Dictionary<int, CartItem>>(cartData);
                if (cartItems != null)
                {
                    CartItems = cartItems;
                }
            }
        }
    }
}
