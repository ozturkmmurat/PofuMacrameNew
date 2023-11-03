using Business.Abstract;
using Business.Constans;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
            IUserOperationClaimDal _userOperationClaimDal;
            public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
            {
                _userOperationClaimDal = userOperationClaimDal;
            }

            public IResult Add(UserOperationClaim userOperationClaim)
            {
                if (userOperationClaim != null)
                {
                    _userOperationClaimDal.Add(userOperationClaim);
                    return new SuccessResult();
                }
                return new ErrorResult();
            }

            public IDataResult<List<UserOperationClaim>> GetAllUserOpeartionClaim()
            {
                var result = _userOperationClaimDal.GetAll();
                if (result != null)
                {
                    return new SuccessDataResult<List<UserOperationClaim>>(result);
                }
                return new ErrorDataResult<List<UserOperationClaim>>();
            }

            public IResult Update(UserOperationClaim userOperationClaim)
            {
                if (userOperationClaim != null)
                {
                    _userOperationClaimDal.Update(userOperationClaim);
                    return new SuccessResult(Messages.SuccessUpdate);
                }
                return new ErrorResult(Messages.UnSuccessUpdate);
            }
        }
    }