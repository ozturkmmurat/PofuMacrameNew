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
        public int? EndProductVariantId { get; set; }
        public int? ParentId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string StockCode { get; set; }  
        public List<AttributeValue> AttributeValues { get; set; }
        public List<string> ProductPaths { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }
}
