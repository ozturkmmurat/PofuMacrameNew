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
    public class EfCityDal : EfEntityRepositoryBase<City, PofuMacrameContext>, ICityDal
    {
        private readonly PofuMacrameContext _context;
        public EfCityDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
