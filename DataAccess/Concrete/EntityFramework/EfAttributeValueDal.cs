using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAttributeValueDal : EfEntityRepositoryBase<AttributeValue, PofuMacrameContext>, IAttributeValueDal
    {
        private readonly PofuMacrameContext _context;
        public EfAttributeValueDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
