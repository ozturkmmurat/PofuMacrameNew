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
        IProductAttributeService _productAttributeService;
        ICategoryAttributeService _categoryAttributeService;
        IProductVariantAttributeCombinationService _productVariantAttributeCombinationService;
        IAttributeValueService _attributeValueService;
        public ProductVariantManager(
            IProductVariantDal productVariantDal,
            IProductStockService productStockService,
            IProductAttributeService productAttributeService,
            ICategoryAttributeService categoryAttributeService,
            IProductVariantAttributeCombinationService productVariantAttributeCombinationService,
            IAttributeValueService attributeValueService)
        {
            _productVariantDal = productVariantDal;
            _productStockService = productStockService;
            _productAttributeService=productAttributeService;
            _categoryAttributeService=categoryAttributeService;
            _productVariantAttributeCombinationService = productVariantAttributeCombinationService;
            _attributeValueService = attributeValueService;
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
            int keepId = 0;
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
                                if (GetByParentIdAttrValueId(addProductVariant.ProductId, productVariant.ParentId, attributeValue, attributeId).Success == false)
                                {
                                    addProductVariant.ParentId = 0;
                                    productVariant.ProductId = addProductVariant.ProductId;
                                    productVariant.AttributeId = attributeId;
                                    productVariant.AttributeValueId = attributeValue;
                                    Add(productVariant);
                                    addProductVariant.ParentId = productVariant.Id;
                                    keepId = productVariant.Id;
                                }
                                else
                                {
                                    addProductVariant.ParentId = keepId;
                                }
                            }


                            // ProductVariant'ı veritabanına ekleyin
                            if (categoryAttributeResult.Attribute == true)
                            {
                                if (GetByParentIdAttrValueId(addProductVariant.ProductId, addProductVariant.ParentId, attributeValue, attributeId).Success == false)
                                {
                                    productVariant.ProductId = addProductVariant.ProductId;
                                    productVariant.AttributeId = attributeId;
                                    productVariant.AttributeValueId = attributeValue;
                                    productVariant.ParentId = addProductVariant.ParentId;
                                    Add(productVariant);
                                    addProductVariant.ParentId =  productVariant.Id;
                                }
                                else
                                {
                                    addProductVariant.ParentId =addProductVariant.ParentId;
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

        public IDataResult<ProductVariant> GetByParentIdAttrValueId(int productId,int? parentId , int? attributeValueId, int? attributeId)
        {
            var result = _productVariantDal.Get(x => x.ProductId == productId && x.ParentId == parentId  && x.AttributeValueId == attributeValueId && x.AttributeId == attributeId);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>(result);
            }
            return new ErrorDataResult<ProductVariant>();
        }

        public IDataResult<List<ProductVariantAttributeValueDto>> GetProductVariantCombination(int productId, int attributeValueId)
        {
            var combinationData = _productVariantAttributeCombinationService.GetCombinationAttributeValue(productId, attributeValueId);
            if (combinationData.Data != null)
            {
                if (combinationData.Data.Count() > 0)
                {
                    List<ProductVariantAttributeValueDto> productVariantAttributes = new List<ProductVariantAttributeValueDto>();

                    foreach (var data in combinationData.Data)
                    {
                        foreach (var item in data)
                        {
                            productVariantAttributes.Add(item);
                        }
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

        public IDataResult<List<ProductVariant>> MapProductVariantCombination(int productId, int attributeValueId)
        {
            var combinationData = _productVariantAttributeCombinationService.GetCombinationAttributeValue(productId, attributeValueId);
            if (combinationData.Data != null)
            {
                if (combinationData.Data.Count() > 0)
                {
                    List<ProductVariant> productVariants = new List<ProductVariant>();

                    foreach (var data in combinationData.Data)
                    {
                        foreach (var item in data)
                        {
                            ProductVariant productVariant = new ProductVariant();
                            productVariant.Id = item.ProductVariantId;
                            productVariant.ProductId = item.ProductId;
                            productVariant.ParentId = item.ParentId;
                            productVariant.AttributeId = item.AttributeId;
                            productVariants.Add(productVariant);
                        }
                    }
                    return new SuccessDataResult<List<ProductVariant>>(productVariants);
                }
            }
            return new ErrorDataResult<List<ProductVariant>>();
        }

        public IDataResult<List<ProductVariantDetailAttributeDto>> GetProductVariantDetail(int productId, int productVariantId)
        {
            var dtoResult = _productVariantDal.GetProductDetailAttribute(productId);
            List<ProductVariantAttributeValueDto> productVariantAttrs = new List<ProductVariantAttributeValueDto>();

            // Gruplama işlemi
            var groupedDtoResult = dtoResult.GroupBy(x => x.AttributeName).ToList();

            foreach (var group in groupedDtoResult)
            {
                // Gruplanmış sonuçları işle
                foreach (var item in group)
                {
                    if (item.ParentId == 0)
                    {
                        item.AttributeValues.Add(_attributeValueService.GetById(item.AttributeValueId.Value).Data);
                    }
                }

                // İsterseniz gruplanmış sonuçları kullanarak işlem yapabilirsiniz
                if (productVariantId == 0)
                {
                    var firstItem = group.FirstOrDefault(x => x.ParentId == 0);
                    if (firstItem != null)
                    {
                        productVariantAttrs = GetProductVariantCombination(productId, firstItem.AttributeValueId.Value).Data;
                    }
                }
                else
                {
                    var test = GetProductVariantCombination(3225,1014).Data;
                }
            }
            var resultToReturn = groupedDtoResult.SelectMany(group => group).ToList();

            return new SuccessDataResult<List<ProductVariantDetailAttributeDto>>(resultToReturn);


        }
    }
}
