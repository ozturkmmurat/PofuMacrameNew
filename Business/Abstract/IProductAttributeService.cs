using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductAttributeService
    {
        IDataResult<List<ProductAttribute>> GetAll();
        IDataResult<List<ProductAttribute>> GetAllByProductId(int productId);
        IDataResult<List<ProductAttribute>> GetAllByProductIds(List<int> productIds);
        IDataResult<ProductAttribute> MappingProductAttribute(ProductVariant productVariant);
        IResult Add(ProductAttribute productAttribute);
        IResult AddList(List<ProductAttribute> productAttributes);
        IResult Update(ProductAttribute productAttribute);
        IResult Delete(ProductAttribute productAttribute);
    }
}
