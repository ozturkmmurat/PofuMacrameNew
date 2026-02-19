using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.ProductAttribute;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IProductAttributeDal : IEntityRepository<ProductAttribute>
    {
        List<ProductAttributeDto> GetProductVariantAttribute(Expression<Func<ProductAttributeDto, bool>> filter = null);
        List<ProductAttribute> GetAllProductIdListNT(List<int> ids);
        List<Entities.Dtos.ProductAttribute.ProductAttributeDto> GetAllProductAttribute(Entities.Dtos.ProductAttribute.ProductAttributeDto productAttributeDto);
        List<FilterProductAttributeDto> GetFilterAttributesByProductIds(List<int> productIds);
    }
}
