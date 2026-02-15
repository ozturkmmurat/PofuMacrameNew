using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductPriceFactorService
    {
        IDataResult<List<ProductPriceFactor>> GetAll();
        IDataResult<List<ProductPriceFactor>> GetAllAsNoTracking();
        IDataResult<ProductPriceFactor> GetById(int id);
        IResult Add(ProductPriceFactor productPriceFactor);
        IResult Update(ProductPriceFactor productPriceFactor);
        IResult Delete(ProductPriceFactor productPriceFactor);
    }
}
