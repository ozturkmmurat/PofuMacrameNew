using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class AttributeValueManager : IAttributeValueService
    {
        IAttributeValueDal _attributeValueDal;
        public AttributeValueManager(IAttributeValueDal attributeValueDal)
        {
            _attributeValueDal = attributeValueDal;
        }
        public IResult Add(AttributeValue attributeValue)
        {
            if (attributeValue != null)
            {
                _attributeValueDal.Add(attributeValue);
                return new SuccessResult();
            }
            return new ErrorResult();
        }


        public IResult Delete(AttributeValue attributeValue)
        {
            if (attributeValue != null)
            {
                _attributeValueDal.Delete(attributeValue);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<AttributeValue>> GetAll()
        {
            var result = _attributeValueDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<AttributeValue>>(result);
            }
            return new ErrorDataResult<List<AttributeValue>>();
        }

        public IDataResult<List<AttributeValue>> GetAllByAttributeId(int attributeId)
        {
            var result = _attributeValueDal.GetAll(x => x.AttributeId == attributeId);
            if (result != null)
            {
                return new SuccessDataResult<List<AttributeValue>>(result);
            }
            return new ErrorDataResult<List<AttributeValue>>();
        }

        public IDataResult<AttributeValue> GetByAttributeId(int attributeId)
        {
            var result = _attributeValueDal.Get(x => x.AttributeId == attributeId);
            if (result != null)
            {
                return new SuccessDataResult<AttributeValue>(result);
            }
            return new ErrorDataResult<AttributeValue>();
        }

        public IDataResult<AttributeValue> GetById(int id)
        {
            var result = _attributeValueDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<AttributeValue>(result);
            }
            return new ErrorDataResult<AttributeValue>();
        }

        public IResult Update(AttributeValue attributeValue)
        {
            if (attributeValue != null)
            {
                _attributeValueDal.Update(attributeValue);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
