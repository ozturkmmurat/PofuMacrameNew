using Business.Abstract;
using Business.Utilities;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Variant;
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

        public string CreateStockCode(List<AddVariantDto> addVariantDtos)
        {
            if (addVariantDtos != null)
            {
                string variantStockCode = null;
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < addVariantDtos[i].AttrCode.Count; j++)
                    {
                        if (variantStockCode == null)
                        {
                            variantStockCode += addVariantDtos[i].ProductId + "-" + addVariantDtos[i].VariantId + "-" + addVariantDtos[i].AttrCode[j] + "-";
                        }
                        else if (addVariantDtos[i].AttrCode.Count == 1)
                        {
                            variantStockCode += addVariantDtos[i].AttrCode[j];
                        }
                        else if (addVariantDtos[i].AttrCode[j] == addVariantDtos[i].AttrCode[addVariantDtos[i].AttrCode.Count - 1])
                        {
                            variantStockCode += addVariantDtos[i].AttrCode[j] + "-" + CreateCodeTime.CreateTime();
                        }
                        else
                        {
                            variantStockCode += "-" + addVariantDtos[i].AttrCode[j];
                        }
                    }
                }
                return variantStockCode;
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

        public IDataResult<Variant> GetById(int id)
        {
            var result = _variantDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<Variant>(result);
            }
            return new ErrorDataResult<Variant>();
        }

        public IDataResult<List<Variant>> MappingVariant(List<AddVariantDto> addVariantDtos)
        {
            if (addVariantDtos != null)
            {
                List<Variant> variants = new List<Variant>();
                for (int i = 0; i < addVariantDtos.Count; i++)
                {
                    Variant variant = new Variant();
                    variant.Id = addVariantDtos[i].VariantId;
                    variant.ProductId = addVariantDtos[i].ProductId;
                    variants.Add(variant);
                }
                return new SuccessDataResult<List<Variant>>(variants);
            }
            return new ErrorDataResult<List<Variant>>();
        }

        public IResult TsaAddList(List<AddVariantDto> addVariantDtos)
        {
            if (addVariantDtos != null)
            {
                List<ProductStock> productStocks = new List<ProductStock>();
                AddVariantDto addVariantDto = new AddVariantDto();
                var mapResult = MappingVariant(addVariantDtos);
                string variantStockCode = null;
                _variantDal.AddRange(mapResult.Data);
                for (int i = 0; i < addVariantDtos.Count; i++)
                {
                    ProductStock productStock = new ProductStock();
                    productStock.VariantId = mapResult.Data[i].Id;
                    variantStockCode = CreateStockCode(addVariantDtos);
                    if (variantStockCode == null)
                    {
                        return new ErrorResult();
                    }
                    productStock.ProductId = addVariantDtos[i].ProductId;
                    productStock.Price = addVariantDtos[i].ProductStock.Price;
                    productStock.Quantity = addVariantDtos[i].ProductStock.Quantity;
                    productStock.StockCode = variantStockCode;
                    productStocks.Add(productStock);
                }
                _productStockService.AddList(productStocks);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult TsaUpdateList(List<AddVariantDto> addVariantDtos)
        {
            throw new NotImplementedException();
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
