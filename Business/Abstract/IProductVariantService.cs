using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.Variant;
using Entities.Dtos.Variant.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductVariantService
    {
        //Variant
        IDataResult<List<ProductVariant>> GetAll();
        IDataResult<List<ProductVariant>> GetAllByProductId(int productId);
        IDataResult<ProductVariant> GetById(int id);
        //Dto
        IDataResult<List<ViewVariantDto>> GetAllDto();
        IResult Add(ProductVariant variant);
        IResult AddList(List<ProductVariant> variants);
        IResult Update(ProductVariant variant);
        IResult Delete(ProductVariant variant);

        IDataResult <List<ProductVariant>> CreateStockCode(ProductDto productDto);
    }
}
