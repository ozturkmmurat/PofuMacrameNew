using Business.Abstract;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductStockManager : IProductStockService
    {
        IProductStockDal _productStockDal;
        public ProductStockManager(IProductStockDal productStockDal)
        {
            _productStockDal = productStockDal;
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
            if (productStocks != null)
            {
                _productStockDal.AddRange(productStocks);
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

        public IDataResult<ProductStock> GetByVariantId(int variantId)
        {
            var result = _productStockDal.Get(x => x.VariantId == variantId);
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

        public IResult UpdateList(List<ProductStock> productStocks)
        {
            if (productStocks != null)
            {
                _productStockDal.UpdateRange(productStocks);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
