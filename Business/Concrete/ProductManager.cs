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
        public IResult TsaAdd(AddProductDto addProductDto)
        {
            if (addProductDto != null)
            {
                Product product = new Product()
                {
                    CategoryId = addProductDto.CategoryId,
                    Title = addProductDto.Title,
                    Description = addProductDto.Description
                };
                _productDal.Add(product);
                addProductDto.ProductId = product.Id;
                string productStockCode = CreateStockCode(addProductDto);
                if (productStockCode == null)
                {
                    return new ErrorResult();
                }
                ProductStock productStock = new ProductStock()
                {
                    ProductId = addProductDto.ProductId,
                    Price = addProductDto.ProductStocks[0].Price,
                    Quantity = addProductDto.ProductStocks[0].Quantity,
                    StockCode = productStockCode
                };
                _productStockService.Add(productStock);
                if (addProductDto.AddVariantDtos != null)
                {
                    for (int i = 0; i < addProductDto.AddVariantDtos.Count; i++)
                    {
                        addProductDto.AddVariantDtos[i].ProductId = product.Id;
                    }
                   var result = _variantService.TsaAddList(addProductDto.AddVariantDtos);
                    if (!result.Success)
                    {
                        return new ErrorResult();
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

        public string CreateStockCode(AddProductDto addProductDto)
        {
            if (addProductDto != null)
            {
                string productStockCode = null;
                for (int i = 0; i < addProductDto.AttrCode.Count; i++)
                {
                    if (productStockCode == null)
                    {
                        productStockCode += addProductDto.ProductId + "-" + addProductDto.AttrCode[i];
                    }
                    else if (addProductDto.AttrCode.Count == 1)
                    {
                        productStockCode += addProductDto.AttrCode[i];
                    }
                    else if (addProductDto.AttrCode[i] == addProductDto.AttrCode[addProductDto.AttrCode.Count - 1])
                    {
                        productStockCode += addProductDto.AttrCode[i] + "-" + CreateCodeTime.CreateTime();
                    }
                    else
                    {
                        productStockCode += "-" + addProductDto.AttrCode[i];
                    }
                }
                return productStockCode;
            }
            return null;
        }

        public IDataResult<List<SelectProductDto>> GetAllProductAndVariant()
        {
            var result = _productDal.GetAllProductAndVariant();
            if (result != null)
            {
                return new SuccessDataResult<List<SelectProductDto>>(result);
            }
            return new ErrorDataResult<List<SelectProductDto>>();
        }
    }
}
