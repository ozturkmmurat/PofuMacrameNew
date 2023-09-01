using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Product.Select
{
    public class SelectListProductVariantDto : IDto
    {
        public int ProductId { get; set; }
        public int ProductVariantId { get; set; }
        public int AttributeValueId { get; set; }
        public int? ParentId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string AttributeValue { get; set; }
        public string StockCode { get; set; }  // Pv --> ProductVariant
        public List<string> ProductPaths { get; set; }
        public List<string> VariantPaths { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }
}
