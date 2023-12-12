using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constans;
using Business.ValidationRules.FluentValidation.User;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        IUserOperationClaimService _userOperationClaimService;
        public UserManager(IUserDal userDal, IUserOperationClaimService userOperationClaimService)
        {
            _userDal = userDal;
            _userOperationClaimService = userOperationClaimService;
        }
        [ValidationAspect(typeof(UserValidator))]
        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult(Messages.SuccessAdd);
        }
        [SecuredOperation("user,admin")]
        public IResult Delete(User user)
        {
            _userDal.Delete(user);
            return new SuccessResult(Messages.SuccessDelete);
        }
        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }
        public IDataResult<List<User>> GetAllUser()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll(), Messages.SuccessGet);
        }

        public IDataResult<User> GetById(int id)
        {
            var result = _userDal.Get(u => u.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<User>(result, Messages.SuccessGet);
            }
            return new ErrorDataResult<User>(Messages.UnSuccessGet);
        }
        [SecuredOperation("user,admin")]
        [ValidationAspect(typeof(UserUpdateDtoValidator))]
        public IResult Update(UserForUpdateDto userForUpdateDto)
        {
            byte[] passwordHash, passwordSalt;


            if (GetByMail(userForUpdateDto.Email) != null && GetById(userForUpdateDto.UserId).Data.Email != userForUpdateDto.Email)
            {
                return new ErrorResult(Messages.AvailableUserMail);
            }


            if (userForUpdateDto.NewPassword != null)
            {
                if (userForUpdateDto.OldPassword == null)
                {
                    return new ErrorResult();
                }
                var result = CheckPassword(userForUpdateDto.Email, userForUpdateDto.OldPassword);
                if (result.Success != true)
                {
                    return new ErrorResult(Messages.OldPasswordIncorrect);
                }
                HashingHelper.CreatePasswordHash(userForUpdateDto.NewPassword, out passwordHash, out passwordSalt);
                var user = new User
                {
                    Id = userForUpdateDto.UserId,
                    Email = userForUpdateDto.Email,
                    FirstName = userForUpdateDto.FirstName,
                    LastName = userForUpdateDto.LastName,
                    PhoneNumber = userForUpdateDto.PhoneNumber,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true
                };
                _userDal.Update(user);
            }
            else
            {

                var result = GetById(userForUpdateDto.UserId);
                var user = new User
                {
                    Id = userForUpdateDto.UserId,
                    Email = userForUpdateDto.Email,
                    FirstName = userForUpdateDto.FirstName,
                    LastName = userForUpdateDto.LastName,
                    PhoneNumber = userForUpdateDto.PhoneNumber,
                    PasswordHash = result.Data.PasswordHash,
                    PasswordSalt = result.Data.PasswordSalt,
                    RefreshToken = result.Data.RefreshToken,
                    RefreshTokenEndDate = result.Data.RefreshTokenEndDate,
                    Status = userForUpdateDto.Status
                };
                _userDal.Update(user);
            }


            return new SuccessResult(Messages.SuccessUpdate);
        }

        public User GetByMail(string email)
        {
            var result = _userDal.Get(u => u.Email == email);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public IDataResult<User> GetWhereMailById(int id)
        {
            var result = _userDal.Get(u => u.Id == id);
            return new SuccessDataResult<User>(result);
        }

        public IResult CheckPassword(string email, string password)
        {
            var userToCheck = GetByMail(email);
            if (userToCheck != null)
            {
                if (!HashingHelper.VerifyPasswordHash(password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
                {
                    return new ErrorDataResult<User>();
                }
            }
            return new SuccessResult();
        }

        public IResult CheckEmail(string email)
        {
            var userToCheck = GetByMail(email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserCheck);
            }
            return new SuccessResult();
        }

        public IResult UpdateRefreshToken(UserRefreshTokenDto userRefreshTokenDto)
        {
            if (userRefreshTokenDto != null)
            {
                var user = _userDal.Get(x => x.Id == userRefreshTokenDto.UserId);
                user.RefreshToken = userRefreshTokenDto.RefreshToken;
                user.RefreshTokenEndDate = userRefreshTokenDto.RefresTokenExpiration;
                _userDal.Update(user);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<User> GetByRefreshToken(string refreshToken)
        {
            var result = _userDal.Get(u => u.RefreshToken == refreshToken);
            if (result != null)
            {
                return new SuccessDataResult<User>(result);
            }
            return new ErrorDataResult<User>();
        }
        [SecuredOperation("admin")]
        public IDataResult<List<UserDto>> GetAllUserDto()
        {
            var result = _userDal.GetAllUserDto();
            if (result != null)
            {
                return new SuccessDataResult<List<UserDto>>(result);
            }
            return new ErrorDataResult<List<UserDto>>();
        }
        [TransactionScopeAspect]
        [SecuredOperation("admin")]
        public IResult UpdateUser(UserDto userDto)
        {
            if (userDto != null)
            {
                UserForUpdateDto user = new UserForUpdateDto
                {
                    UserId = userDto.UserId,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Status = userDto.Status,

                };
                Update(user);

                UserOperationClaim userOperationClaim = new UserOperationClaim()
                {
                    Id = userDto.UserOperationClaimId,
                    OperationClaimId = userDto.OperationClaimId,
                    UserId = userDto.UserId
                };
                _userOperationClaimService.Update(userOperationClaim);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        [SecuredOperation("user,admin")]
        public IDataResult<UserDto> GetUserDtoByUserId(int id, int addressId)
        {
            var result = _userDal.GetUserDtoByUserIdAddressId(id, addressId);
            if (result != null)
            {
                return new SuccessDataResult<UserDto>(result);
            }
            return new ErrorDataResult<UserDto>();
        }
    }
}
