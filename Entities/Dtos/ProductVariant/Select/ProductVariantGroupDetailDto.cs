using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.ProductVariant.Select
{
    public class ProductVariantGroupDetailDto : IDto
    {

        public int ParentId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public List<ProductVariantAttributeValueDto> ProductVariantAttributeValueDtos { get; set; }
    }
}
