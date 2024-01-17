using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCategoryImageDal : EfEntityRepositoryBase<CategoryImage, PofuMacrameContext>, ICategoryImageDal
    {
        private readonly PofuMacrameContext _context;
        public EfCategoryImageDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
