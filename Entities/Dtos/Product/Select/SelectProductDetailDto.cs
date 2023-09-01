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
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string StockCode { get; set; }
        public List<ProductAttribute> ProductAttributes { get; set; }
        public List<Entities.Concrete.ProductVariant> ProductVariants { get; set; }
        public List<Entities.Concrete.ProductImage> ProductImages { get; set; }

    }
}
