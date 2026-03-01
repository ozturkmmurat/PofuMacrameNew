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
        public DateTime RequestedDeliveryStart { get; set; }
        public DateTime RequestedDeliveryEnd { get; set; }
        public List<CartItemParameter> CartItems { get; set; }
        /// <summary>
        /// Misafir sipariş için: Ad Soyad, Email, Telefon, Alıcı Telefonu ve adres bilgileri.
        /// </summary>
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RecipientPhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }
}
