using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class SubVariant : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string StockCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
