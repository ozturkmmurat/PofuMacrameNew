using Core.DataAccess.EntityFramework;
using Core.Utilities.Result.Abstract;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.SubOrder.Select;
using Entities.EntitiyParameter.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfSubOrderDal : EfEntityRepositoryBase<SubOrder, PofuMacrameContext>, ISubOrderDal
    {
        public bool CheckSubOrder(int orderId, int subOrderId, int userId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from so in context.SubOrders.Where(x => x.Id == subOrderId)
                             join o in context.Orders.Where(x => x.Id == orderId && x.UserId == userId)
                             on so.OrderId equals o.Id
                             select new { so, o };

                if (result.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<SelectOrderedProducts> GetAllOrderedProduct()
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from so in context.SubOrders
                             join o in context.Orders
                             on so.OrderId equals o.Id
                             join pv in context.ProductVariants
                             on so.VariantId equals pv.Id
                             join p in context.Products
                             on pv.ProductId equals p.Id

                             select new SelectOrderedProducts
                             {
                                 OrderId = o.Id,
                                 OrderDate = o.OrderDate,
                                 OrderCode = o.OrderCode,
                                 UserId = o.UserId,
                                 VariantId = so.VariantId, // Burada ProductVariant'ın Id'sini alıyoruz
                                 ParentId = pv.ParentId.Value,
                                 Price = so.Price,
                                 SubOrderStatus = so.SubOrderStatus,
                                 ProductName = p.ProductName
                             };

                return result.ToList();
            }
        }
    }
}