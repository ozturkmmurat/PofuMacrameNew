using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IEntityTypeService
    {
        IDataResult<List<EntityType>> GetAll();
        IDataResult<EntityType> GetById(int id);
        IResult Add(EntityType entityType);
        IResult Update(EntityType entityType);
        IResult Delete(EntityType entityType);
    }
}
