using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class VariantDto : IDto
    {
        //Variant
        public int VariantId { get; set; }
        public string VariantTitle { get; set; }


        //Product
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        //ProductStock
        public int ProductStockId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string StockCode { get; set; }

        //Attribute
        public string AttrName { get; set; }

        //AttributeValue
        public string AttrValue { get; set; }
        public string AttrCodeSelect { get; set; }
        public List<string> AttrCode { get; set; }

        //User
        public int UserId { get; set; }
    }
}
