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
        IDataResult<Core.Entities.Concrete.User> Register(UserForRegisterDto userForRegisterDto, string password);
        IDataResult<Core.Entities.Concrete.User> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(string email);
        IResult CheckStatus(string email);
        IDataResult<AccessToken> CreateAccessToken(Core.Entities.Concrete.User user);
    }
}
