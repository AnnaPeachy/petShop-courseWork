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
        /// <summary>
        /// Баланс наличных средств.
        /// </summary>
        public decimal WalletBalance { get; set; }

        /// <summary>
        /// Баланс на банковской карте.
        /// </summary>
        public decimal CardBalance { get; set; }

        /// <summary>
        /// Баланс бонусных баллов (1 балл = 1 рубль).
        /// </summary>
        public decimal BonusBalance { get; set; }

        private List<CartItem> _shoppingCart = new List<CartItem>();

        /// <summary>
        /// Список товаров в корзине.
        /// </summary>
        public List<CartItem> ShoppingCart
        {
            get { return _shoppingCart; }
        }

        /// <summary>
        /// Подсчёт общей суммы корзины.
        /// </summary>
        public decimal GetCartTotal()
        {
            return ShoppingCart.Sum(item => item.GetTotalPrice());
        }

        /// <summary>
        /// Добавление товара в корзину.
        /// </summary>
        public void AddToCart(CartItem item)
        {
            ShoppingCart.Add(item);
        }

        /// <summary>
        /// Удаление товара из корзины.
        /// </summary>
        public void RemoveFromCart(CartItem item)
        {
            ShoppingCart.Remove(item);
        }
    }
}
