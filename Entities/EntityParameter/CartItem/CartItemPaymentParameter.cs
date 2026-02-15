using Core.Entities;

namespace Entities.EntityParameter.CartItem
{
    /// <summary>
    /// Ödeme (TsaPayment) isteğinde frontend sadece bu alanları gönderir; fiyat ve ürün bilgisi backend'den alınır.
    /// </summary>
    public class CartItemPaymentParameter : IDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public int ProductPriceFactorId { get; set; }
    }
}
