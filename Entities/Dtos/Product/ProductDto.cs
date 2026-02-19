using Core.Entities;
using Entities.Dtos.ProductAttribute;
using System.Collections.Generic;

namespace Entities.Dtos.Product
{
    /// <summary>
    /// Ürün ekleme (Add) ve güncelleme (TsaUpdate) için tek DTO.
    /// Add: ProductId 0, MainCategoryId zorunlu. TsaUpdate: ProductId zorunlu, sadece ek kategoriler güncellenir.
    /// </summary>
    public class ProductDto : IDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        /// <summary>
        /// Ana kategori. Add'de zorunlu; TsaUpdate'te kullanılmaz.
        /// </summary>
        public int MainCategoryId { get; set; }
        /// <summary>
        /// Ek kategoriler. Add'de isteğe bağlı; TsaUpdate'te sadece ek kategoriler güncellenir.
        /// </summary>
        public List<int> CategoryId { get; set; }
        public List<Entities.Concrete.ProductAttribute> ProductAttributes { get; set; }
    }
}
