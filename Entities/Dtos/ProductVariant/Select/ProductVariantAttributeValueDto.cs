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
        public int ProductVariantId { get; set; }
        public string AttributeValue { get; set; }
    }
}
