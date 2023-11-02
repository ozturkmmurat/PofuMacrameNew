using Business.Abstract;
using Business.Abstract.ProductVariants;
using Business.Utilities;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
using Entities.Dtos.Product.Select;
using Entities.Dtos.ProductVariant;
using Entities.EntitiyParameter.Product;
using Entities.EntityParameter.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        IProductVariantService _productVariantService;
        IProductStockService _productStockService;
        public ProductManager(
            IProductDal productDal,
            IProductStockService productStockService,
            IProductVariantService productVariantService)
        {
            _productDal = productDal;
            _productVariantService = productVariantService;
            _productStockService = productStockService;
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

        public IDataResult<List<SelectListProductVariantDto>> GetAllProductVariantDtoGroupVariant(FilterProduct filterProduct)
        {
            var result = _productDal.GetAllPvFilterDto(filterProduct).Where(x => x.ProductPaths.Count > 0).ToList();
            if (result != null)
            {
                for (int i = 0; i < result.Count(); i++)
                {
                   var endVariant = _productVariantService.MainVariantEndVariant(result[i].ProductVariantId);
                    result[i].EndProductVariantId = endVariant.Data.Id;
                    var productVariantStock = _productStockService.GetByProductVariantId(result[i].EndProductVariantId.Value).Data;
                    result[i].Price = productVariantStock.Price;
                    result[i].Quantity = productVariantStock.Quantity;
                    result[i].StockCode = productVariantStock.StockCode;
                }
                return new SuccessDataResult<List<SelectListProductVariantDto>>(result);
            }
            return new ErrorDataResult<List<SelectListProductVariantDto>>();
        }

        public IDataResult<List<SelectProductDto>> GetallProductDto()
        {
            var result = _productDal.GetAllFilterDto();
            if (result != null)
            {
                if (result.Count >= 0)
                {
                    return new SuccessDataResult<List<SelectProductDto>>(result);
                }
            }
            return new ErrorDataResult<List<SelectProductDto>>(result);
        }

        public IResult TsaAdd(AddProductVariant addProductVariant)
        {
            if (GetById(addProductVariant.ProductId).Data != null)
            {
                var result = _productVariantService.AddTsaProductVariant(addProductVariant);
                if (result.Success)
                {
                    return new SuccessResult();
                }
                return new ErrorResult();
            }
            else
            {

                if (addProductVariant != null)
                {
                    Product product = new Product()
                    {
                        CategoryId = addProductVariant.CategoryId,
                        ProductName = addProductVariant.ProductName,
                        Description = addProductVariant.Description,
                        ProductCode = addProductVariant.ProductCode,
                    };
                    var addProduct = Add(product);
                    if (addProduct.Success)
                    {
                        addProductVariant.ProductId = product.Id;
                        var result = _productVariantService.AddTsaProductVariant(addProductVariant);
                        if (result.Success)
                        {
                            return new SuccessResult();
                        }
                        return new ErrorResult();
                    }
                }
            }
            return new ErrorResult();
        }

        public IDataResult<SelectProductDetailDto> GetProductDetailDtoByPvId(int productVariantId)
        {
            var result = _productDal.GetFilterDto(x => x.ProductVariantId == productVariantId);
            if (result != null)
            {
                return new SuccessDataResult<SelectProductDetailDto>(result);
            }
            return new ErrorDataResult<SelectProductDetailDto>();
        }

        public IDataResult<SelectProductDto> GetByProductDto(int productId)
        {
            var result = _productDal.GetProductFilterDto(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<SelectProductDto>(result);
            }
            return new ErrorDataResult<SelectProductDto>();
        }

        public IDataResult<int> GetTotalProduct(int categoryId)
        {
            var result = _productDal.GetTotalProduct(categoryId);
            return new SuccessDataResult<int>(result);
        }
    }
}
