using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Product.Select
{
    public class SelectProductDto : IDto
    {
        // Urunleri listelerken bu dto kullanılıyor
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int VariantId { get; set; }
        public int ProductAttributeId { get; set; }
        public int ProductStockId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public List<string> Paths { get; set; }
        public decimal Price { get; set; }
        public string StockCode { get; set; }
        public int Quantity { get; set; }
    }
}
