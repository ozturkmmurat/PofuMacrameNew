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
        public ProductVariantManager(
            IProductVariantDal productVariantDal,
            IProductStockService productStockService,
            IProductAttributeService productAttributeService,
            ICategoryAttributeService categoryAttributeService)
        {
            _productVariantDal = productVariantDal;
            _productStockService = productStockService;
            _productAttributeService=productAttributeService;
            _categoryAttributeService=categoryAttributeService;
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
            var properties = addProductVariant.JsonData.EnumerateObject();
            List<ProductStock> productStocks = new List<ProductStock>();
            addProductVariant.ProductVariants = new List<ProductVariant>();



                if (addProductVariant.IsVariant)
                {
                    if (addProductVariant.JsonData.ValueKind != JsonValueKind.Null || (addProductVariant.JsonData.EnumerateObject().Any()) ||
                (addProductVariant.JsonData.ValueKind != JsonValueKind.Object && addProductVariant.JsonData.EnumerateObject().Any()))
                    {
                        addProductVariant.ProductAttributes = new List<ProductAttribute>();
                        for (int q = 0; q < properties.Count(); q++)
                        {
                            var property = properties.ElementAt(q);
                            var propertyName = property.Name;
                            var propertyValue = property.Value;

                            if (propertyValue.ValueKind == JsonValueKind.Array)
                            {
                                var itemList = new List<Dictionary<string, string>>();

                                var items = propertyValue.EnumerateArray();
                                if (items.Count() <= 0)
                                {
                                    return new ErrorResult();
                                }
                                for (int j = 0; j < items.Count(); j++)
                                {
                                    var item = items.ElementAt(j);
                                    var itemProperties = new Dictionary<string, int>();
                                    ProductVariant productVariant = new ProductVariant();

                                    var innerProperties = item.EnumerateObject();
                                    for (int k = 0; k < innerProperties.Count(); k++)
                                    {
                                        var innerProperty = innerProperties.ElementAt(k);
                                        var innerPropertyName = innerProperty.Name;
                                        var innerPropertyValue = innerProperty.Value.GetInt32();


                                        productVariant.ProductId = addProductVariant.ProductId;
                                        productVariant.AttributeId = Convert.ToInt32(innerPropertyName);
                                        productVariant.AttributeValueId = innerPropertyValue;

                                    }
                                    var categoryResult = _categoryAttributeService.GetAllByCategoryId(addProductVariant.CategoryId);
                                    var categoryAttributeResult = categoryResult.Data.Where(x => x.AttributeId == productVariant.AttributeId).FirstOrDefault();
                                    if (categoryAttributeResult.Slicer == true)
                                    {
                                        productVariant.ParentId = null;
                                        Add(productVariant);
                                        addProductVariant.ProductVariantId = productVariant.Id;
                                    }
                                    else if (categoryAttributeResult.Attribute == true)
                                    {
                                        if (j > 0)
                                        {
                                            var x = items.ElementAt(j-1).EnumerateObject().ElementAt(0).Value;
                                            productVariant.ParentId = addProductVariant.ProductVariantId;
                                            Add(productVariant);
                                            addProductVariant.ProductVariantId = productVariant.Id;

                                            if (j == items.Count() -1)
                                            {
                                                ProductStock productStock = new ProductStock();
                                                productStock.ProductId = addProductVariant.ProductId;
                                                productStock.ProductVariantId = productVariant.Id;
                                                productStocks.Add(productStock);
                                            }
                                        }
                                    }

                                    ProductAttribute productAttribute = new ProductAttribute();
                                    productAttribute.AttributeId = productVariant.AttributeId.Value;
                                    productAttribute.AttributeValueId = productVariant.AttributeValueId.Value;
                                    productAttribute.ProductId = addProductVariant.ProductId;
                                    addProductVariant.ProductAttributes.Add(productAttribute);
                                    itemProperties.Add(productAttribute.AttributeId.ToString(), productAttribute.AttributeValueId);
                                }

                            }
                        }
                        if (addProductVariant.ProductStocks != null)
                        {
                            if (addProductVariant.ProductStocks.Count >= 0)
                            {
                                for (int l = 0; l < addProductVariant.ProductStocks.Count; l++)
                                {
                                    productStocks[l].Price =  addProductVariant.ProductStocks[l].Price;
                                    productStocks[l].Quantity = addProductVariant.ProductStocks[l].Quantity;
                                    productStocks[l].StockCode = addProductVariant.ProductStocks[l].StockCode; 
                                }
                                _productStockService.AddList(productStocks);
                            }
                            else
                            {
                                return new ErrorResult();
                            }
                        }
                        else
                        {
                            return new ErrorResult();
                        }

                        if (addProductVariant.ProductAttributes != null)
                        {
                            if (addProductVariant.ProductAttributes.Count >= 0)
                            {
                                _productAttributeService.AddList(addProductVariant.ProductAttributes);
                            }
                        }
                        return new SuccessResult();
                    }
                    else
                    {
                        return new ErrorResult();
                    }
                }
                else if (!addProductVariant.IsVariant)
                {
                    ProductVariant productVariant = new ProductVariant()
                    {
                        ProductId = addProductVariant.ProductId
                    };
                    var result = Add(productVariant);

                    if (result.Success)
                    {
                        ProductStock productStock = new ProductStock()
                        {
                            ProductId = productVariant.ProductId,
                            ProductVariantId = productVariant.Id,
                            Price = addProductVariant.ProductStocks[0].Price,
                            Quantity = addProductVariant.ProductStocks[0].Quantity,
                            StockCode = addProductVariant.ProductStocks[0].StockCode
                        };
                        var productStockResult = _productStockService.Add(productStock);
                        if (!productStockResult.Success)
                        {
                            return new ErrorResult();
                        }
                        return new SuccessResult();
                    }
                    else
                    {
                        return new ErrorResult();
                    }
                }
            return new ErrorResult();
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

        public IDataResult<List<SelectProductVariantDetailDto>> GetAllMainProductVariant(int productId)
        {
            var result = _productVariantDal.GetAllFilterDto(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<List<SelectProductVariantDetailDto>>(result);
            }
            return new ErrorDataResult<List<SelectProductVariantDetailDto>>();
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
    }
}
