using Business.Abstract;
using Business.Abstract.ProductVariants;
using Business.BusinessAspects.Autofac;
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
using Iyzipay.Model;
using System;
using System.Collections;
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
        ICategoryAttributeService _categoryAttributeService;
        IProductAttributeService _productAttributeService;
        public ProductManager(
            IProductDal productDal,
            IProductStockService productStockService,
            IProductVariantService productVariantService,
            ICategoryAttributeService categoryAttributeService,
            IProductAttributeService productAttributeService)
        {
            _productDal = productDal;
            _productVariantService = productVariantService;
            _productStockService = productStockService;
            _categoryAttributeService=categoryAttributeService;
            _productAttributeService  = productAttributeService;
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
            if (filterProduct != null & filterProduct.Attributes.Count > 0 && filterProduct.CategoryId > 0)
            {
                var applyFiltered = _productDal.ApplyFilteres(filterProduct);
                List<SelectListProductVariantDto> filteredProductVariantDto = new List<SelectListProductVariantDto>();

                if (applyFiltered?.Item1?.Count > 0)
                {
                    for (int i = 0; i < applyFiltered.Item1.Count; i++)
                    {
                        if (applyFiltered.Item1[i].ParentId > 0)
                        {
                            filteredProductVariantDto.Add(_productVariantService.DtoEndVariantMainVariantNT(applyFiltered.Item1[i].ParentId.Value).Data);
                        }
                        else if (applyFiltered.Item1[i].ParentId == 0)
                        {
                            filteredProductVariantDto.Add(_productVariantService.DtoMainVariantEndVariantNT(applyFiltered.Item1[i].Id).Data);

                        }
                    }

                    var resultFiltersEdit = filteredProductVariantDto.GroupBy(x => x.ProductVariantId).Select(group => group.OrderByDescending(item => item.AttributeValues.Count()).First()).ToList(); //ProductVariantId gore grupluyor ama attributeValues en fazla olanlari aliyor
                    var resultFilters = _productDal.ExecuteFilteres(resultFiltersEdit);
                    var processProductVariant = ProcessProductVariantData(resultFilters);

                    if (processProductVariant.Success)
                    {
                        var categoryAttributes = _categoryAttributeService.GetAllByCategoryId(filterProduct.CategoryId).Data;
                        var slicerAttributeValues = categoryAttributes.Where(x => x.Slicer && x.Attribute == false);
                        var attributeAttributeValues = categoryAttributes.Where(x => x.Slicer == false && x.Attribute);
                        var falseAttributeValues = categoryAttributes.Where(x => x.Slicer == false && x.Attribute == false);
                        if (slicerAttributeValues != null)
                        {
                            var slicerIds = filterProduct.Attributes.Where(x => slicerAttributeValues.Any(y => x.Id ==  y.AttributeId)).SelectMany(x => x.ValueId).ToList();
                            var attributeIds = filterProduct.Attributes.Where(x => attributeAttributeValues.Any(y => x.Id ==  y.AttributeId)).SelectMany(x => x.ValueId).ToList();
                            var falseAttributeIds = filterProduct.Attributes.Where(x => falseAttributeValues.Any(y => x.Id ==  y.AttributeId)).SelectMany(x => x.ValueId).ToList();
                            if (applyFiltered.Item1 != null)  //Eger  duz attribute secilmemis ise calisir
                            {
                                if (slicerIds?.Count > 0)
                                {
                                    processProductVariant.Data.Where(x => x.AttributeValues.Any(y => slicerIds.Contains(y.Id))).ToList();

                                }

                                var endFilters = processProductVariant.Data
                                                    .GroupBy(x => x.ProductVariantId)
                                                    .Select(group =>
                                                    {
                                                        var maxAttributeValueItem = group.OrderByDescending(item => item.AttributeValues.Count()).FirstOrDefault();
                                                        return maxAttributeValueItem;
                                                    })
                                    .ToList();

                                if (applyFiltered.Item2 != null)
                                {
                                    foreach (var item in endFilters)
                                    {
                                        var matchingExtraInfo = applyFiltered.Item2.Where(x => x.ProductId == item.ProductId);
                                        if (matchingExtraInfo != null && matchingExtraInfo.Any()) // Check if there are any matching items
                                        {
                                            var attributeValues = matchingExtraInfo.Select(x => new AttributeValue { Id = x.AttributeValueId, AttributeId = x.AttributeId });
                                            item.AttributeValues.AddRange(attributeValues);
                                        }
                                    }
                                }

                                var filterProductAttributeIds = filterProduct.Attributes.Select(x => x.Id).ToList();

                                endFilters.RemoveAll(x => !filterProductAttributeIds.All(y => x.AttributeValues.Any(t => t.AttributeId == y)));

                                for (int i = endFilters.Count - 1; i >= 0; i--)
                                {
                                    var dto = endFilters[i];
                                    bool shouldRemove = dto.AttributeValues.Any(av => filterProduct.Attributes.Any(fa => fa.Id == av.AttributeId && !fa.ValueId.Contains(av.Id)));
                                    if (shouldRemove)
                                    {
                                        endFilters.RemoveAt(i);
                                    }
                                }

                                if (endFilters.Count > 0)
                                {
                                    return new SuccessDataResult<List<SelectListProductVariantDto>>(endFilters);
                                }
                            }

                        }

                    }
                }
            }
            else if (filterProduct != null & filterProduct.Attributes.Count == 0 && filterProduct.CategoryId > 0)
            {
                var executeNoFilter = _productDal.DefaultOnNoFilter(filterProduct);
                if (executeNoFilter.Count > 0)
                {
                    var processProductVariant = ProcessProductVariantData(executeNoFilter);
                    if (processProductVariant.Success)
                    {
                        return new SuccessDataResult<List<SelectListProductVariantDto>>(processProductVariant.Data);
                    }

                }
                else
                {
                    return new SuccessDataResult<List<SelectListProductVariantDto>>();
                }
            }
            else if (filterProduct != null & filterProduct.Attributes.Count == 0 && filterProduct.CategoryId == 0)
            {
                var executeNoFilter = _productDal.RandomDefaultOnNoFilter(filterProduct);
                if (executeNoFilter.Count > 0)
                {
                    var processProductVariant = ProcessProductVariantData(executeNoFilter);
                    if (processProductVariant.Success)
                    {
                        if (processProductVariant.Data.Count > 0)
                        {
                            return new SuccessDataResult<List<SelectListProductVariantDto>>(processProductVariant.Data);
                        }
                        else
                        {
                            return new SuccessDataResult<List<SelectListProductVariantDto>>();
                        }
                    }
                }
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

        public IDataResult<List<Product>> GetAllProductByCategoryIdNT(int categoryId)
        {
            var result = _productDal.GetAllAsNoTracking(x => x.CategoryId == categoryId);
            if (result != null)
            {
                return new SuccessDataResult<List<Product>>(result);
            }
            return new ErrorDataResult<List<Product>>(result);
        }

        public IDataResult<List<SelectListProductVariantDto>> ProcessProductVariantData(List<SelectListProductVariantDto> processProductVariants)
        {
            if (processProductVariants != null & processProductVariants.Count > 0)
            {
                for (int i = 0; i < processProductVariants.Count; i++)
                {
                    if (processProductVariants[i].ParentId == 0)
                    {
                        processProductVariants[i].ProductPaths = _productVariantService.ProductVariantImage(processProductVariants[i].ProductVariantId).Data;
                        if (processProductVariants[i].ProductPaths != null & processProductVariants[i].ProductPaths.Count > 0)
                        {
                            processProductVariants[i].ProductVariantId = processProductVariants[i].ProductVariantId;
                            var endVariant = _productVariantService.MainVariantEndVariantNT(processProductVariants[i].ProductVariantId);
                            processProductVariants[i].EndProductVariantId = endVariant.Data.Id;

                            var productVariantStock = _productStockService.GetByProductVariantId(processProductVariants[i].EndProductVariantId.Value).Data;
                            processProductVariants[i].NetPrice = productVariantStock.NetPrice;
                            processProductVariants[i].Quantity = productVariantStock.Quantity;
                            processProductVariants[i].StockCode = productVariantStock.StockCode;
                        }
                        else
                        {
                            processProductVariants.Remove(processProductVariants[i]);
                        }
                    }
                    else if (processProductVariants[i].ParentId > 0)
                    {
                        var mainVariant = _productVariantService.EndVariantMainVariantNT(processProductVariants[i].ParentId.Value);
                        processProductVariants[i].ProductPaths = _productVariantService.ProductVariantImage(mainVariant.Data.Id).Data;
                        if (processProductVariants[i].ProductPaths != null & processProductVariants[i].ProductPaths.Count > 0)
                        {
                            processProductVariants[i].ProductVariantId = mainVariant.Data.Id;
                            var endVariant = _productVariantService.MainVariantEndVariantNT(processProductVariants[i].ProductVariantId);
                            processProductVariants[i].EndProductVariantId = endVariant.Data.Id;

                            var productVariantStock = _productStockService.GetByProductVariantId(processProductVariants[i].EndProductVariantId.Value).Data;
                            processProductVariants[i].NetPrice = productVariantStock.NetPrice;
                            processProductVariants[i].Quantity = productVariantStock.Quantity;
                            processProductVariants[i].StockCode = productVariantStock.StockCode;
                        }
                        else
                        {
                            processProductVariants.Remove(processProductVariants[i]);
                        }
                    }
                }
                return new SuccessDataResult<List<SelectListProductVariantDto>>(processProductVariants);
            }
            return new ErrorDataResult<List<SelectListProductVariantDto>>();
        }
    }
}
