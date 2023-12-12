using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Dtos.User;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualPOS.Iyzico.Abstract
{
    public interface IIyzicoPaymentService
    {
        IDataResult<Options> GetOptions();
        IDataResult<CreateCheckoutFormInitializeRequest> MappingBuyer(UserDto userDto, CreateCheckoutFormInitializeRequest request);
        IDataResult<CreateCheckoutFormInitializeRequest> MappingAddress(UserDto userDto, CreateCheckoutFormInitializeRequest request);
        IResult Payment(UserDto userDto, List<BasketItem> basketItem);
        IResult Test();
    }
}
