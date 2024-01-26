using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.ProductVariant.Select
{
    public class ProductVariantFilterDto : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? AttributeId { get; set; }
        public int? AttributeValueId { get; set; }
        public int? ParentId { get; set; }
        public List<AttributeValue> AttributeValues { get; set; }
    }
}
