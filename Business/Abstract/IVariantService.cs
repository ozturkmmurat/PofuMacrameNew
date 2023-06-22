using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Variant;
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
        IDataResult<List<Variant>> MappingVariant(List<AddVariantDto> addVariantDtos);
        string CreateStockCode(List<AddVariantDto> addVariantDtos);
        IResult Add(Variant variant);
        // Tsa --> TransactionScopeAspect
        IResult TsaAddList(List<AddVariantDto> addVariantDtos);
        IResult Update(Variant variant);
        IResult TsaUpdateList(List<AddVariantDto> addVariantDtos);
        IResult Delete(Variant variant);
    }
}
