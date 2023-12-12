using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constans;
using Core.Entities.Concrete;
using Core.Utilities.IoC;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.User;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserAddressManager : IUserAddressService
    {
        IUserAddressDal _userAddressDal;
        private IHttpContextAccessor _httpContextAccessor;
        public UserAddressManager(IUserAddressDal userAddressDal)
        {
            _userAddressDal = userAddressDal;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }
        public IResult Add(UserAddress userAddress)
        {
            if (userAddress != null)
            {
                _userAddressDal.Add(userAddress);
                return new SuccessResult(Messages.SuccessAdd);
            }
            return new ErrorResult(Messages.UnSuccessAdd);
        }

        public IResult Delete(UserAddress userAddress)
        {
            if (userAddress != null)
            {
                _userAddressDal.Delete(userAddress);
                return new SuccessResult(Messages.SuccessDelete);
            }
            return new ErrorResult(Messages.UnSuccessDelete);
        }

        public IDataResult<List<UserAddress>> GetAll()
        {
            var result = _userAddressDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<UserAddress>>(result);
            }
            return new ErrorDataResult<List<UserAddress>>();  
        }

        public IDataResult<UserAddress> GetById(int id)
        {
            var result = _userAddressDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<UserAddress>(result);
            }
            return new ErrorDataResult<UserAddress>();
        }

        [SecuredOperation("user,admin")]
        public IDataResult<List<UserAddress>> GetUserAddresses()
        {
            var userId = ClaimHelper.GetUserId(_httpContextAccessor.HttpContext);
            var result = _userAddressDal.GetAll(x => x.UserId == userId);
            if (result != null)
            {
                return new SuccessDataResult<List<UserAddress>>(result);
            }
            return new ErrorDataResult<List<UserAddress>>();
        }

        public IResult Update(UserAddress userAddress)
        {
            if (userAddress != null)
            {
                _userAddressDal.Update(userAddress);
                return new SuccessResult(Messages.SuccessUpdate);
            }
            return new ErrorResult(Messages.UnSuccessUpdate);
        }
    }
}
