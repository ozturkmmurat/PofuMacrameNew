using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Entities.Dtos.Product.Select
{
    public class SelectProductDetailDto : IDto
    {
        public int ProductId { get; set; }
        public int ProductVariantId { get; set; }
        public int? ParentId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string StockCode { get; set; }
        public string MainImage { get; set; }
        public List<string> ProductPaths { get; set; }
        public List<string> VariantPaths { get; set; }

    }
}
