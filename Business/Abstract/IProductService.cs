using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
using Entities.Dtos.Product.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        IDataResult<Product> GetById(int id);
        IDataResult<List<SelectProductDto>> GetAllProductAndVariant();
        string CreateStockCode(AddProductDto addProductDto);
        IResult Add(Product product);
        IResult TsaAdd(AddProductDto addProductDto);
        IResult Update(Product product);
        IResult Delete(Product product);
    }
}
