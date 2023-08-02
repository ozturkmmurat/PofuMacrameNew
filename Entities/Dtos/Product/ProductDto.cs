using Core.Entities;
using Entities.Concrete;
using Entities.Dtos.Variant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Product
{
    public class ProductDto : IDto
    {
        public int ProductId  { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public List<ProductStock> ProductStocks { get; set; }
        public List<AddVariantDto> AddVariantDtos { get; set; }
    }
}
