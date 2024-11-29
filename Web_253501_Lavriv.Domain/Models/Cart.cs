using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_253501_Lavriv.Domain.Entities;

namespace Web_253501_Lavriv.Domain.Models
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине: ключ - ID продукта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        /// <summary>
        /// Добавить продукт в корзину
        /// </summary>
        /// <param name="detail">Добавляемый продукт</param>
        public virtual void AddToCart(Detail detail)
        {
            if (CartItems.ContainsKey(detail.Id))
            {
                CartItems[detail.Id].Count++;
            }
            else
            {
                CartItems[detail.Id] = new CartItem { Item = detail, Count = 1 };
            }
        }

        /// <summary>
        /// Удалить продукт из корзины
        /// </summary>
        /// <param name="id">ID удаляемого продукта</param>
        public virtual void RemoveItems(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                CartItems.Remove(id);
            }
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        /// <summary>
        /// Количество продуктов в корзине
        /// </summary>
        public int ItemCount => CartItems.Sum(item => item.Value.Count);

        /// <summary>
        /// Общая стоимость продуктов
        /// </summary>
        public decimal TotalAmount => CartItems.Sum(item => item.Value.Item.Price * item.Value.Count);
    }

}
