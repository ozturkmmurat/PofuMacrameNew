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
        public int Quantity{ get; set; }

        //CategoryAttribute
        public bool Slicer { get; set; }
        public bool Attribute { get; set; }

        //Attribute
        public string AttributeKey { get; set; }

        // Variant Images
        public List<string> Paths { get; set; }

        //ProductATTRİBUTE
        public int ProductAttributeId { get; set; }
        public List<string> ProductAttributeValue { get; set; }


        public Entities.Concrete.ProductVariant Variant { get; set; }
        public Entities.Concrete.Product Product { get; set; }
        public Entities.Concrete.ProductStock ProductStock { get; set; }
        public Entities.Concrete.CategoryAttribute CategoryAttribute { get; set; }
        public Entities.Concrete.Attribute EAttribute { get; set; }
        public Entities.Concrete.ProductImage ProductAttributeImage { get; set; }
        public Entities.Concrete.ProductAttribute ProductAttribute { get; set; }


    }
}
