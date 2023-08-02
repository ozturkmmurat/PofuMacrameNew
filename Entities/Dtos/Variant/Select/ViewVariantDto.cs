using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Variant.Select
{
    public class ViewVariantDto : IDto
    {
        //Variant
        public int VariantId { get; set; }
        public string StockCode { get; set; }

        //Product 
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }

        //Product Stock
        public decimal Price { get; set; }

        // Variant Images
        public List<string> Paths { get; set; }

        //ProductATTRİBUTE
        public int ProductAttributeId { get; set; }
    }
}
