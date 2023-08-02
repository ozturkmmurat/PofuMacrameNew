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
        IDataResult<List<SelectProductDto>> GetAllDto();
        IDataResult<Product> GetById(int id);
        IResult Add(Product product);
        IResult TsaAdd(ProductDto addProductDto);
        IResult Update(Product product);
        IResult Delete(Product product);
    }
}
