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
        public ProductStockManager(IProductStockDal productStockDal,
            IProductVariantAttributeCombinationService productVariantAttributeCombinationService)
        {
            _productStockDal = productStockDal;
            _productVariantAttributeCombinationService = productVariantAttributeCombinationService;
        }
        public IResult Add(ProductStock productStock)
        {
            if (productStock != null)
            {
                _productStockDal.Add(productStock);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult AddList(List<ProductStock> productStocks)
        {
            if (productStocks != null && productStocks.Count > 0)
            {
                _productStockDal.AddRange(productStocks);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult CheckProductStock(ProductStock productStock)
        {
            if (productStock != null)
            {
                var checkProducStock = _productStockDal.Get(x => x.ProductVariantId == productStock.ProductVariantId);
                if (checkProducStock != null & checkProducStock.Quantity <= 0 && productStock.Quantity > checkProducStock.Quantity)
                {
                    return new ErrorResult(Messages.UnSuccessProductStockCheck);
                }
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult CheckProductStockPrice(ProductStock productStock)
        {
            if (productStock != null)
            {
                var checkProducStock = _productStockDal.Get(x => x.ProductVariantId == productStock.ProductVariantId);
                if (checkProducStock != null & checkProducStock.Price != productStock.Price)
                {
                    return new ErrorResult(Messages.UnSuccessProductStockPrice);
                }
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(ProductStock productStock)
        {
            if (productStock != null)
            {
                _productStockDal.Delete(productStock);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<ProductStock>> GetAll()
        {
            var result = _productStockDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<ProductStock>>(result);
            }
            return new ErrorDataResult<List<ProductStock>>();
        }

        public IDataResult<List<ProductStock>> GetAllByProductId(int productId)
        {
            var result = _productStockDal.GetAll(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<List<ProductStock>>(result);
            }
            return new ErrorDataResult<List<ProductStock>>();
        }

        public IDataResult<List<SelectProductStockDto>> GetAllProductStockDto(int productId)
        {
            var result = GetAllByProductId(productId);
            var productVariantAttributes = _productVariantAttributeCombinationService.GetAllEndCombinationAttributeValue(productId).Data;
            if (result.Data != null && result.Success && productVariantAttributes != null)
            {
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
            }
            return new ErrorDataResult<List<SelectProductStockDto>>();
        }

        public IDataResult<ProductStock> GetById(int id)
        {
            var result = _productStockDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<ProductStock>(result);
            }
            return new ErrorDataResult<ProductStock>();
        }

        public IDataResult<ProductStock> GetByProductId(int productId)
        {
            var result = _productStockDal.Get(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<ProductStock>(result);
            }
            return new ErrorDataResult<ProductStock>();
        }

        public IDataResult<ProductStock> GetByProductVariantId(int variantId)
        {
            var result = _productStockDal.Get(x => x.ProductVariantId == variantId);
            if (result != null)
            {
                return new SuccessDataResult<ProductStock>(result);
            }
            return new ErrorDataResult<ProductStock>();
        }

        public IResult Update(ProductStock productStock)
        {
            if (productStock != null)
            {
                _productStockDal.Update(productStock);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
