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
        public int FirstProductVariantId { get; set; }
        public int EndProductVariantId { get; set; }
        public int? ParentId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Kdv { get; set; }
        public decimal NetPrice { get; set; }
        public string StockCode { get; set; }
        public string AttributeValue { get; set; }
    }
}
