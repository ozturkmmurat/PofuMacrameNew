using Business.Abstract;
using Business.Utilities;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
using Entities.Dtos.Product.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        IProductStockService _productStockService;
        IVariantService _variantService;
        public ProductManager(
            IProductDal productDal,
            IProductStockService productStockService,
            IVariantService variantService)
        {
            _productDal = productDal;
            _productStockService = productStockService;
            _variantService = variantService;
        }
        public IResult Add(Product product)
        {
            if (product != null)
            {
                _productDal.Add(product);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
        [TransactionScopeAspect]
        public IResult TsaAdd(ProductDto addProductDto)
        {
            if (addProductDto != null)
            {
                Product product = new Product()
                {
                    CategoryId = addProductDto.CategoryId,
                    ProductName = addProductDto.ProductName,
                    Description = addProductDto.Description
                };
                _productDal.Add(product);
                addProductDto.ProductId = product.Id;

                var variantCreateStockCode = _variantService.CreateStockCode(addProductDto);
                if (variantCreateStockCode == null)
                {
                    return new ErrorResult();
                }
                else
                {
                    if (variantCreateStockCode != null)
                    {

                        var result = _variantService.AddList(variantCreateStockCode.Data);
                        for (int i = 0; i < addProductDto.AddVariantDtos.Count; i++)
                        {
                            addProductDto.AddVariantDtos[i].VariantId = variantCreateStockCode.Data[i].Id;
                        }
                        if (!result.Success)
                        {
                            return new ErrorResult();
                        }
                        else
                        {
                            var mapPrdStckResult = _productStockService.MappingProductStock(addProductDto);
                            var addProductStock = _productStockService.AddList(mapPrdStckResult.Data);
                        }
                    }
                }
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(Product product)
        {
            if (product != null)
            {
                _productDal.Delete(product);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<Product>> GetAll()
        {
            var result = _productDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<Product>>(result);
            }
            return new ErrorDataResult<List<Product>>();
        }

        public IDataResult<Product> GetById(int id)
        {
            var result = _productDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<Product>(result);
            }
            return new ErrorDataResult<Product>();
        }

        public IResult Update(Product product)
        {
            if (product != null)
            {
                _productDal.Update(product);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<SelectProductDto>> GetAllDto()
        {
            var result = _productDal.GetAllFilterDto();
            if (result != null)
            {
                return new SuccessDataResult<List<SelectProductDto>>(result);
            }
            return new ErrorDataResult<List<SelectProductDto>>();
        }
    }
}
