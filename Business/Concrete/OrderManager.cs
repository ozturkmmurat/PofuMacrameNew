using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        IOrderDal _orderDal;
        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
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

        public IDataResult<Order> OrderCode(string orderCode)
        {
            var result = _orderDal.Get(x => x.OrderCode == orderCode);
            if (result != null)
            {
                return new SuccessDataResult<Order>(result);
            }
            return new ErrorDataResult<Order>();
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
