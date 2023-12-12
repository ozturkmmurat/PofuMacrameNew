using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CityManager : ICityService
    {
        ICityDal _cityDal;

        public CityManager(ICityDal cityDal)
        {
             _cityDal = cityDal;
        }
        public IDataResult<List<City>> GetAll()
        {
            var result = _cityDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<City>>(result);
            }
            return new ErrorDataResult<List<City>>();
        }
    }
}
