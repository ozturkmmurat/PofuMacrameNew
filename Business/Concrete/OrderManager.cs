using Business.Abstract;
using Business.Utilities;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        IOrderDal _orderDal;
        ISubOrderDal _subOrderDal;
        public OrderManager(
            IOrderDal orderDal,
            ISubOrderDal subOrderDal)
        {
            _orderDal = orderDal;
            _subOrderDal = subOrderDal;
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

        public IDataResult<Order> OrderCode(string orderCode)
        {
            var result = _orderDal.Get(x => x.OrderCode == orderCode);
            if (result != null)
            {
                return new SuccessDataResult<Order>(result);
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
                foreach (var subOrder in orderDto.subOrders)
                {
                    resultSubOrder = new SubOrder
                    {
                        OrderId = mappingOrder.Data.Id,
                        Price = subOrder.Price,
                        VariantId = subOrder.VariantId
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
                foreach (var subOrder in orderDto.subOrders)
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
    }
}
