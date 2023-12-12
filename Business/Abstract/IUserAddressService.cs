using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserAddressService
    {
        IDataResult<List<UserAddress>> GetAll();
        IDataResult<UserAddress> GetById(int id);
        IDataResult<List<UserAddress>> GetUserAddresses();
        IResult Add(UserAddress userAddress);
        IResult Update(UserAddress userAddress);
        IResult Delete(UserAddress userAddress);
    }
}
