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
    public class ProductVariantManager : IProductVariantService
    {
        IProductVariantDal _productVariantDal;
        IProductStockService _productStockService;
        public ProductVariantManager(
            IProductVariantDal productVariantDal,
            IProductStockService productStockService)
        {
            _productVariantDal = productVariantDal;
            _productStockService = productStockService;
        }
        public IResult Add(ProductVariant variant)
        {
            if (variant != null)
            {
                _productVariantDal.Add(variant);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult AddList(List<ProductVariant> variants)
        {
            if (variants != null)
            {
                _productVariantDal.AddRange(variants);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<ProductVariant>> CreateStockCode(ProductDto productDto)
        {
            if (productDto != null)
            {

                List<ProductVariant> variants = new List<ProductVariant>();
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
                    ProductVariant variant = new ProductVariant();
                    variant.StockCode = variantStockCode;
                    variant.ProductId = productDto.ProductId;
                    variants.Add(variant);
                }
                return new SuccessDataResult<List<ProductVariant>>(variants);
            }
            return null;
        }

        public IResult Delete(ProductVariant variant)
        {
            if (variant != null)
            {
                _productVariantDal.Delete(variant);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<ProductVariant>> GetAll()
        {
            var result = _productVariantDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<ProductVariant>>(result);
            }
            return new ErrorDataResult<List<ProductVariant>>();
        }

        public IDataResult<List<ProductVariant>> GetAllByProductId(int productId)
        {
            var result = _productVariantDal.GetAll(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<List<ProductVariant>>(result);
            }
            return new ErrorDataResult<List<ProductVariant>>();
        }

        public IDataResult<List<ViewVariantDto>> GetAllDto()
        {
            var result = _productVariantDal.GetAllFilterDto();
            if (result != null)
            {
                return new SuccessDataResult<List<ViewVariantDto>>(result);
            }
            return new ErrorDataResult<List<ViewVariantDto>>(result);
        }

        public IDataResult<ProductVariant> GetById(int id)
        {
            var result = _productVariantDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>(result);
            }
            return new ErrorDataResult<ProductVariant>();
        }

        public IResult Update(ProductVariant variant)
        {
            if (variant != null)
            {
                _productVariantDal.Update(variant);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
