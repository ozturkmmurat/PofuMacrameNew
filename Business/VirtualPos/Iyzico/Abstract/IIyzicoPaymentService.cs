using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.User;
using Entities.EntityParameter.CartItem;
using Entities.EntityParameter.Iyzico;
using Entities.LibraryEntities.Iyzico;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.VirtualPos.Iyzico.Abstract
{
    public interface IIyzicoPaymentService
    {
        IDataResult<Iyzipay.Options> GetOptions();
        IDataResult<CreateCheckoutFormInitializeRequest> MappingBuyer(UserDto userDto, TsaPaymentParameter tsaPaymentParameter, CreateCheckoutFormInitializeRequest request);
        IDataResult<CreateCheckoutFormInitializeRequest> MappingAddress(UserDto userDto, CreateCheckoutFormInitializeRequest request);
        IResult Test();
        IResult Test2(string x, string y);
        IDataResult<Object> TsaPayment(TsaPaymentParameter tsaPaymentParameter);
        IDataResult<CheckoutForm> PaymentResult(PaymentResultPostParameter paymentResultPostParameter);
        IDataResult<Refund> RefundProduct(ReturningProduct returningProduct);
        IDataResult<ReturningProduct> ShredJsonData(ReturningProduct returningProduct);
        IDataResult<Cancel> CancelOrder(CancelOrder cancelOrder);
        IDataResult<CancelOrder> ShredJsonData(CancelOrder cancelOrder);
    }
}
