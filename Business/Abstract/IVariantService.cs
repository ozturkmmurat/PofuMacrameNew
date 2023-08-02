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
    public interface IVariantService
    {
        //Variant
        IDataResult<List<Variant>> GetAll();
        IDataResult<List<Variant>> GetAllByProductId(int productId);
        IDataResult<Variant> GetById(int id);
        //Dto
        IDataResult<List<ViewVariantDto>> GetAllDto();
        IResult Add(Variant variant);
        IResult AddList(List<Variant> variants);
        IResult Update(Variant variant);
        IResult Delete(Variant variant);

        IDataResult <List<Variant>> CreateStockCode(ProductDto productDto);
    }
}
