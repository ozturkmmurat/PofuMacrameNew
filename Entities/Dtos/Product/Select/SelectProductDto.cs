using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Product.Select
{
    public class SelectProductDto : IDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int? VariantId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
