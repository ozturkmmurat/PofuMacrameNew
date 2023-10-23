using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.CategoryAttribute.Select
{
    //Urun listesi sayfasinda kullaniliyor filtre kısmında hangi kategorinin hangi ozellige sahip oldugu gosterilir
    public class FilterCategoryAttributeDto
    {
        public int AttributeId { get; set; }
        public string CategoryName { get; set; }
        public string AttributeName { get; set; }
        public List<AttributeValue> AttributeValues { get; set; }

    }
}
