using Core.Utilities.Result.Abstract;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IAttributeService 
    {
        IDataResult<List<Entities.Concrete.Attribute>> GetAll();
        IDataResult<Entities.Concrete.Attribute> GetById(int id);
        IResult Add(Entities.Concrete.Attribute attribute);
        IResult Update(Entities.Concrete.Attribute attribute);
        IResult Delete(Entities.Concrete.Attribute attribute);
    }
}
