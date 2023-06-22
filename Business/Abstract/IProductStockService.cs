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
        IDataResult<ProductStock> GetByProductId(int productId);
        IResult Add(ProductStock productStock);
        IResult AddList(List<ProductStock> productStocks);
        IResult Update(ProductStock productStock);
        IResult UpdateList(List<ProductStock> productStocks);
        IResult Delete(ProductStock productStock);
    }
}
