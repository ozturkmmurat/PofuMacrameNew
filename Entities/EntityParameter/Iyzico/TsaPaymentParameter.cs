using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Entities.EntityParameter.CartItem;

namespace Entities.EntityParameter.Iyzico
{
    public class TsaPaymentParameter : IDto
    {
        public int AddressId { get; set; }
        /// <summary>
        /// Sipariş adresi için tek ilçe (ProductPriceFactor). Tüm ürünler aynı adrese gideceği için sipariş başına bir kez uygulanır.
        /// </summary>
        public int ProductPriceFactorId { get; set; }
        public string TcNo { get; set; }
        public string OrderDescription { get; set; }
        public List<CartItemParameter> CartItems { get; set; }
    }
}
