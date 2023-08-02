using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
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
        IResult Delete(ProductStock productStock);
        IDataResult<List<ProductStock>> MappingProductStock(ProductDto productDto);
    }
}
