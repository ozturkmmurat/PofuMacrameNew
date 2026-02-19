using Entities.Concrete;
using System.Collections.Generic;

namespace Entities.Dtos.ProductAttribute
{
    /// <summary>
    /// Ürün listesi filtre alanında kullanılır. Verilen ürünlere ait filtre özellikleri ve seçeneklerini taşır.
    /// </summary>
    public class FilterProductAttributeDto
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public List<AttributeValue> AttributeValues { get; set; }
    }
}
