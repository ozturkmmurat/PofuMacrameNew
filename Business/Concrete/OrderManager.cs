using Business.Abstract;
using Business.Abstract.ProductVariants;
using Business.BusinessAspects.Autofac;
using Business.Utilities;
using Core.Aspects.Autofac.Transaction;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.User;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.Order;
using Entities.Dtos.Order.Select;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        IOrderDal _orderDal;
        ISubOrderDal _subOrderDal;
        IProductVariantService _productVariantService;
        IProductImageService _productImageService;
        IMailService _mailService;
        private IHttpContextAccessor _httpContextAccessor;
        public OrderManager(
            IOrderDal orderDal,
            ISubOrderDal subOrderDal,
            IProductVariantService productVariantService,
            IProductImageService productImageService,
            IMailService mailService)
        {
            _orderDal = orderDal;
            _subOrderDal = subOrderDal;
            _productVariantService = productVariantService;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _productImageService = productImageService;
            _mailService = mailService;
        }
        public IResult Add(Order order)
        {
            if (order != null)
            {
                _orderDal.Add(order);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<string> CreateOrderCode(Order order)
        {
            string orderCode = null;
            if (order != null)
            {
                var getAllResult = GetAll().Data.Count;
                orderCode = CreateCodeTime.CreateTime() + "-" + "0" + orderCode + "-" + order.UserId + getAllResult+1;
                return new SuccessDataResult<string>(orderCode);
            }
            return new ErrorDataResult<string>();
        }

        public IResult Delete(Order order)
        {
            if (order != null)
            {
                _orderDal.Delete(order);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<Order>> GetAll()
        {
            var result = _orderDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<Order>>(result);
            }
            return new ErrorDataResult<List<Order>>();
        }

        public IDataResult<List<Order>> GetAllByUserId(int userId)
        {
            var result = _orderDal.GetAll(x => x.UserId == userId);
            if (result != null)
            {
                return new SuccessDataResult<List<Order>>(result);
            }
            return new ErrorDataResult<List<Order>>();
        }

        [SecuredOperation("user,admin")]
        public IDataResult<List<SelectUserOrderDto>> GetAllUserOrderDto()
        {
            var result = _orderDal.GetAllUserOrder(ClaimHelper.GetUserId(_httpContextAccessor.HttpContext));
            if (result != null & result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < result[i].SelectSubOrderDtos.Count(); j++)
                    {
                        result[i].SelectSubOrderDtos[j].ImagePath = _productImageService.GetByProductVariantId(_productVariantService.EndVariantMainVariantNT(result[i].SelectSubOrderDtos[j].ParentId).Data.Id).Data.Path;
                    }
                }
                return new SuccessDataResult<List<SelectUserOrderDto>>(result);
            }
            return new ErrorDataResult<List<SelectUserOrderDto>>();
        }

        public IDataResult<SelectUserOrderDto> GetUserOrderDtoDetail(Order order)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            var result = (SelectUserOrderDto)null;

            if (roleClaims.Contains("admin"))
            {
                var getOrder = _orderDal.Get(x => x.Guid == order.Guid);
                var userId = order?.UserId ?? 0;
                result = _orderDal.GetUserOrder(getOrder.Guid);
            }
            else if(roleClaims.Contains("user") && order.Id > 0)
            {
                var getOrder = _orderDal.Get(x => x.Guid == order.Guid);
                result = _orderDal.GetUserOrder(getOrder.Guid);
            }
            else if(!string.IsNullOrWhiteSpace(order.OrderCode) && !string.IsNullOrWhiteSpace(order.Email))
            {
                var orderTrackingResult = GetOrderTracking(order);

                if (orderTrackingResult.Success == false)
                    return new ErrorDataResult<SelectUserOrderDto>();

                result =_orderDal.GetUserOrder(orderTrackingResult.Data.Guid);
            }

            if (result != null)
            {
                for (int i = 0; i < result.SelectSubOrderDtos.Count(); i++)
                {
                    var getProductVariantAttributeResult = _productVariantService.GetProductVariantAttribute(result.SelectSubOrderDtos[i].ParentId);
                    result.SelectSubOrderDtos[i].ImagePath = _productImageService.GetByProductVariantId(getProductVariantAttributeResult.Data.VariantId).Data.Path;
                    result.SelectSubOrderDtos[i].Attribute = _productVariantService.GetProductVariantAttribute(result.SelectSubOrderDtos[i].ParentId).Data.Attribute;
                }
                return new SuccessDataResult<SelectUserOrderDto>(result);
            }
            return new ErrorDataResult<SelectUserOrderDto>();
        }

        public IDataResult<Order> GetById(int id)
        {
            var result = _orderDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<Order>(result);
            }
            return new ErrorDataResult<Order>();
        }

        public IDataResult<Order> GetByOrderIdUserId(int orderId, int userId)
        {
            var result = _orderDal.Get(x => x.Id == orderId && x.UserId == userId);
            if (result != null)
            {
                return new SuccessDataResult<Order>(result);
            }
            return new ErrorDataResult<Order>();
        }

        public IDataResult<Order> GetByGuid(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
                return new ErrorDataResult<Order>();
            var result = _orderDal.Get(x => x.Guid == guid);
            if (result != null)
                return new SuccessDataResult<Order>(result);
            return new ErrorDataResult<Order>();
        }

        public IDataResult<Order> MappingOrder(OrderDto orderDto)
        {
            if (orderDto != null)
            {
                Order order = new Order()
                {
                    Id = orderDto.OrderId,
                    UserId = orderDto.UserId,
                    OrderCode = orderDto.OrderCode,
                    TotalPrice = orderDto.TotalPrice
                };
                return new SuccessDataResult<Order>(order);
            }
            return new ErrorDataResult<Order>();
        }

        [TransactionScopeAspect]
        public IResult TsaAdd(OrderDto orderDto)
        {
            if (orderDto != null)
            {
                var mappingOrder = MappingOrder(orderDto);
                var result = CreateOrderCode(mappingOrder.Data);
                mappingOrder.Data.OrderCode = result.Data;
                _orderDal.Add(mappingOrder.Data);
                var resultSubOrder = new SubOrder();
                foreach (var subOrder in orderDto.SubOrders)
                {
                    resultSubOrder = new SubOrder
                    {
                        OrderId = mappingOrder.Data.Id,
                        Price = subOrder.Price,
                        VariantId = subOrder.VariantId,
                    };
                    mappingOrder.Data.TotalPrice += subOrder.Price;
                }
                _subOrderDal.Add(resultSubOrder);
                _orderDal.Update(mappingOrder.Data);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
        [TransactionScopeAspect]
        public IResult TsaUpdate(OrderDto orderDto)
        {
            if (orderDto != null)
            {
                var mappingOrder = MappingOrder(orderDto);
                var result = CreateOrderCode(mappingOrder.Data);
                mappingOrder.Data.OrderCode = result.Data;
                _orderDal.Update(mappingOrder.Data);
                var resultSubOrder = new SubOrder();
                foreach (var subOrder in orderDto.SubOrders)
                {
                    resultSubOrder = new SubOrder
                    {
                        Id = subOrder.Id,
                        OrderId = mappingOrder.Data.Id,
                        Price = subOrder.Price,
                        VariantId = subOrder.VariantId
                    };
                    mappingOrder.Data.TotalPrice += subOrder.Price;
                }
                _subOrderDal.Update(resultSubOrder);
                _orderDal.Update(mappingOrder.Data);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Update(Order order)
        {
            if (order != null)
            {
                _orderDal.Update(order);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        [SecuredOperation("admin")]
        public IResult MarkAsShipped(Order order)
        {
            if (order == null || order.Id <= 0)
                return new ErrorResult("Sipariş bulunamadı.");

            var orderFromDb = _orderDal.Get(x => x.Id == order.Id);
            if (orderFromDb == null)
                return new ErrorResult("Sipariş bulunamadı.");

            orderFromDb.OrderStatus = 2; // Kargoya verildi
            _orderDal.Update(orderFromDb);

            var subOrders = _subOrderDal.GetAll(x => x.OrderId == order.Id);
            if (subOrders != null && subOrders.Count > 0)
            {
                for (int i = 0; i < subOrders.Count; i++)
                    subOrders[i].SubOrderStatus = 2;
                _subOrderDal.UpdateRange(subOrders);
            }

            if (!string.IsNullOrWhiteSpace(orderFromDb.Email))
                _mailService.OrderShippedToCustomer(orderFromDb.Email,orderFromDb.OrderCode ?? "");

            return new SuccessResult();
        }
        private IDataResult<Order> GetOrderTracking(Order order)
        {
            var result = _orderDal.GetAsNoTracking(x => x.OrderCode == order.OrderCode && x.Email == order.Email);
            if (result != null)
                return new SuccessDataResult<Order>(result);

            return new ErrorDataResult<Order>();
        }
    }
}
