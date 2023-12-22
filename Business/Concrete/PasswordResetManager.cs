using Business.Abstract;
using Business.Constans;
using Business.Constans.Html.Mail;
using Business.ValidationRules.FluentValidation.User;
using Core.Aspects.Autofac.Validation;
using Core.Business;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.Security.Link;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.User;
using Entities.EntityParameter.PasswordReset;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace Business.Concrete
{
    public class PasswordResetManager : IPasswordResetService
    {
        IPasswordResetDal _passwordResetDal;
        IUserService _userService;
        IMailService _mailService;
        public PasswordResetManager(
            IPasswordResetDal passwordResetDal,
            IUserService userService,
            IMailService mailService)
        {
            _passwordResetDal = passwordResetDal;
            _userService=userService;
            _mailService=mailService;

        }
        public IResult Add(PasswordReset passwordReset)
        {
            if (passwordReset != null && !GetByUserId(passwordReset.UserId).Success)
            {
                _passwordResetDal.Add(passwordReset);
                return new SuccessResult(Messages.PasswordResetCode);
            }
            return new ErrorResult(Messages.FailedEmailCheck);
        }

        public IResult CheckCodeExpired(PasswordReset passwordReset)
        {
            if (passwordReset != null)
            {
                if (DateTime.Now < passwordReset.ResetEndDate)
                {
                    return new SuccessResult();
                }
                else
                {
                    return new ErrorResult(Messages.CodeHasExpired);
                }

            }
            return new ErrorResult();
        }

        public IResult Delete(PasswordReset passwordReset)
        {
            if (passwordReset != null)
            {
                _passwordResetDal.Delete(passwordReset);
                return new SuccessResult(Messages.PasswordResetCode);
            }
            return new ErrorResult();
        }

        public IDataResult<List<PasswordReset>> GetAll()
        {
            var result = _passwordResetDal.GetAll(x => x.Code != null && x.Code != "" && DateTime.Now < x.ResetEndDate);
            if (result != null)
            {
                return new SuccessDataResult<List<PasswordReset>>(result);
            }
            return new ErrorDataResult<List<PasswordReset>>();
        }

        public IDataResult<PasswordReset> GetByCode(string code)
        {
            if (code != "" && code != null)
            {
                var result = _passwordResetDal.Get(x => x.Code == code);
                if (result != null)
                {

                    return new SuccessDataResult<PasswordReset>(result);
                }
            }
            return new ErrorDataResult<PasswordReset>();
        }

        public IDataResult<PasswordReset> GetByCodeAndUrl(string code, string codeUrl)
        {
            if (code != "" && code != null && codeUrl != "" && codeUrl != null)
            {
                var result = _passwordResetDal.Get(x => x.Code == code && x.CodeUrl == codeUrl);
                if (result != null)
                {
                    return new SuccessDataResult<PasswordReset>(result);
                }
            }
            return new ErrorDataResult<PasswordReset>();
        }

        public IDataResult<PasswordReset> GetByCodeUrl(string codeUrl)
        {
            if (codeUrl != "" && codeUrl != null)
            {
                var result = _passwordResetDal.Get(x => x.CodeUrl == codeUrl);
                if (result != null)
                {
                    return new SuccessDataResult<PasswordReset>(result);
                }
            }
            return new ErrorDataResult<PasswordReset>();
        }

        public IDataResult<PasswordReset> GetByUrl(string url)
        {
            if (url != "" && url != null)
            {
                var result = _passwordResetDal.Get(x => x.Url == url);
                if (result != null)
                {
                    return new SuccessDataResult<PasswordReset>(result);
                }
            }

            return new ErrorDataResult<PasswordReset>();
        }

        public IDataResult<PasswordReset> GetByUserId(int userId)
        {
            var result = _passwordResetDal.Get(x => x.UserId == userId);
            if (result != null)
            {
                return new SuccessDataResult<PasswordReset>(result);
            }
            return new ErrorDataResult<PasswordReset>();
        }

        [ValidationAspect(typeof(UserPasswordResetDtoValidator))]
        public IResult PasswordReset(UserPasswordResetDto userPasswordResetDto)
        {
            var checkLink = GetByUrl(userPasswordResetDto.Link);
            if (checkLink.Success)
            {
                var checkExpiration = CheckCodeExpired(checkLink.Data);
                IResult rulesResult = BusinessRules.Run(checkExpiration);
                if (rulesResult  != null)
                {
                    return new ErrorResult(rulesResult.Message);
                }

                var user = _userService.GetById(checkLink.Data.UserId);
                if (user.Success)
                {
                    UserForUpdateDto userForUpdateDto = new UserForUpdateDto()
                    {
                        UserId = user.Data.Id,
                        FirstName = user.Data.FirstName,
                        LastName = user.Data.LastName,
                        Email = user.Data.Email,
                        NewPassword = userPasswordResetDto.NewPassword,
                        PhoneNumber = user.Data.PhoneNumber,
                        Status = user.Data.Status,
                    };
                    var passwordResetResult = _userService.PasswordReset(userForUpdateDto);
                    if (passwordResetResult.Success)
                    {
                        checkLink.Data.Url = "";
                        checkLink.Data.Code = "";
                        checkLink.Data.UserId = checkLink.Data.UserId;
                        checkLink.Data.Status = 0;
                        Update(checkLink.Data);

                        return new SuccessResult(passwordResetResult.Message);
                    }
                }
            }
            return new ErrorResult(Messages.UnSuccessUserPasswordReset);
        }

        public IDataResult<string> SendPasswordResetCode(PasswordResetParameter passwordResetParameter)
        {
            var user = _userService.GetByMail(passwordResetParameter.Email);
            if (user != null)
            {
                var uniqueCode = UniqueCodeGenerator.GenerateUniqueCode(8);

                var updateResetPassword = GetByUserId(user.Id);
                PasswordReset passwordReset = new PasswordReset();
                passwordReset.UserId = user.Id;
                passwordReset.Code = uniqueCode.ToUpper();
                passwordReset.Url = "";
                passwordReset.ResetEndDate = DateTime.Now.AddMinutes(10);
                passwordReset.CodeUrl = UniqueCodeGenerator.GenerateUniqueLink().ToLower();

                if (!updateResetPassword.Success)
                {
                    Add(passwordReset);
                }
                else
                {
                    Update(passwordReset);
                }

                _mailService.ForgotPasswordCode(passwordResetParameter.Email, uniqueCode);

                return new SuccessDataResult<string>(data:passwordReset.CodeUrl, Messages.PasswordResetCode);
            }
            return new ErrorDataResult<string>(message:Messages.FailedEmailCheck);
        }

        public IResult SendPasswordResetLink(PasswordResetParameter passwordResetParameter)
        {
            var result = GetByCodeAndUrl(passwordResetParameter.Code, passwordResetParameter.CodeUrl);
            if (result.Data != null)
            {
                if (DateTime.Now > result.Data.ResetEndDate)
                {
                    return new ErrorResult();
                }
                var user = _userService.GetById(result.Data.UserId);

                if (user != null)
                {
                    var uniqueLink = UniqueCodeGenerator.GenerateUniqueLink();

                    PasswordReset passwordReset = new PasswordReset();
                    passwordReset.UserId = result.Data.UserId;
                    passwordReset.Code = "";
                    passwordReset.Url = uniqueLink.ToLower();
                    passwordReset.ResetEndDate = result.Data.ResetEndDate;

                    Update(passwordReset);
                    _mailService.SendPaswordResetLink(user.Data.Email, uniqueLink);

                    return new SuccessResult(Messages.PasswordResetCode);
                }
                else
                {
                    return new ErrorResult(Messages.FindFailedUser);
                }
            }
            return new ErrorResult(Messages.FailedEmailCheck);
        }

        public IResult Update(PasswordReset passwordReset)
        {
            var result = GetByUserId(passwordReset.UserId);
            if (passwordReset != null && result != null)
            {
                _passwordResetDal.Update(passwordReset);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult UpdateList(List<PasswordReset> passwordResets)
        {
            if (passwordResets != null & passwordResets.Count > 0)
            {
                _passwordResetDal.UpdateRange(passwordResets);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
