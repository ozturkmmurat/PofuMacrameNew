using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.ProductAttribute
{
    public class ProductAttributeDto : IDto
    {
        public List<int> CategoryId { get; set; }
        public int AttributeId { get; set; }
        public int AttributeValueId { get; set; }
        public string AttributeValue { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string AttributeName { get; set; }
    }
}
