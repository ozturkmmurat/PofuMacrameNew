using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class AttributeValue : IEntity
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }
    }
}
