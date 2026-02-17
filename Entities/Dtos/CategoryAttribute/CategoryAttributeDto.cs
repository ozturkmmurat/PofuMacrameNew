using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.CategoryAttribute
{
    public class CategoryAttributeDto : IDto
    {
        public List<int> CategoryId { get; set; }
        public int AttributeId { get; set; }
        public int AttributeValueId { get; set; }
        public string AttributeValue { get; set; }
    }
}
