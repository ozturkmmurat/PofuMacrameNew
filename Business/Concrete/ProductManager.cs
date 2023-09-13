using Business.Abstract;
using Business.Utilities;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
using Entities.Dtos.Product.Select;
using Entities.Dtos.ProductVariant;
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
        public ProductManager(
            IProductDal productDal,
            IProductStockService productStockService,
            IProductVariantService productVariantService)
        {
            _productDal = productDal;
            _productVariantService = productVariantService;
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

        public IDataResult<List<SelectListProductVariantDto>> GetAllPvProductVariantDtoGroupProduct()
        {
            // Web sitesinde urunler listelenirken kullaniliyor
            var result = _productDal.GetAllPvFilterDto().GroupBy(x => x.ProductId).Select(group => group.FirstOrDefault()).ToList();
            if (result != null)
            {
                return new SuccessDataResult<List<SelectListProductVariantDto>>(result);
            }
            return new ErrorDataResult<List<SelectListProductVariantDto>>();
        }

        public IDataResult<List<SelectListProductVariantDto>> GetAllProductVariantDtoGroupVariant()
        {
            // Web sitesinde urune bağli urun varyantlari listelenirken kullanılıyor
            var result = _productDal.GetAllPvFilterDto(x => x.ParentId == null).GroupBy(x => x.ProductVariantId).Select(group => group.FirstOrDefault()).ToList(); ;
            if (result != null)
            {
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
    }
}
