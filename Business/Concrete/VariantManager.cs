using Business.Abstract;
using Business.Utilities;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.Variant;
using Entities.Dtos.Variant.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class VariantManager : IVariantService
    {
        IVariantDal _variantDal;
        IProductStockService _productStockService;
        public VariantManager(
            IVariantDal variantDal,
            IProductStockService productStockService)
        {
            _variantDal = variantDal;
            _productStockService = productStockService;
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

        public IResult AddList(List<Variant> variants)
        {
            if (variants != null)
            {
                _variantDal.AddRange(variants);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<Variant>> CreateStockCode(ProductDto productDto)
        {
            if (productDto != null)
            {

                List<Variant> variants = new List<Variant>();
                for (int i = 0; i < productDto.AddVariantDtos.Count; i++)
                {
                    string variantStockCode = null;
                    for (int j = 0; j < productDto.AddVariantDtos[i].AttrCode.Count; j++)
                    {
                        if (variantStockCode == null)
                        {
                            variantStockCode += productDto.ProductId + "-"  + productDto.AddVariantDtos[i].AttrCode[j] + "-";
                        }
                        else if (productDto.AddVariantDtos[i].AttrCode.Count == 1)
                        {
                            variantStockCode += productDto.AddVariantDtos[i].AttrCode[j];
                        }
                        else if (productDto.AddVariantDtos[i].AttrCode[j] == productDto.AddVariantDtos[i].AttrCode[productDto.AddVariantDtos[i].AttrCode.Count - 1])
                        {
                            variantStockCode += productDto.AddVariantDtos[i].AttrCode[j] + "-" + CreateCodeTime.CreateTime();
                        }
                        else
                        {
                            variantStockCode += "-" + productDto.AddVariantDtos[i].AttrCode[j];
                        }
                    }
                    Variant variant = new Variant();
                    variant.StockCode = variantStockCode;
                    variant.ProductId = productDto.ProductId;
                    variants.Add(variant);
                }
                return new SuccessDataResult<List<Variant>>(variants);
            }
            return null;
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

        public IDataResult<List<ViewVariantDto>> GetAllDto()
        {
            var result = _variantDal.GetAllFilterDto();
            if (result != null)
            {
                return new SuccessDataResult<List<ViewVariantDto>>(result);
            }
            return new ErrorDataResult<List<ViewVariantDto>>(result);
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
