using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class ProductStock : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string StockCode { get; set; }
    }
}
