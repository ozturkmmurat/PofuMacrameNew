using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class DistrictManager : IDistrictService
    {
        IDistrictDal _districtDal;

        public DistrictManager(IDistrictDal districtDal)
        {
            _districtDal = districtDal;
        }

        public IResult Add(District district)
        {
            if (district != null)
            {
                _districtDal.Add(district);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(District district)
        {
            if (district != null)
            {
                _districtDal.Delete(district);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<District>> GetAll()
        {
            var result = _districtDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<District>>(result);
            }
            return new ErrorDataResult<List<District>>();
        }

        public IDataResult<List<District>> GetAllAsNoTracking()
        {
            var result = _districtDal.GetAllAsNoTracking();
            if (result != null)
            {
                return new SuccessDataResult<List<District>>(result);
            }
            return new ErrorDataResult<List<District>>();
        }

        public IDataResult<District> GetById(int id)
        {
            var result = _districtDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<District>(result);
            }
            return new ErrorDataResult<District>();
        }

        public IResult Update(District district)
        {
            if (district != null)
            {
                _districtDal.Update(district);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
