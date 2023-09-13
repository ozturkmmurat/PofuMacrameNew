using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.CategoryAttribute.Select
{
    public class SelectCategoryAttributeDto : IDto
    {
        //Bu Dto admin panel de ilgili category nin sahip oldugu attribute ve attributelerin ozelligini temsil eder.
        public int CategoryAttributeId { get; set; }
        public int CategoryId { get; set; }
        public int AttributeId { get; set; }
        public int? VariableId { get; set; }
        public bool Slicer { get; set; }
        public bool Attribute { get; set; }
        public bool Required { get; set; }
    }
}
