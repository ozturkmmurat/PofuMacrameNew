﻿using Business.Abstract;
using Business.Constans;
using Business.ValidationRules.FluentValidation.User;
using Core.Aspects.Autofac.Validation;
using Core.Business;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Core.Utilities.Security.Link;
using Entities.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private IUserOperationClaimService _userOperationClaimService;
        private IMailService _mailService;
        private IPasswordResetService _passwordResetService;
        public AuthManager(
            IUserService userService,
            ITokenHelper tokenHelper,
            IUserOperationClaimService userOperationClaimService,
            IMailService mailService,
            IPasswordResetService passwordResetService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _userOperationClaimService = userOperationClaimService;
            _mailService = mailService;
            _passwordResetService = passwordResetService;
        }
        [ValidationAspect(typeof(UserForRegisterDtoValidator))]
        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;

            if (_userService.GetByMail(userForRegisterDto.Email) != null)
            {
                return new ErrorDataResult<User>(Messages.CurrentMail);
            }

            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                RegistrationDate = DateTime.Now
            };
            _userService.Add(user);
            var newUser = _userService.GetByMail(userForRegisterDto.Email);

            var userOperationClaim = new UserOperationClaim()
            {
                UserId = newUser.Id,
                OperationClaimId = 3
            };
            _userOperationClaimService.Add(userOperationClaim);
            _mailService.Register(user.FirstName, user.LastName, user.Email);
            return new SuccessDataResult<User>(user, Messages.SuccessRegister);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);

            IResult result = BusinessRules.Run(_userService.CheckPassword(userForLoginDto.Email, userForLoginDto.Password), _userService.CheckEmail(userForLoginDto.Email), CheckStatus(userForLoginDto.Email));
            if (result == null)
            {
                return new SuccessDataResult<Core.Entities.Concrete.User>(userToCheck, "Başarılı giriş");
            }
            return new ErrorDataResult<Core.Entities.Concrete.User>(result.Message);

        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAvailable);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken, "Token oluşturuldu.");
        }

        public IResult CheckStatus(string email)
        {
            var statusCheck = _userService.GetByMail(email);
            if (statusCheck != null)
            {
                if (statusCheck.Status != false)
                {
                    return new SuccessResult();
                }
            }

            return new ErrorResult("Yetkililer tarafından hesabınızın aktif hale getirilmesi gerekmektedir.");
        }
    }
}
