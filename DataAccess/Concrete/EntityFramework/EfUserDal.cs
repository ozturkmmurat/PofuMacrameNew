using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, PofuMacrameContext>, IUserDal
    {
        public List<UserDto> GetAllUserDto()
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from u in context.Users
                             join uop in context.UserOperationClaims
                             on u.Id equals uop.UserId
                             into userOperationClaimTemp
                             from uopt in userOperationClaimTemp.DefaultIfEmpty()
                             join op in context.OperationClaims
                             on uopt.OperationClaimId equals op.Id
                              into operationClaimTemp
                             from opct in operationClaimTemp.DefaultIfEmpty()

                             select new UserDto
                             {
                                 UserOperationClaimId = uopt.Id,
                                 OperationClaimId = opct.Id,
                                 UserId = u.Id,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 Email = u.Email,
                                 Status = u.Status,
                                 OperationClaimName = opct.Name,                             };

                return result.ToList();
            }
        }

        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new PofuMacrameContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };

                return result.ToList();
            }
        }

        public UserDto GetUserDtoByUserIdAddressId(int userId, int addressId)
        {
            using (var context = new PofuMacrameContext())
            {
                var result = from u in context.Users.Where(x => x.Id == userId)
                             join a in context.UserAddresses.Where(x => x.Id == addressId)
                             on u.Id equals a.UserId
                             join c in context.Cities
                             on a.CityId equals c.Id

                             select new UserDto
                             {
                                UserId = u.Id,
                                FirstName = u.FirstName,
                                LastName= u.LastName,
                                Email = u.Email,
                                PhoneNumber = u.PhoneNumber,
                                RegistrationDate = u.RegistrationDate.Value,
                                LastLoginDate = u.LastLoginDate.Value,
                                Status = u.Status,
                                Country = "Türkiye",
                                UserCity =  c.Name,
                                AddressTitle = a.AddressTitle,
                                Address = a.Address,
                                PostCode = a.PostCode
                             };
                return result.FirstOrDefault();
            }
        }
    }
}
