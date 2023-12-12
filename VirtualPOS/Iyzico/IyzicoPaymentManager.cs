using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Entities.Dtos.User;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualPOS.Iyzico.Abstract;

namespace VirtualPOS.Iyzico
{
    public class IyzicoPaymentManager : IIyzicoPaymentService
    {
        public IDataResult<Iyzipay.Options> GetOptions()
        {
            Iyzipay.Options options = new Iyzipay.Options();
            options.ApiKey = "sandbox-JhWWKjfLizjGbynSPDMeNfpl6MVSnFKz\r\n";
            options.SecretKey = "sandbox-KKyHHykgnBolgi1fqX1dlFLZTSNbeUR7\r\n";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";
            return new SuccessDataResult<Iyzipay.Options>(options);
        }

        public IDataResult<CreateCheckoutFormInitializeRequest> MappingAddress(UserDto userDto, CreateCheckoutFormInitializeRequest request)
        {
            if (userDto != null)
            {
                Address shippingAddress = new Address();
                shippingAddress.ContactName = userDto.FirstName + " " + userDto.LastName;
                shippingAddress.City = userDto.UserCity;
                shippingAddress.Country = "Turkey";
                shippingAddress.Description = userDto.Address;
                shippingAddress.ZipCode = userDto.PostCode;
                request.ShippingAddress = shippingAddress;

                Address billingAddress = new Address();
                billingAddress.ContactName = userDto.FirstName + " " + userDto.LastName;
                billingAddress.City = userDto.UserCity;
                billingAddress.Country = "Turkey";
                billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
                billingAddress.ZipCode = userDto.PostCode;
                request.BillingAddress = billingAddress;
                
                return new SuccessDataResult<CreateCheckoutFormInitializeRequest>(request);
            }
            return new ErrorDataResult<CreateCheckoutFormInitializeRequest>();
        }

        public IDataResult<CreateCheckoutFormInitializeRequest> MappingBuyer(UserDto userDto, CreateCheckoutFormInitializeRequest request)
        {
            if (userDto != null)
            {
                Buyer buyer = new Buyer();
                buyer.Id = userDto.UserId.ToString();
                buyer.Name = userDto.FirstName;
                buyer.Surname = userDto.LastName;
                buyer.GsmNumber = "+905350000000";
                buyer.Email = userDto.Email;
                buyer.IdentityNumber = "74300864791";
                buyer.LastLoginDate = "2015-10-05 12:43:35";
                buyer.RegistrationDate = "2013-04-21 15:12:09";
                buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
                buyer.Ip = "85.34.78.112";
                buyer.City = "Istanbul";
                buyer.Country = "Turkey";
                buyer.ZipCode = userDto.PostCode;
                request.Buyer = buyer;

                return new SuccessDataResult<CreateCheckoutFormInitializeRequest>(request);
            }
            return new ErrorDataResult<CreateCheckoutFormInitializeRequest>();
        }

        public IResult Payment(UserDto userDto, List<BasketItem> basketItem)
        {
            if (userDto != null && basketItem != null)
            {
                CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
                request.Locale = Locale.TR.ToString();
                request.ConversationId = "123456789";
                request.Price = "1";
                request.PaidPrice = "1.2";
                request.Currency = Currency.TRY.ToString();
                request.BasketId = "B67832";
                request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
                request.CallbackUrl = "https://www.merchant.com/callback";

                var buyerResult = MappingBuyer(userDto, request);
                if (buyerResult.Success)
                {
                    var addressResult = MappingAddress(userDto, request);
                    if (addressResult.Success)
                    {
                        for (int i = 0; i < basketItem.Count; i++)
                        {
                            request.BasketItems.Add(basketItem[i]);
                        }
                        CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, GetOptions().Data);
                    }
                }
            }
            throw new NotImplementedException();
        }

        public IResult Test()
        {
            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = "1";
            request.PaidPrice = "1.2";
            request.Currency = Currency.TRY.ToString();
            request.BasketId = "B67832";
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "https://www.merchant.com/callback";


            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "0.3";
            basketItems.Add(firstBasketItem);

            BasketItem secondBasketItem = new BasketItem();
            secondBasketItem.Id = "BI102";
            secondBasketItem.Name = "Game code";
            secondBasketItem.Category1 = "Game";
            secondBasketItem.Category2 = "Online Game Items";
            secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
            secondBasketItem.Price = "0.5";
            basketItems.Add(secondBasketItem);

            BasketItem thirdBasketItem = new BasketItem();
            thirdBasketItem.Id = "BI103";
            thirdBasketItem.Name = "Usb";
            thirdBasketItem.Category1 = "Electronics";
            thirdBasketItem.Category2 = "Usb / Cable";
            thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            thirdBasketItem.Price = "0.2";
            basketItems.Add(thirdBasketItem);
            request.BasketItems = basketItems;

            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, GetOptions().Data);
            return new SuccessResult();
        }
    }
}
