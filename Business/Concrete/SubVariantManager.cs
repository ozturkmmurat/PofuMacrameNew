using Business.Abstract;
using Business.Utilities;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class SubVariantManager : ISubVariantService
    {
        ISubVariantDal _subVariantDal;
        public SubVariantManager(ISubVariantDal subVariantDal)
        {
            _subVariantDal = subVariantDal;
        }
        public IResult Add(SubVariant variant)
        {
            throw new NotImplementedException();
        }

        public IDataResult<string> CreateStockCode(List<SubVariantDto> subVariantDtos)
        {
            string stockCode = null;
            for (int i = 0; i < subVariantDtos.Count; i++)
            {
                if (stockCode == null)
                {
                    stockCode = subVariantDtos[i].UserId + "-" + subVariantDtos[i].ProductId + "-" + subVariantDtos[i].VariantId  + "-" + subVariantDtos[i].AttrtCode;
                }
                else if (subVariantDtos.Count == 1)
                {
                    stockCode += CreateCodeTime.CreateTime();
                }
                else if (subVariantDtos[i] == subVariantDtos[subVariantDtos.Count - 1])
                {
                    stockCode += CreateCodeTime.CreateTime();
                }
                else
                {
                    stockCode += "-" + subVariantDtos[i].AttrtCode;
                }
            }
            if (stockCode != null)
            {
                return new SuccessDataResult<string>(stockCode);
            }
            return new ErrorDataResult<string>();
        }

        public IResult Delete(SubVariant variant)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<SubVariant>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<SubVariant>> GetAllByVariantId(int variantId)
        {
            throw new NotImplementedException();
        }

        public IDataResult<SubVariant> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IDataResult<SubVariant> GetByStockCode(string stockCode)
        {
            throw new NotImplementedException();
        }

        public IResult Update(SubVariant variant)
        {
            throw new NotImplementedException();
        }
    }
}
