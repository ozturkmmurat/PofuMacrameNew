using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IDistrictService
    {
        IDataResult<List<District>> GetAll();
        IDataResult<List<District>> GetAllAsNoTracking();
        IDataResult<District> GetById(int id);
        IResult Add(District district);
        IResult Update(District district);
        IResult Delete(District district);
    }
}
