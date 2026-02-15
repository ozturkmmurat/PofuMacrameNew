using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.ProductStock
{
    /// <summary>
    /// Fiyat kontrolü isteği: birden fazla varyant id'si + sipariş için tek ilçe (ProductPriceFactorId).
    /// </summary>
    public class ProductStockPriceCheckDto : IDto
    {
        public List<int> ProductVariantId { get; set; }
        public int ProductPriceFactorId { get; set; }
    }
}
