using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.CategoryAttribute.Select
{
    //Admin panel de ilgili kategorinin ozelliklerini listelemek icin kullaniliyor.
    public class ViewCategoryAttributeDto : IDto
    {
        public int CategoryId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public List<AttributeValue> AttributeValues { get; set; }
        public bool Slicer { get; set; }
        public bool Attribute { get; set; }
    }
}
