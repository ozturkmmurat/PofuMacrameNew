using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.ProductVariant.Select
{
    public class ProductVariantDetailAttributeDto : IDto
    {
        public int? ParentId { get; set; }
        public int? AttributeId { get; set; }
        public string AttributeName { get; set; }
        public List<AttributeValue> AttributeValues { get; set; }
    }
}
