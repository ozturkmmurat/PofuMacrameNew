using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserAddressDal : EfEntityRepositoryBase<UserAddress, PofuMacrameContext>, IUserAddressDal
    {
        private readonly PofuMacrameContext _context;
        public EfUserAddressDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
