using Business.Abstract;
using Business.Constans;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductPriceFactorManager : IProductPriceFactorService
    {
        IProductPriceFactorDal _productPriceFactorDal;

        public ProductPriceFactorManager(IProductPriceFactorDal productPriceFactorDal)
        {
            _productPriceFactorDal = productPriceFactorDal;
        }

        public IResult Add(ProductPriceFactor productPriceFactor)
        {
            if (productPriceFactor == null)
                return new ErrorResult(Messages.DataRuleFail);

            var exists = _productPriceFactorDal.Get(x => x.DistrictId == productPriceFactor.DistrictId);
            if (exists != null)
                return new ErrorResult(Messages.ProductPriceFactorDistrictExists);

            _productPriceFactorDal.Add(productPriceFactor);
            return new SuccessResult();
        }

        public IResult Delete(ProductPriceFactor productPriceFactor)
        {
            if (productPriceFactor != null)
            {
                _productPriceFactorDal.Delete(productPriceFactor);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<ProductPriceFactor>> GetAll()
        {
            var result = _productPriceFactorDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<ProductPriceFactor>>(result);
            }
            return new ErrorDataResult<List<ProductPriceFactor>>();
        }

        public IDataResult<List<ProductPriceFactor>> GetAllAsNoTracking()
        {
            var result = _productPriceFactorDal.GetAllAsNoTracking();
            if (result != null)
            {
                return new SuccessDataResult<List<ProductPriceFactor>>(result);
            }
            return new ErrorDataResult<List<ProductPriceFactor>>();
        }

        public IDataResult<ProductPriceFactor> GetById(int id)
        {
            var result = _productPriceFactorDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<ProductPriceFactor>(result);
            }
            return new ErrorDataResult<ProductPriceFactor>();
        }

        public IResult Update(ProductPriceFactor productPriceFactor)
        {
            if (productPriceFactor == null)
                return new ErrorResult(Messages.DataRuleFail);

            var exists = _productPriceFactorDal.Get(x => x.DistrictId == productPriceFactor.DistrictId && x.Id != productPriceFactor.Id);
            if (exists != null)
                return new ErrorResult(Messages.ProductPriceFactorDistrictExists);

            _productPriceFactorDal.Update(productPriceFactor);
            return new SuccessResult();
        }
    }
}
