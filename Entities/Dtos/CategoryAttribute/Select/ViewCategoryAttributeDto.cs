using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.CategoryAttribute.Select
{
    public class ViewCategoryAttributeDto : IDto
    {
        public int CategoryId { get; set; }
        public int AttributeId { get; set; }
        public int AttributeValueId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public bool Slicer { get; set; }
        public bool Attribute { get; set; }
    }
}
