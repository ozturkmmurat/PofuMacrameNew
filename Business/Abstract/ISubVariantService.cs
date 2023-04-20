using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ISubVariantService
    {
        IDataResult<List<SubVariant>> GetAll();
        IDataResult<List<SubVariant>> GetAllByVariantId(int variantId);
        IDataResult<SubVariant> GetById(int id);
        IDataResult<SubVariant> GetByStockCode(string stockCode);
        IResult Add(SubVariant variant);
        IResult Update(SubVariant variant);
        IResult Delete(SubVariant variant);
        IDataResult<string> CreateStockCode(List<SubVariantDto> subVariantDtos);
    }
}
