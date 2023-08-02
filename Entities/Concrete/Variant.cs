using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Concrete
{
    public class Variant : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string StockCode { get; set; }
        public decimal Price { get; set; }
    }
}
