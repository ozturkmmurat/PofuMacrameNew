using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductImageDal : EfEntityRepositoryBase<ProductImage, PofuMacrameContext>, IProductmageDal
    {
        private readonly PofuMacrameContext _context;
        public EfProductImageDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
