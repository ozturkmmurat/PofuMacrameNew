using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Product.Select
{
    public class SelectListProductDto : IDto
    {
        public int ProductId { get; set; }
        public int ProductVariantId { get; set; }
        public int? ParentId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public bool IsVariant { get; set; }
        public string PvStockCode { get; set; }  // Pv --> ProductVariant
        public List<string> ProductPaths { get; set; }
        public List<string> VariantPaths { get; set; }
        public decimal? Price { get; set; }
    }
}
