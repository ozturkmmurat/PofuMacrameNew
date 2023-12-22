using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Security.JWT;
using Entities.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);

        IDataResult<AccessToken> CreateAccessToken(User user);
        IResult UserExists(string email);
        IResult CheckStatus(string email);
    }
}
