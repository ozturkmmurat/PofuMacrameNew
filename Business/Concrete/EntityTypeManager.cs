using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class EntityTypeManager : IEntityTypeService
    {
        IEntityTypeDal _entityTypeDal;

        public EntityTypeManager(IEntityTypeDal entityTypeDal)
        {
            _entityTypeDal = entityTypeDal;
        }
        public IResult Add(EntityType entityType)
        {
            if (entityType != null)
            {
                _entityTypeDal.Add(entityType);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(EntityType entityType)
        {
            if (entityType != null)
            {
                _entityTypeDal.Delete(entityType);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<EntityType>> GetAll()
        {
            var result = _entityTypeDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<EntityType>>(result);
            }
            return new ErrorDataResult<List<EntityType>>();
        }

        public IDataResult<EntityType> GetById(int id)
        {
            var result = _entityTypeDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<EntityType>(result);
            }
            return new ErrorDataResult<EntityType>();
        }

        public IResult Update(EntityType entityType)
        {
            if (entityType != null)
            {
                _entityTypeDal.Update(entityType);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
