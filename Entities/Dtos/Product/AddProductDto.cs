using Core.Entities;
using Entities.Concrete;
using Entities.Dtos.Variant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Product
{
    public class AddProductDto : IDto
    {
        //Bu Class Product ve Variantlarını birlikte oluşturmak için kullanılıyor
        public int ProductId  { get; set; }
        public int ProductStockId { get; set; }
        public int CategoryId { get; set; }
        public int EntityTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public IFormFile File { get; set; }
        public List<ProductStock> ProductStocks { get; set; }
        public List<AddVariantDto> AddVariantDtos { get; set; }
        public List<string> AttrCode { get; set; } 
    }
}
