using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Core.Utilities.Result.Abstract;
using Entities.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<OperationClaim>> GetClaims(User user);
        IDataResult<List<User>> GetAllUser();
        IDataResult<List<UserDto>> GetAllUserDto();
        IDataResult<User> GetById(int id);
        IDataResult<User> GetByRefreshToken(string refreshToken);
        Core.Entities.Concrete.User GetByMail(string email);
        IResult UpdateRefreshToken(UserRefreshTokenDto userRefreshTokenDto);
        IResult CheckPassword(string email, string password);
        IResult CheckEmail(string email);
        IResult Add(User user);
        IResult Update(UserForUpdateDto userForRegisterDto);
        IResult UpdateUser(UserDto userDto);
        IResult Delete(User user);
    }
}
