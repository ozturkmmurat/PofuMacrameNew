using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductAttributeManager : IProductAttributeService
    {
        IProductAttributeDal _productAttributeDal;
        public ProductAttributeManager(IProductAttributeDal productAttributeDal)
        {
            _productAttributeDal = productAttributeDal;
        }
        public IResult Add(ProductAttribute productAttribute)
        {
            if (productAttribute != null)
            {
                _productAttributeDal.Add(productAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult AddList(List<ProductAttribute> productAttributes)
        {
            if (productAttributes != null)
            {
                if (productAttributes.Count >= 0)
                {
                    _productAttributeDal.AddRange(productAttributes);
                    return new SuccessResult();
                }
            }
            return new ErrorResult();
        }

        public IResult Delete(ProductAttribute productAttribute)
        {
            if (productAttribute != null)
            {
                _productAttributeDal.Delete(productAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<ProductAttribute>> GetAll()
        {
            var result = _productAttributeDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<ProductAttribute>>(result);
            }
            return new ErrorDataResult<List<ProductAttribute>>();
        }

        public IDataResult<List<ProductAttribute>> GetAllByProductId(int productId)
        {
            var result = _productAttributeDal.GetAll(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<List<ProductAttribute>>(result);
            }
            return new ErrorDataResult<List<ProductAttribute>>();
        }

        public IDataResult<List<ProductAttributeDto>> GetAllDtoByProductId(int productId)
        {
            var result = _productAttributeDal.GetAllFilterDto(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<List<ProductAttributeDto>>(result);
            }
            return new ErrorDataResult<List<ProductAttributeDto>>();
        }

        public IResult Update(ProductAttribute productAttribute)
        {
            if (productAttribute != null)
            {
                _productAttributeDal.Update(productAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
