using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;

namespace Entities.Dtos.ProductVariant
{
    public class AddProductVariant
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public List<Entities.Concrete.ProductVariant> ProductVariants { get; set; }
        public List<Entities.Concrete.ProductStock> ProductStocks { get; set; }
        public List<ProductAttribute> ProductAttributes { get; set; }
        public JsonElement JsonData { get; set; }

        [NotMapped]
        public bool IsVariant { get; set; }
    }
}
