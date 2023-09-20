using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.ProductVariant.Select
{
    public class ProductVariantAttributeDto : IDto
    {
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public int AttributeValueId { get; set; }
        public int? ParentId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }
        public List<string> ProductPaths { get; set; }
    }
}
