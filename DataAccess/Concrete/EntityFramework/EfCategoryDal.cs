﻿using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category, PofuMacrameContext>, ICategoryDal
    {
        private readonly PofuMacrameContext _context;
        public EfCategoryDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
