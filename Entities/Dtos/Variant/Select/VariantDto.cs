using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Variant.Select
{
    public class VariantDto : IDto
    {
        //Variant
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ModelCode { get; set; }
        public string StockCode { get; set; }
        public decimal Price { get; set; }

        // Variant Images
        public List<string> ImagePaths { get; set; }
    }
}
