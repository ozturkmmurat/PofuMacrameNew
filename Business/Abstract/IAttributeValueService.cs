using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Abstract
{
    public interface IAttributeValueService
    {
        IDataResult<List<AttributeValue>> GetAll();
        IDataResult<List<AttributeValue>> GetAllByAttributeId(int attributeId);
        IDataResult<AttributeValue> GetById(int id);
        IDataResult<AttributeValue> GetByAttributeId(int attributeId);
        IResult Add(AttributeValue attributeValue);
        IResult Update(AttributeValue attributeValue);
        IResult Delete(AttributeValue attributeValue);
    }
}
