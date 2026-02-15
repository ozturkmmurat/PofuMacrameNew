using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Dtos.ProductImage
{
    public class ProductImageDto : IEntity
    {
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public string Path { get; set; }
        public bool IsMain { get; set; }
        public DateTime? CreateDate { get; set; }

        [NotMapped]
        public List<IFormFile> Files { get; set; }
    }
}
