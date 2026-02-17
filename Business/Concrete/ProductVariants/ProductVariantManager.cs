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
using Entities.Dtos.Product.Select;
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
        IAttributeService _attributeService;
        IAttributeValueService _attributeValueService;
        public ProductVariantManager(
            IProductVariantDal productVariantDal,
            IProductStockService productStockService,
            IProductAttributeService productAttributeService,
            ICategoryAttributeService categoryAttributeService,
            IProductVariantAttributeCombinationService productVariantAttributeCombinationService,
            IProductImageService productImageService,
            IAttributeService attributeService,
            IAttributeValueService attributeValueService
            )
        {
            _productVariantDal = productVariantDal;
            _productStockService = productStockService;
            _categoryAttributeService=categoryAttributeService;
            _productVariantAttributeCombinationService = productVariantAttributeCombinationService;
            _productImageService=productImageService;
            _productAttributeService = productAttributeService;
            _attributeService = attributeService;
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

            if (jsonData.Count > 0 && addProductVariant.IsVariant)
            {
                List<ProductStock> productStocks = new List<ProductStock>();
                int productStockIndex = 0;
                addProductVariant.ProductVariants = new List<ProductVariant>();

                // Her kombinasyon = bir yol (path). ParentId=0 ana varyant, diğerleri alt varyant zinciri.
                foreach (var item in jsonData)
                {
                    var path = item.Value; // örn. [{2003:1014}, {2004:1016}, {2007:2022}]
                    if (path == null || !path.Any()) continue;

                    int currentParentId = 0; // yolun başında her zaman kök (ana varyant)

                    foreach (var step in path)
                    {
                        foreach (var kvp in step)
                        {
                            var attributeId = Convert.ToInt32(kvp.Key);
                            var attributeValue = kvp.Value;

                            var checkData = GetByParentIdAttrValueId(addProductVariant.ProductId, currentParentId, attributeValue, attributeId);
                            if (checkData.Success)
                            {
                                currentParentId = checkData.Data.Id;
                            }
                            else
                            {
                                var newVariant = new ProductVariant
                                {
                                    ProductId = addProductVariant.ProductId,
                                    AttributeId = attributeId,
                                    AttributeValueId = attributeValue,
                                    ParentId = currentParentId
                                };
                                Add(newVariant);
                                currentParentId = newVariant.Id;
                            }
                            break; // her step'ta tek key-value var
                        }
                    }

                    foreach (var attribute in addProductVariant.ProductAttributes)
                    {
                        attribute.ProductId = addProductVariant.ProductId;
                    }

                    _productAttributeService.AddRange(addProductVariant.ProductAttributes);

                    // Bu kombinasyonun son düğümü (yaprak) = stok bağlanacak varyant
                    if (productStockIndex < addProductVariant.ProductStocks.Count)
                    {
                        productStocks.Add(new ProductStock
                        {
                            ProductId = addProductVariant.ProductId,
                            ProductVariantId = currentParentId,
                            Price = addProductVariant.ProductStocks[productStockIndex].Price,
                            Kdv = addProductVariant.ProductStocks[productStockIndex].Kdv,
                            NetPrice = addProductVariant.ProductStocks[productStockIndex].NetPrice,
                            Quantity = addProductVariant.ProductStocks[productStockIndex].Quantity,
                            StockCode = addProductVariant.ProductStocks[productStockIndex].StockCode,
                        });
                        productStockIndex++;
                    }
                }

                _productStockService.AddList(productStocks);
                return new SuccessResult();
            }
            else if(jsonData.Count == 0 && !addProductVariant.IsVariant) //Eger urunun bir varyanti yok ise burasi calisacak
            {
                ProductVariant productVariant = new ProductVariant()
                {
                    ProductId = addProductVariant.ProductId,
                    AttributeId = 0,
                    AttributeValueId = 0,
                    ParentId = 0
                };
                var addProductVariantResult = Add(productVariant);

                if (addProductVariantResult.Success)
                {
                    ProductStock productStock = new ProductStock()
                    {
                        ProductId= addProductVariant.ProductId,
                        ProductVariantId = productVariant.Id,
                        Price = addProductVariant.ProductStocks[0].Price,
                        Quantity = addProductVariant.ProductStocks[0].Quantity,
                        Kdv = addProductVariant.ProductStocks[0].Kdv,
                        NetPrice = addProductVariant.ProductStocks[0].NetPrice,
                        StockCode = addProductVariant.ProductStocks[0].StockCode
                    };

                    var addProductStockResult = _productStockService.Add(productStock);

                    if (addProductStockResult.Success)
                    {
                        return new SuccessResult();
                    }
                    return new ErrorResult();
                }
            }



            // Diğer işlemleri burada gerçekleştirin

            return new ErrorResult();
        }

        public IResult Delete(ProductVariant variant)
        {
            if (variant != null)
            {
                if (variant.AttributeValueId != 0)
                {
                    var getSubProductVariant = MapProductVariantCombination(variant.ProductId, variant.AttributeValueId);
                    if (getSubProductVariant.Data != null && getSubProductVariant.Data.Any())
                    {
                        _productVariantDal.DeleteRange(getSubProductVariant.Data);
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
            var result = _productVariantDal.GetAll(x => x.ProductId == productId && (attributeId == null || x.AttributeId == attributeId.Value));
            if (result != null)
            {
                return new SuccessDataResult<List<ProductVariant>>(result);
            }
            return new ErrorDataResult<List<ProductVariant>>();
        }

        public IDataResult<ProductVariant> GetByParentIdAttrValueId(int productId, int? parentId, int? attributeValueId, int? attributeId)
        {
            var result = _productVariantDal.Get(x => x.ProductId == productId && (parentId == null || x.ParentId == parentId.Value) && (attributeValueId == null || x.AttributeValueId == attributeValueId.Value) && (attributeId == null || x.AttributeId == attributeId.Value));
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
                        productVariant.AttributeValueId = data.AttributeValueId;
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
                var groupResult = result.Where(x => (x.AttributeId ?? 0) != 0).GroupBy(x => x.AttributeName).ToList();
                List<ProductVariantGroupDetailDto> groupDetailList = new List<ProductVariantGroupDetailDto>();
                for (int i = 0; i < groupResult.Count(); i++)
                {
                    var first = groupResult[i].FirstOrDefault();
                    if (first == null || (first.AttributeId ?? 0) == 0) continue;
                    ProductVariantGroupDetailDto groupDetail = new ProductVariantGroupDetailDto();
                    groupDetail.AttributeId = first.AttributeId ?? 0;
                    groupDetail.ParentId = first.ParentId ?? 0;
                    groupDetail.AttributeName = first.AttributeName;
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
                            var imageResult = _productImageService.GetByProductVariantId(groupDetailList[i].ProductVariantAttributeValueDtos[j].ProductVariantId);
                            if (imageResult.Success)
                            {
                                groupDetailList[i].ProductVariantAttributeValueDtos[j].ImagePath = imageResult.Data.Path;
                            }
                        }
                    }

                    if (i > 0)
                    {
                        var firstDto = groupDetailList[i - 1].ProductVariantAttributeValueDtos.FirstOrDefault();
                        if (firstDto != null)
                        {
                            var data = _productVariantDal.GetAllProductDetailAttributeByProductIdParentId(productId, firstDto.ProductVariantId);
                            groupDetailList[i].ProductVariantAttributeValueDtos.AddRange(data);
                        }
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
                                groupDetailList[i].ProductVariantAttributeValueDtos[j].Kdv = checkStock.Data.Kdv;
                                groupDetailList[i].ProductVariantAttributeValueDtos[j].NetPrice = checkStock.Data.NetPrice;
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
                            productVariant = GetProductVariantByParentIdNT(parentId).Data;
                        }
                        if (productVariant != null && productVariant.AttributeId != 0)
                        {
                            if (productVariantGroups[i].AttributeId == productVariant.AttributeId)
                            {
                                if (i == productVariantGroups.Count -1)
                                {
                                    keepParentId = productVariant.Id;
                                }
                                productVariant = GetProductVariantByParentIdNT(productVariant.Id).Data;
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
                                    var firstData = data[0];
                                    if (firstData.AttributeId == 0) { j++; continue; }
                                    ProductVariantGroupDetailDto productVariantGroupDetailDto = new ProductVariantGroupDetailDto();
                                    List<ProductVariantAttributeValueDto> productVariantAttributeValues = new List<ProductVariantAttributeValueDto>();
                                    productVariantGroupDetailDto.ProductVariantAttributeValueDtos = productVariantAttributeValues;

                                    ProductVariantAttributeValueDto productVariantAttributeValueDto = null;
                                    productVariantGroupDetailDto.AttributeId = firstData.AttributeId;
                                    productVariantGroupDetailDto.ParentId = firstData.ParentId;
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
                                        productVariantGroups[j].ProductVariantAttributeValueDtos[k].Kdv = checkStock.Data.Kdv;
                                        productVariantGroups[j].ProductVariantAttributeValueDtos[k].NetPrice = checkStock.Data.NetPrice;
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

        public IDataResult<ProductVariant> GetProductVariantByParentIdNT(int parentId)
        {
            var result = _productVariantDal.GetAsNoTracking(x => x.ParentId == parentId);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>(result);
            }
            return new ErrorDataResult<ProductVariant>();
        }

        //Bir ana varyantın id bilgisine göre o varyantın en sonuncu varyantını buluyor.
        public IDataResult<ProductVariant> MainVariantEndVariantNT(int productVariantId)
        {
            var result = GetProductVariantByParentIdNT(productVariantId);
            if (result != null)
            {
                if (result.Data == null)
                {
                    result = GetByIdNT(productVariantId);
                }
                List<ProductVariant> productVariants = new List<ProductVariant>();

                productVariants.Add(result.Data);
                for (int i = 0; i < productVariants.Count; i++)
                {
                    var productVariant = GetProductVariantByParentIdNT(productVariants[i].Id).Data;
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
        //Sonuncu varyanttan ana varyantı buluyor
        public IDataResult<ProductVariant> EndVariantMainVariantNT(int parentId)
        {
            var result = GetProductVariantByParentIdNT(parentId);
            if (result.Data != null)
            {
                List<ProductVariant> productVariants = new List<ProductVariant>();

                productVariants.Add(result.Data);
                for (int i = 0; i < productVariants.Count; i++)
                {
                    if (productVariants[i].ParentId == 0)
                        return new SuccessDataResult<ProductVariant>(productVariants[i]);
                    var productVariant = GetByIdNT(productVariants[i].ParentId).Data;
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

        //ParentId den baslayıp ilgili varyantın ozelliklerini buluyor.
        public IDataResult<ProductVariantAttributeDto> GetProductVariantAttribute(int parentId)
        {
            var result = GetProductVariantByParentIdNT(parentId);
            if (result.Data != null)
            {
                List<ProductVariant> productVariants = new List<ProductVariant>();
                ProductVariantAttributeDto productVariantAttributeDto = new ProductVariantAttributeDto();

                productVariants.Add(result.Data);

                for (int i = 0; i < productVariants.Count; i++)
                {
                    if (productVariants[i].ParentId == 0)
                    {
                        productVariantAttributeDto.VariantId = productVariants[i].Id;
                        return new SuccessDataResult<ProductVariantAttributeDto>(productVariantAttributeDto);
                    }
                    var productVariant = GetById(productVariants[i].ParentId).Data;
                    if (productVariant != null)
                    {
                        if (productVariants[i].AttributeId != 0 && productVariants[i].AttributeValueId != 0)
                        {
                            var attrResult = _attributeService.GetById(productVariants[i].AttributeId);
                            var attrValResult = _attributeValueService.GetById(productVariants[i].AttributeValueId);
                            if (attrResult.Success && attrResult.Data != null && attrValResult.Success && attrValResult.Data != null)
                                productVariantAttributeDto.Attribute += attrResult.Data.Name + ": " + attrValResult.Data.Value;
                        }
                        productVariants.Add(productVariant);
                    }
                    else
                    {
                        productVariantAttributeDto.VariantId = productVariants[i].Id;
                        return new SuccessDataResult<ProductVariantAttributeDto>(productVariantAttributeDto);
                    }

                }
            }
            return new ErrorDataResult<ProductVariantAttributeDto>();

        }

        public IDataResult<ProductVariant> GetByIdNT(int id)
        {
            var result = _productVariantDal.GetAsNoTracking(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>(result);
            }
            return new ErrorDataResult<ProductVariant>();
        }

        //Sonuncu varyanttan ana varyanta gidip urun fotografini getiriyor
        public IDataResult<List<string>> ProductVariantImage(int propductVariantId)
        {
            if (propductVariantId > 0)
            {
                var productImages = _productImageService.GetFirstTwoPathNT(propductVariantId);
                return new SuccessDataResult<List<string>>(productImages.Data);
            }
            return new ErrorDataResult<List<string>>();
        }

        public IDataResult<SelectListProductVariantDto> DtoEndVariantMainVariantNT(int parentId)
        {

            var result = GetProductVariantByParentIdNT(parentId);
            if (result.Data != null && parentId > 0)
            {
                List<SelectListProductVariantDto> productVariants = new List<SelectListProductVariantDto>();
                List<AttributeValue> attributeValues = new List<AttributeValue>();

                SelectListProductVariantDto productVariantDto = new SelectListProductVariantDto();
                productVariantDto.ProductVariantId = result.Data.Id;
                productVariantDto.ParentId  = result.Data.ParentId;
                productVariantDto.ProductId = result.Data.ProductId;

                if (result.Data.AttributeValueId != 0 && result.Data.AttributeId != 0)
                {
                    AttributeValue firstAttributeValue = new AttributeValue();
                    firstAttributeValue.Id = result.Data.AttributeValueId;
                    firstAttributeValue.AttributeId = result.Data.AttributeId;
                    attributeValues.Add(firstAttributeValue);
                }

                productVariants.Add(productVariantDto);

                for (int i = 0; i < productVariants.Count; i++)
                {
                    if ((productVariants[i].ParentId ?? 0) == 0)
                        return new ErrorDataResult<SelectListProductVariantDto>();
                    var productVariant = GetByIdNT(productVariants[i].ParentId ?? 0).Data;
                    if (productVariant != null && productVariant.ParentId != 0)
                    {
                        SelectListProductVariantDto data = new SelectListProductVariantDto();
                        data.ProductVariantId = productVariant.Id;
                        data.ParentId = productVariant.ParentId;
                        data.ProductId = productVariant.ProductId;

                        productVariants.Add(data);

                        if (productVariant.AttributeValueId != 0 && productVariant.AttributeId != 0)
                        {
                            AttributeValue attributeValue = new AttributeValue();
                            attributeValue.Id = productVariant.AttributeValueId;
                            attributeValue.AttributeId = productVariant.AttributeId;
                            attributeValues.Add(attributeValue);
                        }
                    }
                    else
                    {
                        if (productVariant != null && productVariant.AttributeValueId != 0 && productVariant.AttributeId != 0)
                        {
                            AttributeValue attributeValue = new AttributeValue();
                            attributeValue.Id = productVariant.AttributeValueId;
                            attributeValue.AttributeId = productVariant.AttributeId;
                            attributeValues.Add(attributeValue);
                        }
                        productVariants[i].AttributeValues = attributeValues;
                        return new SuccessDataResult<SelectListProductVariantDto>(productVariants[i]);
                    }

                }
            }
            return new ErrorDataResult<SelectListProductVariantDto>();
        }

        public IDataResult<SelectListProductVariantDto> DtoMainVariantEndVariantNT(int productVariantId)
        {
            var result = GetByIdNT(productVariantId);
            if (result.Data != null && productVariantId > 0)
            {
                List<SelectListProductVariantDto> productVariants = new List<SelectListProductVariantDto>();
                List<AttributeValue> attributeValues = new List<AttributeValue>();

                SelectListProductVariantDto productVariantDto = new SelectListProductVariantDto();
                productVariantDto.ProductVariantId = result.Data.Id;
                productVariantDto.ParentId  = result.Data.ParentId;
                productVariantDto.ProductId = result.Data.ProductId;

                if (result.Data.AttributeValueId != 0 && result.Data.AttributeId != 0)
                {
                    AttributeValue firstAttributeValue = new AttributeValue();
                    firstAttributeValue.Id = result.Data.AttributeValueId;
                    firstAttributeValue.AttributeId = result.Data.AttributeId;
                    attributeValues.Add(firstAttributeValue);
                }

                productVariants.Add(productVariantDto);

                for (int i = 0; i < productVariants.Count; i++)
                {
                    var productVariant = GetByParentIdNT(productVariants[i].ProductVariantId).Data;
                    if (productVariant != null && productVariant.ParentId == 0)
                    {
                        SelectListProductVariantDto data = new SelectListProductVariantDto();
                        data.ProductVariantId = productVariant.Id;
                        data.ParentId = productVariant.ParentId;
                        data.ProductId = productVariant.ProductId;

                        productVariants.Add(data);

                        if (productVariant.AttributeValueId != 0 && productVariant.AttributeId != 0)
                        {
                            AttributeValue attributeValue = new AttributeValue();
                            attributeValue.Id = productVariant.AttributeValueId;
                            attributeValue.AttributeId = productVariant.AttributeId;
                            attributeValues.Add(attributeValue);
                        }
                    }
                    else if(productVariant != null && productVariant.ParentId > 0)
                    {
                        SelectListProductVariantDto data = new SelectListProductVariantDto();
                        data.ProductVariantId = productVariant.Id;
                        data.ParentId = productVariant.ParentId;
                        data.ProductId = productVariant.ProductId;

                        productVariants.Add(data);

                        if (productVariant.AttributeValueId != 0 && productVariant.AttributeId != 0)
                        {
                            AttributeValue attributeValue = new AttributeValue();
                            attributeValue.Id = productVariant.AttributeValueId;
                            attributeValue.AttributeId = productVariant.AttributeId;
                            attributeValues.Add(attributeValue);
                        }
                        productVariants[0].AttributeValues = attributeValues;
                    }
                    else if(productVariant == null)
                    {
                        productVariants[0].AttributeValues = attributeValues;
                        return new SuccessDataResult<SelectListProductVariantDto>(productVariants[0]);
                    }

                }
            }
            return new ErrorDataResult<SelectListProductVariantDto>();
        }

        public IDataResult<ProductVariant> GetByParentIdNT(int parentId)
        {
            var result = _productVariantDal.GetAsNoTracking(x => x.ParentId == parentId);
            if (result != null)
            {
                return new SuccessDataResult<ProductVariant>(result);
            }
            return new ErrorDataResult<ProductVariant>();
        }
    }
}


