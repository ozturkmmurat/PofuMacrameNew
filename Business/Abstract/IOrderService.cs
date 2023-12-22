using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.Order;
using Entities.Dtos.Order.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IDataResult<List<Order>> GetAll();
        IDataResult<List<Order>> GetAllByUserId(int userId);
        IDataResult<List<SelectUserOrderDto>> GetAllUserOrderDto(); //Kullaniciin kendi siparislerini gormesi icin
        IDataResult<SelectUserOrderDto> GetUserOrderDtoDetail(int orderId, int userId);
        IDataResult<Order> GetById(int id);
        IDataResult<Order> GetByOrderIdUserId(int orderId, int userId);
        IDataResult<Order> OrderCode(string orderCode);
        IDataResult<string> CreateOrderCode(Order order);
        IDataResult<Order> MappingOrder(OrderDto orderDto);
        IResult Add(Order order);
        IResult TsaAdd(OrderDto orderDto);
        IResult Update(Order order);
        IResult TsaUpdate(OrderDto orderDto);
        IResult Delete(Order order);
    }
}
