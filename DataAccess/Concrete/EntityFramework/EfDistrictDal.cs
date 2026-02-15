using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfDistrictDal : EfEntityRepositoryBase<District, PofuMacrameContext>, IDistrictDal
    {
        public EfDistrictDal(PofuMacrameContext context) : base(context)
        {
        }
    }
}
