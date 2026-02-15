using Business.Abstract.ProductVariants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Dtos.ProductVariant.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete.ProductVariants
{
    public class ProductVariantAttributeCombinationManager : IProductVariantAttributeCombinationService
    {
        IProductVariantDal _productVariantDal;
        public ProductVariantAttributeCombinationManager(IProductVariantDal productVariantDal)
        {
            _productVariantDal = productVariantDal;
        }
        public IDataResult<List<List<ProductVariantAttributeValueDto>>> GetAllCombinationAttributeValue(int productId)
        {
            var result = _productVariantDal.GetAllSubProductAttributeDtoProductId(productId);
            if (result != null && result.Any())
            {
                List<List<ProductVariantAttributeValueDto>> allCombinations = new List<List<ProductVariantAttributeValueDto>>();

                // Ana grupları bulun
                var mainGroups = result.Where(va => va.ParentId == 0).ToList();

                foreach (var mainGroup in mainGroups)
                {
                    List<List<ProductVariantAttributeValueDto>> combinations = new List<List<ProductVariantAttributeValueDto>>();

                    // Başlangıç üst grubunu bulun
                    var startGroup = result.FirstOrDefault(v => v.ParentId == mainGroup.ParentId && v.AttributeValueId == mainGroup.AttributeValueId);

                    if (startGroup != null)
                    {
                        // Başlangıç üst grubu için kombinasyonları oluşturun
                        List<ProductVariantAttributeValueDto> combination = new List<ProductVariantAttributeValueDto> { startGroup };
                        GenerateManySubCombinations(result, combination, Convert.ToInt32(startGroup.ProductVariantId), combinations);
                    }

                    allCombinations.AddRange(combinations);
                }

                return new SuccessDataResult<List<List<ProductVariantAttributeValueDto>>>(allCombinations);
            }
            return new ErrorDataResult<List<List<ProductVariantAttributeValueDto>>>();

        }

        public void GenerateManySubCombinations(List<ProductVariantAttributeValueDto> variants, List<ProductVariantAttributeValueDto> combination, int parentId, List<List<ProductVariantAttributeValueDto>> combinations)
        {
            // Alt grupları bulun (Belirli bir ParentId'ye sahip olanlar)
            var subGroups = variants.Where(v => v.ParentId == parentId).ToList();

            if (subGroups.Count == 0)
            {
                // Bu bir kombinasyonu tamamladık, listeye ekleyin
                combinations.Add(new List<ProductVariantAttributeValueDto>(combination));
                return;
            }

            foreach (var subGroup in subGroups)
            {
                // Alt grup için kombinasyonları oluşturun
                combination.Add(subGroup);
                GenerateManySubCombinations(variants, combination, Convert.ToInt32(subGroup.ProductVariantId), combinations);
                combination.RemoveAt(combination.Count - 1); // Kombinasyonu geri al
            }
        }
        public IDataResult<List<ProductVariantAttributeValueDto>> GetCombinationAttributeValue(int productId, int productVariantId)
        {
            var result = _productVariantDal.GetAllSubProductAttributeDtoProductId(productId);
            if (result != null)
            {
                List<ProductVariantAttributeValueDto> combination = new List<ProductVariantAttributeValueDto>();
                var startGroup = result.FirstOrDefault(va => va.ProductVariantId == productVariantId);

                if (startGroup != null)
                {
                    GenerateSubCombinations(result, combination, startGroup.ParentId);
                }

                return new SuccessDataResult<List<ProductVariantAttributeValueDto>>(combination);
            }
            return new ErrorDataResult<List<ProductVariantAttributeValueDto>>("Belirtilen ürün için kombinasyonlar bulunamadı.");
        }

        public IDataResult<List<ProductVariantAttributeValueDto>> GetAllEndCombinationAttributeValue(int productId)
        {
            var result = GetAllCombinationAttributeValue(productId).Data;
            if (result != null)
            {
                if (result.Count() > 0)
                {
                    List<ProductVariantAttributeValueDto> productVariantAttrList = new List<ProductVariantAttributeValueDto>();
                    foreach (var items in result)
                    {
                        ProductVariantAttributeValueDto productVariantAttributeValueDto = new ProductVariantAttributeValueDto();
                        foreach (var item in items)
                        {
                            if (item != items.Last())
                            {
                                productVariantAttributeValueDto.AttributeValue += item.AttributeName + ": " + item.AttributeValue + " - ";
                            }
                            if (item == items.First() && item.ParentId == 0)
                            {
                                productVariantAttributeValueDto.ProductVariantId = item.ProductVariantId;
                            }
                            if (item == items.Last())
                            {
                                productVariantAttributeValueDto.EndProductVariantId = item.ProductVariantId;
                                productVariantAttributeValueDto.ProductId = item.ProductId;
                                productVariantAttributeValueDto.ParentId = item.ParentId;
                                productVariantAttributeValueDto.AttributeValue += item.AttributeName + ": " + item.AttributeValue;
                                productVariantAttrList.Add(productVariantAttributeValueDto);
                            }
                        }
                    }
                    return new SuccessDataResult<List<ProductVariantAttributeValueDto>>(productVariantAttrList);
                }
            }
            return new ErrorDataResult<List<ProductVariantAttributeValueDto>>();
        }

        public IDataResult<List<ProductVariantAttributeValueDto>> GetEndCombinationAttributeValue(int productId, int productVariantId)
        {
            var result = GetCombinationAttributeValue(productId, productVariantId).Data;
            if (result != null)
            {
                if (result.Count() > 0)
                {
                    List<ProductVariantAttributeValueDto> productVariantAttrList = new List<ProductVariantAttributeValueDto>();
                        foreach (var item in result)
                        {
                            ProductVariantAttributeValueDto productVariantAttributeValueDto = new ProductVariantAttributeValueDto();

                            if (item != result.Last())
                            {
                                productVariantAttributeValueDto.AttributeValue += item.AttributeName + ": " + item.AttributeValue + " - ";
                            }
                            if (item == result.First() && item.ParentId == 0)
                            {
                                productVariantAttributeValueDto.ProductVariantId = item.ProductVariantId;
                            }
                            if (item == result.Last())
                            {
                                productVariantAttributeValueDto.EndProductVariantId = item.ProductVariantId;
                                productVariantAttributeValueDto.ProductId = item.ProductId;
                                productVariantAttributeValueDto.ParentId = item.ParentId;
                                productVariantAttributeValueDto.AttributeValue += item.AttributeName + ": " + item.AttributeValue;
                                productVariantAttrList.Add(productVariantAttributeValueDto);
                            }
                    }
                    return new SuccessDataResult<List<ProductVariantAttributeValueDto>>(productVariantAttrList);
                }
            }
            return new ErrorDataResult<List<ProductVariantAttributeValueDto>>();
        }

        public void GenerateSubCombinations(List<ProductVariantAttributeValueDto> variants, List<ProductVariantAttributeValueDto> combination, int parentId)
        {
            // Alt grupları bulun (Belirli bir ParentId'ye sahip olanlar)
            var subGroups = variants.Where(v => v.ParentId == parentId).ToList();

            if (subGroups.Count == 0)
            {
                // Bu bir kombinasyonu tamamladık, listeye ekleyin
                return;
            }

            foreach (var subGroup in subGroups)
            {
               
                // Alt grup için kombinasyonları oluşturun
                combination.Add(subGroup);
                GenerateSubCombinations(variants, combination, Convert.ToInt32(subGroup.ProductVariantId));
            }
        }
    }
}
