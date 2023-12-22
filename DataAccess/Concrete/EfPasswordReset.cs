using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete
{
    public class EfPasswordResetDal : EfEntityRepositoryBase<PasswordReset, PofuMacrameContext>, IPasswordResetDal
    {
    }
}
