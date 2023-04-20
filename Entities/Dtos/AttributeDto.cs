using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class AttributeDto : IDto
    {
        public int AttributeId { get; set; }
        public int AttributeValueId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}
