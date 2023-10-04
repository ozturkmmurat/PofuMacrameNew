using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.ProductVariant.Select
{
    public class ProductVariantAttributeValueDto : IEntity
    {
        public int ProductId { get; set; }
        public int? ParentId { get; set; }
        public int AttributeId { get; set; }
        public int? AttributeValueId { get; set; }
        public int ProductVariantId { get; set; }
        public int EndProductVariantId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public string ImagePath { get; set; }

    }
}
