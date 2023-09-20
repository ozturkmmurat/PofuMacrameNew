using Business.Abstract;
using Business.Utilities;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.ProductVariant;
using Entities.Dtos.ProductVariant.Select;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Business.Concrete
{
    public class ProductVariantManager : IProductVariantService
    {
        IProductVariantDal _productVariantDal;
        IProductStockService _productStockService;
        IProductAttributeService _productAttributeService;
        ICategoryAttributeService _categoryAttributeService;
        IAttributeValueService _attributeValueService;
        IAttributeService _attributeService;
        public ProductVariantManager(
            IProductVariantDal productVariantDal,
            IProductStockService productStockService,
            IProductAttributeService productAttributeService,
            ICategoryAttributeService categoryAttributeService,
            IAttributeValueService attributeValueService,
            IAttributeService attributeService)
        {
            _productVariantDal = productVariantDal;
            _productStockService = productStockService;
            _productAttributeService=productAttributeService;
            _categoryAttributeService=categoryAttributeService;
            _attributeValueService =attributeValueService;
            _attributeService=attributeService;
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
        [TransactionScopeAspect]
        public IResult AddTsaProductVariant(AddProductVariant addProductVariant)
        {
            // JsonElement'i bir string'e dönüştürün
            string jsonDataString = addProductVariant.JsonData.ToString();

            // Json verilerini çözümleyin
            var jsonData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, int>>>>(jsonDataString);

            List<ProductStock> productStocks = new List<ProductStock>();
            addProductVariant.ProductVariants = new List<ProductVariant>();

            foreach (var item in jsonData)
            {
                var propertyName = item.Key;
                var variants = item.Value;

                if (variants != null && variants.Any())
                {
                    foreach (var variant in variants)
                    {
                        ProductVariant productVariant = new ProductVariant();

                        foreach (var attribute in variant)
                        {
                            var attributeId = Convert.ToInt32(attribute.Key);
                            var attributeValue = attribute.Value;

                            var categoryResult = _categoryAttributeService.GetAllByCategoryId(addProductVariant.CategoryId);
                            var categoryAttributeResult = categoryResult.Data.Where(x => x.AttributeId == attributeId).FirstOrDefault();
                            if (categoryAttributeResult.Slicer == true)
                            {
                                productVariant.ParentId = 0;
                                if (GetByParentIdAttrValueId(addProductVariant.ProductId, productVariant.ParentId, attributeValue).Success == false)
                                {
                                    addProductVariant.ParentId = attributeValue;
                                    productVariant.ProductId = addProductVariant.ProductId;
                                    productVariant.AttributeId = attributeId;
                                    productVariant.AttributeValueId = attributeValue;
                                    Add(productVariant);
                                }
                                else
                                {
                                    addProductVariant.ParentId = attributeValue;
                                }
                            }

                           
                            // ProductVariant'ı veritabanına ekleyin
                            if (categoryAttributeResult.Attribute == true)
                            {
                                if (GetByParentIdAttrValueId(addProductVariant.ProductId, addProductVariant.ParentId, attributeValue).Success == false)
                                {
                                    productVariant.ProductId = addProductVariant.ProductId;
                                    productVariant.AttributeId = attributeId;
                                    productVariant.AttributeValueId = attributeValue;
                                    productVariant.ParentId = addProductVariant.ParentId;
                                    Add(productVariant);
                                    addProductVariant.ParentId = attributeValue;
                                }
                                else
                                {
                                    addProductVariant.ParentId = attributeValue;
                                }
                            }

                            if (variants.IndexOf(variant) == variants.Count -1)
                            {
                                // ProductStock nesnesini doldurun
                                ProductStock productStock = new ProductStock()
                                {
                                    ProductId = addProductVariant.ProductId,
                                    ProductVariantId = productVariant.Id,
                                    Price = addProductVariant.ProductStocks[0].Price,
                                    Quantity = addProductVariant.ProductStocks[0].Quantity,
                                    StockCode = addProductVariant.ProductStocks[0].StockCode
                                };

                                // ProductStock'ı listeye ekleyin
                                productStocks.Add(productStock);
                            }
                           
                        }
                    }
                }
            }

            // ProductStock'ları veritabanına ekleyin
            _productStockService.AddList(productStocks);

            // Diğer işlemleri burada gerçekleştirin

            return new SuccessResult();
        }

        public IResult Delete(ProductVariant variant)
        {
            if (variant != null)
            {
                var getSubProductVariant = GetSubProductVariantById(variant.Id);
                if (getSubProductVariant.Data != null)
                {
                    if (getSubProductVariant.Data.Count() >= 0)
                    {
                        _productVariantDal.DeleteRange(getSubProductVariant.Data);
                        return new SuccessResult();
                    }
                }
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

        public IDataResult<ProductVariant> GetById(int id)
        {
            var result = _productVariantDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>(result);
            }
            return new ErrorDataResult<ProductVariant>();
        }

        public IDataResult<ProductVariant> GetByProductId(int productId)
        {
            var result = _productVariantDal.Get(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>(result);
            }
            return new ErrorDataResult<ProductVariant>();
        }

        public IDataResult<List<ProductVariantAttributeDto>> GetAllMainProductVariant(int productId)
        {
            var result = _productVariantDal.GetAllFilterDto(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<List<ProductVariantAttributeDto>>(result);
            }
            return new ErrorDataResult<List<ProductVariantAttributeDto>>();
        }

        public IDataResult<List<ProductVariant>> GetSubProductVariantById(int productVariantId)
        {
            List<ProductVariant> productVariants = new List<ProductVariant>();
            var result = _productVariantDal.Get(x => x.Id == productVariantId && x.ParentId == null);
            if (result != null)
            {
                productVariants.Add(result);
                var productVariantAll = GetAllByProductId(result.ProductId).Data;
                for (int i = 0; i < productVariantAll.Count(); i++)
                {
                    for (int j = 0; j < productVariants.Count(); j++)
                    {
                        if (productVariants[j].Id == productVariantAll[i].ParentId)
                        {
                            productVariants.Add(productVariantAll[i]);
                        }
                    }
                }
                return new SuccessDataResult<List<ProductVariant>>(productVariants);
            }
            return new ErrorDataResult<List<ProductVariant>>();
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

        public IDataResult<List<MainProductVariantAttributeDto>> GetProductVariantAttribute(int productId)
        {
            var result = _productVariantDal.GetProductVariantAttributes(productId);
            if (result != null)
            {
                for (int i = 0; i < result.Count(); i++)
                {
                    var mainAttributeValues = GetMainProductVariantAttrValue(result[i].ProductId, result[i].ParentId, result[i].AttributeId).Data;
                    result[i].ProductVariantAttributeValueDtos  = mainAttributeValues;
                    result[i].ProductVariantAttributeValueDtos = result[i].ProductVariantAttributeValueDtos.GroupBy(x => x.AttributeValue).FirstOrDefault().ToList();
                }
                return new SuccessDataResult<List<MainProductVariantAttributeDto>>(result);
            }
            return new ErrorDataResult<List<MainProductVariantAttributeDto>>();
        }
        public IDataResult<List<ProductVariant>> GetAllByProductIdAttrId(int productId, int? attributeId)
        {
            var result = _productVariantDal.GetAll(x => x.ProductId == productId && x.AttributeId == attributeId);
            if (result != null)
            {
                return new SuccessDataResult<List<ProductVariant>>(result);
            }
            return new ErrorDataResult<List<ProductVariant>>();
        }

        public IDataResult<List<ProductVariantAttributeValueDto>> GetMainProductVariantAttrValue(int productId, int? parentId, int attributeId)
        {
            var result = _productVariantDal.GetMainProductAttributeValue(productId, parentId, attributeId);
            if (result != null)
            {
                if (result.Count() > 0)
                {
                    List<ProductVariantAttributeValueDto> list = new List<ProductVariantAttributeValueDto>();
                    list = result;
                    return new SuccessDataResult<List<ProductVariantAttributeValueDto>>(list);
                }
            }

            return new ErrorDataResult<List<ProductVariantAttributeValueDto>>();
        }

        public IDataResult<ProductVariant> GetByParentIdAttrValueId(int productId, int? parentId, int? attributeValueId)
        {
            var result = _productVariantDal.Get(x => x.ProductId == productId &&  x.ParentId == parentId && x.AttributeValueId == attributeValueId);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>();
            }
            return new ErrorDataResult<ProductVariant>();
        }
    }
}
