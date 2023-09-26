using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
using Entities.Dtos.ProductStock;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductStockService
    {
        IDataResult<List<ProductStock>> GetAll();
        IDataResult<List<ProductStock>> GetAllByProductId(int productId);
        IDataResult<ProductStock> GetById(int id);
        IDataResult<ProductStock> GetByVariantId(int variantId);
        IDataResult<ProductStock> GetByProductId(int productId);
        IDataResult<List<SelectProductStockDto>> GetAllProductStockDto(int productId);
        IResult Add(ProductStock productStock);
        IResult AddList(List<ProductStock> productStocks);
        IResult Update(ProductStock productStock);
        IResult Delete(ProductStock productStock);
    }
}
