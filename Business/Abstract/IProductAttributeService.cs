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
        IDataResult<List<ProductAttributeDto>> GetAllDtoByProductId(int productId);
        IResult Add(ProductAttribute productAttribute);
        IResult Update(ProductAttribute productAttribute);
        IResult Delete(ProductAttribute productAttribute);
    }
}
