using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos.Product.Select;
using Entities.Dtos.ProductVariant.Select;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IProductVariantDal : IEntityRepository<ProductVariant>
    {
        List<ProductVariantDetailAttributeDto> GetProductDetailAttribute(int productId);
        List<ProductVariantAttributeValueDto> GetSubProductAttributeDto(int productId);
    }
}
