using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.User;
using Entities.EntityParameter.PasswordReset;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IPasswordResetService
    {
        IDataResult<List<PasswordReset>> GetAll();
        IDataResult<PasswordReset> GetByUserId(int userId);
        IDataResult<PasswordReset> GetByCode(string code);
        IDataResult<PasswordReset> GetByUrl(string url);
        IDataResult<PasswordReset> GetByCodeAndUrl(string code, string codeUrl);
        IDataResult<PasswordReset> GetByCodeUrl(string codeUrl);
        IResult Add(PasswordReset passwordReset);
        IResult Update(PasswordReset passwordReset);
        IResult UpdateList(List<PasswordReset> passwordResets);
        IResult Delete(PasswordReset passwordReset);
        IDataResult<string> SendPasswordResetCode(PasswordResetParameter passwordResetParameter);
        IResult SendPasswordResetLink(PasswordResetParameter passwordResetParameter);
        IResult PasswordReset(UserPasswordResetDto userPasswordResetDto);
        IResult CheckCodeExpired(PasswordReset passwordReset);
    }
}
