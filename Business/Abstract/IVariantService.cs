using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IVariantService
    {
        IDataResult<List<Variant>> GetAll();
        IDataResult<List<Variant>> GetAllByProductId(int productId);
        IDataResult<Variant> GetById(int id);
        IDataResult<Variant> GetByStockCode(string stockCode);
        IDataResult<string> CreateStockCode(List<VariantDto> variantDtos);
        IResult Add(Variant variant);
        IResult Update(Variant variant);
        IResult Delete(Variant variant);
    }
}
