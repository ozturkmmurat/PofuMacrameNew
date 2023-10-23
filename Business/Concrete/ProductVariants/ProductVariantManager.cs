using Business.Abstract;
using Business.Abstract.ProductVariants;
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
        ICategoryAttributeService _categoryAttributeService;
        IProductVariantAttributeCombinationService _productVariantAttributeCombinationService;
        IProductImageService _productImageService;
        IProductAttributeService _productAttributeService;
        public ProductVariantManager(
            IProductVariantDal productVariantDal,
            IProductStockService productStockService,
            IProductAttributeService productAttributeService,
            ICategoryAttributeService categoryAttributeService,
            IProductVariantAttributeCombinationService productVariantAttributeCombinationService,
            IProductImageService productImageService
            )
        {
            _productVariantDal = productVariantDal;
            _productStockService = productStockService;
            _categoryAttributeService=categoryAttributeService;
            _productVariantAttributeCombinationService = productVariantAttributeCombinationService;
            _productImageService=productImageService;
            _productAttributeService = productAttributeService;
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
                        ProductAttribute productAttribute = new ProductAttribute();
                        foreach (var attribute in variant)
                        {
                            var attributeId = Convert.ToInt32(attribute.Key);
                            var attributeValue = attribute.Value;

                            var categoryResult = _categoryAttributeService.GetAllByCategoryId(addProductVariant.CategoryId);
                            var categoryAttributeResult = categoryResult.Data.Where(x => x.AttributeId == attributeId).FirstOrDefault();
                            if (categoryAttributeResult.Slicer == true)
                            {
                                productVariant.ParentId = 0;
                                var checkData = GetByParentIdAttrValueId(addProductVariant.ProductId, productVariant.ParentId, attributeValue, attributeId);
                                if (checkData.Success == false)
                                {
                                    addProductVariant.ParentId = 0;
                                    productVariant.ProductId = addProductVariant.ProductId;
                                    productVariant.AttributeId = attributeId;
                                    productVariant.AttributeValueId = attributeValue;
                                    
                                    Add(productVariant);
                                    productAttribute.ProductId = productVariant.ProductId;
                                    productAttribute.AttributeId = attributeId;
                                    productAttribute.AttributeValueId = attributeValue;
                                    _productAttributeService.Add(productAttribute);
                                    addProductVariant.ParentId = productVariant.Id;
                                }
                                else
                                {
                                    addProductVariant.ParentId = checkData.Data.Id;
                                }
                            }


                            // ProductVariant'ı veritabanına ekleyin
                            if (categoryAttributeResult.Attribute == true)
                            {
                                var checkData = GetByParentIdAttrValueId(addProductVariant.ProductId, addProductVariant.ParentId, attributeValue, attributeId);
                                if (checkData.Success == false)
                                {
                                    productVariant.ProductId = addProductVariant.ProductId;
                                    productVariant.AttributeId = attributeId;
                                    productVariant.AttributeValueId = attributeValue;
                                    productVariant.ParentId = addProductVariant.ParentId;
                                    Add(productVariant);
                                    productAttribute.ProductId = productVariant.ProductId;
                                    productAttribute.AttributeId = attributeId;
                                    productAttribute.AttributeValueId = attributeValue;
                                    _productAttributeService.Add(productAttribute);
                                    addProductVariant.ParentId =  productVariant.Id;
                                }
                                else
                                {
                                    addProductVariant.ParentId = checkData.Data.Id;
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
                var getSubProductVariant = MapProductVariantCombination(variant.ProductId, Convert.ToInt32(variant.AttributeValueId));
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

        public IResult Update(ProductVariant variant)
        {
            if (variant != null)
            {
                _productVariantDal.Update(variant);
                return new SuccessResult();
            }
            return new ErrorResult();
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

        public IDataResult<ProductVariant> GetByParentIdAttrValueId(int productId, int? parentId, int? attributeValueId, int? attributeId)
        {
            var result = _productVariantDal.Get(x => x.ProductId == productId && x.ParentId == parentId  && x.AttributeValueId == attributeValueId && x.AttributeId == attributeId);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>(result);
            }
            return new ErrorDataResult<ProductVariant>();
        }

        public IDataResult<List<ProductVariantAttributeValueDto>> GetProductVariantCombination(int productId, int productVariantId)
        {
            var combinationData = _productVariantAttributeCombinationService.GetCombinationAttributeValue(productId, productVariantId);
            if (combinationData.Data != null)
            {
                if (combinationData.Data.Count() > 0)
                {
                    List<ProductVariantAttributeValueDto> productVariantAttributes = new List<ProductVariantAttributeValueDto>();

                    foreach (var data in combinationData.Data)
                    {
                        productVariantAttributes.Add(data);
                    }
                    return new SuccessDataResult<List<ProductVariantAttributeValueDto>>(productVariantAttributes);
                }
            }
            return new ErrorDataResult<List<ProductVariantAttributeValueDto>>();
        }

        public IDataResult<List<List<ProductVariantAttributeValueDto>>> GetAllProductVariantCombination(int productId)
        {
            var result = _productVariantAttributeCombinationService.GetAllCombinationAttributeValue(productId);
            if (result != null)
            {
                return new SuccessDataResult<List<List<ProductVariantAttributeValueDto>>>(result.Data);
            }
            return new ErrorDataResult<List<List<ProductVariantAttributeValueDto>>>();
        }

        public IDataResult<List<ProductVariant>> MapProductVariantCombination(int productId, int productVariantId)
        {
            var combinationData = _productVariantAttributeCombinationService.GetCombinationAttributeValue(productId, productVariantId);
            if (combinationData.Data != null)
            {
                if (combinationData.Data.Count() > 0)
                {
                    List<ProductVariant> productVariants = new List<ProductVariant>();

                    foreach (var data in combinationData.Data)
                    {
                        ProductVariant productVariant = new ProductVariant();
                        productVariant.Id = data.ProductVariantId;
                        productVariant.ProductId = data.ProductId;
                        productVariant.ParentId = data.ParentId;
                        productVariant.AttributeId = data.AttributeId;
                        productVariants.Add(productVariant);
                    }
                    return new SuccessDataResult<List<ProductVariant>>(productVariants);
                }
            }
            return new ErrorDataResult<List<ProductVariant>>();
        }
        //Urun detay sayfasinda ilgili ana varyantin default olarak ana ve alt varyantlarını listeliyor
        public IDataResult<List<ProductVariantGroupDetailDto>> GetDefaultProductVariantDetail(int productId, int parentId)
        {
            var result = _productVariantDal.GetAllProductDetailAttributeByProductId(productId);
            if (result != null)
            {
                var groupResult = result.GroupBy(x => x.AttributeName).ToList();
                List<ProductVariantGroupDetailDto> groupDetailList = new List<ProductVariantGroupDetailDto>();
                for (int i = 0; i < groupResult.Count(); i++)
                {
                    ProductVariantGroupDetailDto groupDetail = new ProductVariantGroupDetailDto();
                    groupDetail.AttributeId = groupResult[i].FirstOrDefault().AttributeId.Value;
                    groupDetail.ParentId = groupResult[i].FirstOrDefault().ParentId.Value;
                    groupDetail.AttributeName = groupResult[i].FirstOrDefault().AttributeName;
                    groupDetail.ProductVariantAttributeValueDtos = new List<ProductVariantAttributeValueDto>();
                    groupDetailList.Add(groupDetail);
                }

                for (int i = 0; i < groupDetailList.Count(); i++)
                {
                    if (groupDetailList[i].ParentId == 0)
                    {
                        var getAllProductVariant = _productVariantDal.GetAllProductDetailAttributeByProductIdParentId(productId, 0);
                        groupDetailList[i].ProductVariantAttributeValueDtos.AddRange(getAllProductVariant);
                        for (int j = 0; j < groupDetailList[i].ProductVariantAttributeValueDtos.Count(); j++)
                        {
                            groupDetailList[i].ProductVariantAttributeValueDtos[j].ImagePath = _productImageService.GetByProductVariantId(groupDetailList[i].ProductVariantAttributeValueDtos[j].ProductVariantId).Data.Path;
                        }
                    }

                    if (i > 0)
                    {
                        var data = _productVariantDal.GetAllProductDetailAttributeByProductIdParentId(productId, Convert.ToInt32(groupDetailList[i -1].ProductVariantAttributeValueDtos.FirstOrDefault().ProductVariantId));
                        groupDetailList[i].ProductVariantAttributeValueDtos.AddRange(data);
                    }

                    if (i == groupDetailList.Count() -1)
                    {
                        for (int j = 0; j < groupDetailList[i].ProductVariantAttributeValueDtos.Count(); j++)
                        {
                            var checkStock = _productStockService.GetByProductVariantId(groupDetailList[i].ProductVariantAttributeValueDtos[j].ProductVariantId);
                            if (checkStock.Data != null)
                            {
                                groupDetailList[i].ProductVariantAttributeValueDtos[j].Price = checkStock.Data.Price;
                                groupDetailList[i].ProductVariantAttributeValueDtos[j].Quantity = checkStock.Data.Quantity;
                            }
                        }
                    }

                    
                }
                return new SuccessDataResult<List<ProductVariantGroupDetailDto>>(groupDetailList);

            }
            return new ErrorDataResult<List<ProductVariantGroupDetailDto>>();

        }
        //Urun detay sayfasında secilen varyantın alt varyantlarini gostermek icin kullaniliyor
        public IDataResult<List<ProductVariantGroupDetailDto>> GetSubProductVariantDetail(List<ProductVariantGroupDetailDto> productVariantGroups, int productId, int parentId)
        {
            var result = _productVariantDal.GetAllProductDetailAttributeByParentId(parentId);

            if (result != null)
            {
                if (productVariantGroups.Count > 0)
                {
                    ProductVariant productVariant = null;
                    int keepParentId = 0;
                    for (int i = 0; i < productVariantGroups.Count(); i++)
                    {
                        if (productVariantGroups[i].ProductVariantAttributeValueDtos.Any(x => x.ProductVariantId == parentId))
                        {
                            productVariant = GetProductVariantByParentId(parentId).Data;
                        }
                        if (productVariant != null)
                        {
                            if (productVariantGroups[i].AttributeId == productVariant.AttributeId)
                            {
                                if (i == productVariantGroups.Count -1)
                                {
                                    keepParentId = productVariant.Id;
                                }
                                productVariant = GetProductVariantByParentId(productVariant.Id).Data;
                                productVariantGroups.Remove(productVariantGroups[i]);

                                i--;
                            }

                        }
                    }

                    if (parentId != 0)
                    {
                        for (int j = 0; j < productVariantGroups.Count; j++)
                        {
                            var data = _productVariantDal.GetAllSubProductAttributeDtoByParentId(parentId);
                            if (data != null)
                            {
                                if (data.Count > 0)
                                {
                                    ProductVariantGroupDetailDto productVariantGroupDetailDto = new ProductVariantGroupDetailDto();
                                    List<ProductVariantAttributeValueDto> productVariantAttributeValues = new List<ProductVariantAttributeValueDto>();
                                    productVariantGroupDetailDto.ProductVariantAttributeValueDtos = productVariantAttributeValues;

                                    ProductVariantAttributeValueDto productVariantAttributeValueDto = null;
                                    productVariantGroupDetailDto.AttributeId = data[0].AttributeId;
                                    productVariantGroupDetailDto.ParentId = data[0].ParentId.Value;
                                    productVariantGroupDetailDto.AttributeName = data[0].AttributeName;
                                    productVariantAttributeValueDto = data[0];
                                    productVariantGroupDetailDto.ProductVariantAttributeValueDtos.AddRange(data);
                                    productVariantGroups.Add(productVariantGroupDetailDto);
                                    parentId = data[0].ProductVariantId;
                                }
                            }
                            if (j == productVariantGroups.Count() -1)
                            {
                                for (int k = 0; k < productVariantGroups[j].ProductVariantAttributeValueDtos.Count(); k++)
                                {
                                    var checkStock = _productStockService.GetByProductVariantId(productVariantGroups[j].ProductVariantAttributeValueDtos[k].ProductVariantId);
                                    if (checkStock.Data != null)
                                    {
                                        productVariantGroups[j].ProductVariantAttributeValueDtos[k].Price = checkStock.Data.Price;
                                        productVariantGroups[j].ProductVariantAttributeValueDtos[k].Quantity = checkStock.Data.Quantity;
                                    }
                                }
                            }
                        }
                    }

                   

                }
                return new SuccessDataResult<List<ProductVariantGroupDetailDto>>(productVariantGroups);
            }
            return new ErrorDataResult<List<ProductVariantGroupDetailDto>>();
        }

        public IDataResult<ProductVariant> GetProductVariantByParentId(int parentId)
        {
            var result = _productVariantDal.Get(x => x.ParentId == parentId);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>(result);
            }
            return new ErrorDataResult<ProductVariant>();
        }

        public IDataResult<ProductVariant> MainVariantEndVariant(int productVariantId)
        {
            var result = GetProductVariantByParentId(productVariantId);
            if (result != null)
            {
                List<ProductVariant> productVariants = new List<ProductVariant>();

                productVariants.Add(result.Data);
                for (int i = 0; i < productVariants.Count; i++)
                {
                    var productVariant = GetProductVariantByParentId(productVariants[i].Id).Data;
                    if (productVariant != null)
                    {
                        productVariants.Add(productVariant);
                    }
                    else
                    {
                        return new SuccessDataResult<ProductVariant>(productVariants[i]);
                    }

                }
            }
            return new ErrorDataResult<ProductVariant>();

        }
    }
}

