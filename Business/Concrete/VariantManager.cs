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
    public class VariantManager : IVariantService
    {
        IVariantDal _variantDal;
        IProductAttributeService _productAttributeService;
        public VariantManager(
            IVariantDal variantDal,
            IProductAttributeService productAttributeService)
        {
            _variantDal = variantDal;
            _productAttributeService = productAttributeService;
        }
        public IResult Add(Variant variant)
        {
            if (variant != null)
            {
                _variantDal.Add(variant);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<string> CreateStockCode(List<VariantDto> variantDtos)
        {
            string stockCode = null;
            for (int i = 0; i < variantDtos.Count; i++)
            {
                if (stockCode == null)
                {
                    stockCode = variantDtos[i].UserId + "-" + variantDtos[i].ProductId + "-" + variantDtos[i].AttrtCode;
                }
                else if(variantDtos.Count  == 1){
                    stockCode += CreateCodeTime.CreateTime();
                }
                else if(variantDtos[i] == variantDtos[variantDtos.Count -1])
                {
                    stockCode += CreateCodeTime.CreateTime();
                }
                else
                {
                    stockCode += "-" + variantDtos[i].AttrtCode;
                }
            }
            if (stockCode != null)
            {
                return new SuccessDataResult<string>(stockCode);
            }
            return new ErrorDataResult<string>();
        }

        public IResult Delete(Variant variant)
        {
            if (variant != null)
            {
                _variantDal.Delete(variant);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<Variant>> GetAll()
        {
            var result = _variantDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<Variant>>(result);
            }
            return new ErrorDataResult<List<Variant>>();
        }

        public IDataResult<List<Variant>> GetAllByProductId(int productId)
        {
            var result = _variantDal.GetAll(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<List<Variant>>(result);
            }
            return new ErrorDataResult<List<Variant>>();
        }

        public IDataResult<Variant> GetById(int id)
        {
            var result = _variantDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<Variant>(result);
            }
            return new ErrorDataResult<Variant>();
        }

        public IDataResult<Variant> GetByStockCode(string stockCode)
        {
            var result = _variantDal.Get(x => x.StockCode == stockCode);
            if (result != null)
            {
                return new SuccessDataResult<Variant>(result);
            }
            return new ErrorDataResult<Variant>();
        }

        public IResult Update(Variant variant)
        {
            if (variant != null)
            {
                _variantDal.Update(variant);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
