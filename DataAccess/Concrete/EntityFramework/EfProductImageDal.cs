using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<string> GetFirstTwoPhotosNT(int productVariantId)
        {
            var result = _context.ProductImages.AsNoTracking()
                .Where(x => x.ProductVariantId == productVariantId)
                .Select(x => x.Path)
                .Take(2)
                .ToList();

            return result;
        }
    }
}
