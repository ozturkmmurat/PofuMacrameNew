using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPasswordResetDal : EfEntityRepositoryBase<PasswordReset, PofuMacrameContext>, IPasswordResetDal
    {
        private readonly PofuMacrameContext _context;
        public EfPasswordResetDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
