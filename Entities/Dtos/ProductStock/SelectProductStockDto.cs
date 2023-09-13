using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.ProductStock
{
    public class SelectProductStockDto : IDto
    {
        public int ProductStockId { get; set; }
        public int ProductId { get; set; }
        public int ProductVariantId { get; set; }
        public int? AttributeId { get; set; }
        public int? AttributeValueId { get; set; }
        public int? ParentId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string StockCode { get; set; }
        public string AttributeValue { get; set; }
    }
}
