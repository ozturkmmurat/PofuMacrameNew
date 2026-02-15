using Business.Abstract;
using Business.Abstract.ProductVariants;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
using Entities.Dtos.ProductStock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Business.Constans;

namespace Business.Concrete
{
    public class ProductStockManager : IProductStockService
    {
        IProductStockDal _productStockDal;
        IProductVariantAttributeCombinationService _productVariantAttributeCombinationService;
        IProductPriceFactorService _productPriceFactorService;

        public ProductStockManager(IProductStockDal productStockDal,
            IProductVariantAttributeCombinationService productVariantAttributeCombinationService,
            IProductPriceFactorService productPriceFactorService)
        {
            _productStockDal = productStockDal ?? throw new ArgumentNullException(nameof(productStockDal));
            _productVariantAttributeCombinationService = productVariantAttributeCombinationService ?? throw new ArgumentNullException(nameof(productVariantAttributeCombinationService));
            _productPriceFactorService = productPriceFactorService ?? throw new ArgumentNullException(nameof(productPriceFactorService));
        }
        public IResult Add(ProductStock productStock)
        {
            if (productStock == null)
                return new ErrorResult(Messages.DataRuleFail);

            _productStockDal.Add(productStock);
            return new SuccessResult();
        }

        public IResult AddList(List<ProductStock> productStocks)
        {
            if (productStocks == null || productStocks.Count == 0)
                return new ErrorResult(Messages.DataRuleFail);
            _productStockDal.AddRange(productStocks);
            return new SuccessResult();
        }

        public IResult CheckProductStock(ProductStock productStock)
        {
            if (productStock == null)
                return new ErrorResult(Messages.DataRuleFail);

            var checkProducStock = _productStockDal.Get(x => x.ProductVariantId == productStock.ProductVariantId);
            if (checkProducStock == null)
                return new ErrorResult(Messages.UnSuccessProductStockCheck);

            if (checkProducStock.Quantity <= 0 || productStock.Quantity > checkProducStock.Quantity)
                return new ErrorResult(Messages.UnSuccessProductStockCheck);
            return new SuccessResult();
        }

/// <summary>
        /// ProductVariantIds listesindeki her varyant için NetPrice döner. İlçe ek ücreti sipariş başına bir kez: ilk dönen DTO'nun ExtraPrice alanında set edilir.
        /// </summary>
        public IDataResult<List<ProductStockPriceDto>> CheckProductStockPrice(ProductStockPriceCheckDto productStockPriceCheckDto)
        {
            if (productStockPriceCheckDto?.ProductVariantId == null || productStockPriceCheckDto.ProductVariantId.Count == 0)
                return new ErrorDataResult<List<ProductStockPriceDto>>(Messages.UnSuccessProductStockPrice);

            decimal orderExtraPrice = 0;
            if (productStockPriceCheckDto.ProductPriceFactorId > 0)
            {
                var factor = _productPriceFactorService.GetById(productStockPriceCheckDto.ProductPriceFactorId);
                if (factor != null && factor.Success && factor.Data != null)
                    orderExtraPrice = factor.Data.ExtraPrice;
            }

            var resultList = new List<ProductStockPriceDto>(productStockPriceCheckDto.ProductVariantId.Count);
            for (int i = 0; i < productStockPriceCheckDto.ProductVariantId.Count; i++)
            {
                int variantId = productStockPriceCheckDto.ProductVariantId[i];
                if (variantId <= 0)
                    return new ErrorDataResult<List<ProductStockPriceDto>>(Messages.DataRuleFail);

                var stock = _productStockDal.Get(x => x.ProductVariantId == variantId);
                if (stock == null)
                    return new ErrorDataResult<List<ProductStockPriceDto>>(Messages.UnSuccessProductStockPrice);

                resultList.Add(new ProductStockPriceDto
                {
                    NetPrice = stock.NetPrice,
                    ExtraPrice = i == 0 ? orderExtraPrice : 0
                });
            }
            return new SuccessDataResult<List<ProductStockPriceDto>>(resultList);
        }

        public IResult Delete(ProductStock productStock)
        {
            if (productStock == null)
                return new ErrorResult(Messages.DataRuleFail);

            _productStockDal.Delete(productStock);
            return new SuccessResult();
        }

        public IDataResult<List<ProductStock>> GetAll()
        {
            var result = _productStockDal.GetAll();
            if (result == null)
                return new ErrorDataResult<List<ProductStock>>(Messages.UnSuccessGet);

            return new SuccessDataResult<List<ProductStock>>(result);
        }

        public IDataResult<List<ProductStock>> GetAllByProductId(int productId)
        {
            if (productId <= 0)
                return new ErrorDataResult<List<ProductStock>>(Messages.DataRuleFail);
            var result = _productStockDal.GetAll(x => x.ProductId == productId);
            if (result == null)
                return new ErrorDataResult<List<ProductStock>>(Messages.UnSuccessGet);

            return new SuccessDataResult<List<ProductStock>>(result);
        }

        public IDataResult<List<SelectProductStockDto>> GetAllProductStockDto(int productId)
        {
            if (productId <= 0)
                return new ErrorDataResult<List<SelectProductStockDto>>(Messages.DataRuleFail);

            var result = GetAllByProductId(productId);
            if (!result.Success || result.Data == null)
                return new ErrorDataResult<List<SelectProductStockDto>>(Messages.UnSuccessGet);

            var productVariantAttributes = _productVariantAttributeCombinationService.GetAllEndCombinationAttributeValue(productId).Data;
            if (productVariantAttributes == null)
                return new SuccessDataResult<List<SelectProductStockDto>>(new List<SelectProductStockDto>());

            if (result.Data.Count > 0 && productVariantAttributes.Count > 0)
            {
                var joinResult = from stock in result.Data
                                     join productVariantAttribute in productVariantAttributes
                                     on stock.ProductVariantId equals productVariantAttribute.EndProductVariantId
                                     select new SelectProductStockDto
                                     {
                                         ProductId = stock.ProductId,
                                         FirstProductVariantId = productVariantAttribute.ProductVariantId,
                                         EndProductVariantId = stock.ProductVariantId,
                                         ProductStockId = stock.Id,
                                         ParentId = productVariantAttribute.ParentId,
                                         StockCode = stock.StockCode,
                                         Price = stock.Price,
                                         Quantity = stock.Quantity,
                                         Kdv = stock.Kdv,
                                         NetPrice = stock.NetPrice,
                                         AttributeValue = productVariantAttribute.AttributeValue
                                     };


                return new SuccessDataResult<List<SelectProductStockDto>>(joinResult.ToList());
            }
            return new ErrorDataResult<List<SelectProductStockDto>>(Messages.UnSuccessGet);
        }

        public IDataResult<ProductStock> GetById(int id)
        {
            if (id <= 0)
                return new ErrorDataResult<ProductStock>(Messages.DataRuleFail);

            var result = _productStockDal.Get(x => x.Id == id);
            if (result == null)
                return new ErrorDataResult<ProductStock>(Messages.UnSuccessGet);

            return new SuccessDataResult<ProductStock>(result);
        }

        public IDataResult<ProductStock> GetByProductId(int productId)
        {
            if (productId <= 0)
                return new ErrorDataResult<ProductStock>(Messages.DataRuleFail);
            var result = _productStockDal.Get(x => x.ProductId == productId);
            if (result == null)
                return new ErrorDataResult<ProductStock>(Messages.UnSuccessGet);
            return new SuccessDataResult<ProductStock>(result);
        }

        public IDataResult<ProductStock> GetByProductVariantId(int variantId)
        {
            if (variantId <= 0)
                return new ErrorDataResult<ProductStock>(Messages.DataRuleFail);

            var result = _productStockDal.Get(x => x.ProductVariantId == variantId);
            if (result == null)
                return new ErrorDataResult<ProductStock>(Messages.UnSuccessGet);

            return new SuccessDataResult<ProductStock>(result);
        }

        public IResult Update(ProductStock productStock)
        {
            if (productStock == null)
                return new ErrorResult(Messages.DataRuleFail);

            _productStockDal.Update(productStock);
            return new SuccessResult();
        }
    }
}
