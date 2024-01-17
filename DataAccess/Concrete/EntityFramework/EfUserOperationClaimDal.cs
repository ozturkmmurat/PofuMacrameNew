using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserOperationClaimDal : EfEntityRepositoryBase<UserOperationClaim, PofuMacrameContext>, IUserOperationClaimDal
    {
        private readonly PofuMacrameContext _context;
        public EfUserOperationClaimDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
