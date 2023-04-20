using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductStockService
    {
        IDataResult<List<ProductStock>> GetAll();
        IDataResult<ProductStock> GetById(int id);
        IDataResult<ProductStock> GetByVariantId(int variantId);
        IResult Add(ProductStock productStock);
        IResult Update(ProductStock productStock);
        IResult Delete(ProductStock productStock);
    }
}
