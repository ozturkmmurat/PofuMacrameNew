using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
using Entities.Dtos.Product.Select;
using Entities.Dtos.ProductVariant;
using Entities.EntitiyParameter.Product;
using Entities.EntityParameter.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        IDataResult<List<SelectProductDto>> GetallProductDto();
        IDataResult<int> GetTotalProduct(int categoryId);
        IDataResult<SelectProductDetailDto> GetProductDetailDtoByPvId(int productVariantId);
        IDataResult<List<SelectListProductVariantDto>> GetAllProductVariantDtoGroupVariant(FilterProduct filterProduct);
        IDataResult<Product> GetById(int id);
        IDataResult<SelectProductDto> GetByProductDto(int productId);
        IResult Add(Product product);
        IResult TsaAdd(AddProductVariant addProductVariant);
        IResult Update(Product product);
        IResult Delete(Product product);
    }
}
