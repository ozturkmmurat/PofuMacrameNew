using Core.Utilities.Result.Abstract;
using Entities.Dtos.Mail;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IMailService
    {
        IResult ConstantSendMail(MailDto mailDto);
        IResult VariableSendMail(MailDto mailDto);
        IResult Register(string firstName, string lastName, string email);
        IResult ForgotPasswordCode(string email, string code);
        IResult SendPaswordResetLink(string email, string link);
        IResult CreateOrder(string orderCode, string recipientEmail);
        IResult CancelOrder(string firstName, string lastName, string orderCode);
        IResult RefundingProduct(string firstName, string lastName, int variantId);
        IResult OrderShipped(string orderCode);
        IResult OrderShippedToCustomer(string? recipientEmail, string orderCode);
        IResult AdminCancelOrder();
        IResult AdminRefundingProduct();
    }
}
