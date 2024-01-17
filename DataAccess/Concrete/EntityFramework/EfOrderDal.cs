using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
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
        private readonly PofuMacrameContext _context;
        public EfOrderDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
        public List<SelectUserOrderDto> GetAllUserOrder(int userId)
        {
            var result = from o in _context.Orders.Where(x => x.UserId == userId)
                         select new SelectUserOrderDto
                         {
                             OrderId = o.Id,
                             OrderDate = o.OrderDate,
                             TotalPrice = o.TotalPrice,
                             SelectSubOrderDtos = (from so in _context.SubOrders
                                                   join pv in _context.ProductVariants
                                                   on so.VariantId equals pv.Id
                                                   join p in _context.Products
                                                   on pv.ProductId equals p.Id
                                                   where so.OrderId == o.Id
                                                   select new SelectSubOrderDto
                                                   {
                                                       SubOrderId = so.Id,
                                                       VariantId = so.VariantId, // Burada ProductVariant'ın Id'sini alıyoruz
                                                       ParentId = pv.ParentId.Value,
                                                       Price = so.Price,
                                                       Kdv = so.Kdv,
                                                       NetPrice = so.NetPrice,
                                                       SubOrderStatus = so.SubOrderStatus,
                                                       ProductName = p.ProductName
                                                   })
                                                  .ToList()
                         };

            return result.ToList();
        }

        public List<SelectUserOrderDto> GetAllUserOrderAdmin()
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from o in context.Orders
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
                                                           Kdv = so.Kdv,
                                                           NetPrice = so.NetPrice,
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
                             join u in context.Users.Where(x => x.Id == userId)
                             on o.UserId equals u.Id
                             select new SelectUserOrderDto
                             {
                                 OrderId = o.Id,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 PhoneNumber = u.PhoneNumber,
                                 Address = o.Address,
                                 OrderDate = o.OrderDate,
                                 TotalPrice = o.TotalPrice,
                                 OrderStatus = o.OrderStatus,
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
                                                           Kdv = so.Kdv,
                                                           NetPrice = so.NetPrice,
                                                           SubOrderStatus = so.SubOrderStatus
                                                       })
                                                      .ToList()
                             };

                return result.FirstOrDefault();
            }
        }
    }
}