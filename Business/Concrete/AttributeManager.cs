using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AttributeManager : IAttributeService
    {
        IAttributeDal _attributeDal;
        public AttributeManager(IAttributeDal attributeDal)
        {
            _attributeDal = attributeDal;
        }
        public IResult Add(Entities.Concrete.Attribute attribute)
        {
            if (attribute != null)
            {
                _attributeDal.Add(attribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(Entities.Concrete.Attribute attribute)
        {
            if (attribute != null)
            {
                _attributeDal.Delete(attribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<Entities.Concrete.Attribute>> GetAll()
        {
            var result = _attributeDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<Entities.Concrete.Attribute>>(result);
            }
            return new ErrorDataResult<List<Entities.Concrete.Attribute>>();
        }

        public IDataResult<Entities.Concrete.Attribute> GetById(int id)
        {
            var result = _attributeDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<Entities.Concrete.Attribute>(result);
            }
            return new ErrorDataResult<Entities.Concrete.Attribute>();
        }

        public IResult Update(Entities.Concrete.Attribute attribute)
        {
            if (attribute != null)
            {
                _attributeDal.Update(attribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
