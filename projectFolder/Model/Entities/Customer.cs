using System;
using System.Collections.Generic;
using System.Linq;

namespace petShop_courseWork.Model
{
    /// <summary>
    /// Представляет покупателя с тремя типами средств и корзиной покупок.
    /// </summary>
    public class Customer
    {
        public string Name { get; set; }

        // Баланс наличных средств
        public decimal WalletBalance { get; set; }

        // Баланс на банковской карте
        public decimal CardBalance { get; set; }

        // Баланс бонусных баллов (1 балл = 1 рубль)
        public decimal BonusBalance { get; set; }

        // Список товаров в корзине
        public List<CartItem> ShoppingCart { get; set; } = new List<CartItem>();

        // Подсчёт общей суммы корзины
        public decimal GetCartTotal()
        {
            return ShoppingCart.Sum(item => item.GetTotalPrice());
        }

        // Добавление товара в корзину.
        public void AddToCart(CartItem item)
        {
            ShoppingCart.Add(item);
        }

        // Удаление товара из корзины.
        public void RemoveFromCart(CartItem item)
        {
            ShoppingCart.Remove(item);
        }
    }
}
