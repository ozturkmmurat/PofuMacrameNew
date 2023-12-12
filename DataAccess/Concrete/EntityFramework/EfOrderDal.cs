using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.Order.Select;
using Entities.Dtos.SubOrder.Select;
using Entities.EntitiyParameter.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOrderDal : EfEntityRepositoryBase<Order, PofuMacrameContext>, IOrderDal
    {
        public List<SelectUserOrderDto> GetAllUserOrder(int userId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from o in context.Orders.Where(x => x.UserId == userId)
                             select new SelectUserOrderDto
                             {
                                 OrderId = o.Id,
                                 OrderDate = o.OrderDate,
                                 TotalPrice = o.TotalPrice,
                                 SelectSubOrderDtos = (from so in context.SubOrders
                                                       join pv in context.ProductVariants
                                                       on so.VariantId equals pv.Id
                                                       join p in context.Products
                                                       on pv.ProductId equals p.Id
                                                       where so.OrderId == o.Id
                                                       select new SelectSubOrderDto
                                                       {
                                                           SubOrderId = so.Id,
                                                           VariantId = so.VariantId, // Burada ProductVariant'ın Id'sini alıyoruz
                                                           ParentId = pv.ParentId.Value,
                                                           Price = so.Price,
                                                           SubOrderStatus = so.SubOrderStatus,
                                                           ProductName = p.ProductName
                                                       })
                                                      .ToList()
                             };

                return result.ToList();
            }
        }

        public SelectUserOrderDto GetUserOrder(int userId, int orderId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from o in context.Orders.Where(x => x.UserId == userId && x.Id == orderId)
                             select new SelectUserOrderDto
                             {
                                 OrderId = o.Id,
                                 OrderDate = o.OrderDate,
                                 TotalPrice = o.TotalPrice,
                                 Address = o.Address,
                                 SelectSubOrderDtos = (from so in context.SubOrders
                                                       join pv in context.ProductVariants
                                                       on so.VariantId equals pv.Id
                                                       join p in context.Products
                                                       on pv.ProductId equals p.Id
                                                       where so.OrderId == o.Id
                                                       select new SelectSubOrderDto
                                                       {
                                                           SubOrderId = so.Id,
                                                           VariantId = so.VariantId, // Burada ProductVariant'ın Id'sini alıyoruz
                                                           ParentId = pv.ParentId.Value,
                                                           Price = so.Price,
                                                           SubOrderStatus = so.SubOrderStatus
                                                       })
                                                      .ToList()
                             };

                return result.FirstOrDefault();
            }
        }
    }
}