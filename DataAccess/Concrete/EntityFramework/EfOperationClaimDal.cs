using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOperationClaimDal : EfEntityRepositoryBase<OperationClaim, PofuMacrameContext>, IOperationClaimDal
    {
        private readonly PofuMacrameContext _context;
        public EfOperationClaimDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
