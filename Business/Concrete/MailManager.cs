using Business.Abstract;
using Business.Constans.Html.Mail;
using Core.Entities.Concrete;
using Core.Utilities.Helpers.MailHelper;
using Core.Utilities.IoC;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.User;
using Entities.Concrete;
using Entities.Dtos.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class MailManager : IMailService
    {
        Mail mailEntity;
        IMailHelper _mailHelper;
        private IHttpContextAccessor _httpContextAccessor;
        public IConfiguration Configuration { get;}

        public MailManager(IMailHelper mailHelper, IConfiguration config)
        {
            _mailHelper = mailHelper;
            Configuration = config;
            mailEntity = Configuration.GetSection("Mail").Get<Mail>();
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        public IResult ConstantSendMail(MailDto mailDto)
        {
            if (mailDto != null)
            {
                Mail sendMail = new Mail()
                {
                    MailSender = mailEntity.MailSender,
                    SenderPassword = mailEntity.SenderPassword,
                    SenderSmtp = mailEntity.SenderSmtp,
                    SenderPort = mailEntity.SenderPort,
                    MailRecipientList = mailEntity.MailRecipientList
                };

                mailEntity.MailSubject = mailDto.MailTitle;
                mailEntity.MailHtmlBody = mailDto.MailBody;
                _mailHelper.SendMail(mailEntity, mailDto);

                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult CreateOrder(string orderCode, string recipientEmail)
        {
            MailDto mailDto = new MailDto();
            mailDto.Email = recipientEmail;
            mailDto.MailTitle = "Siparişiniz Başarıyla Alınmıştır";
            mailDto.MailBody = MailHtml.CreateOrder(orderCode);
            var sendMailResult = VariableSendMail(mailDto);
            if (sendMailResult.Success)
                return new SuccessResult();
            return new ErrorResult();
        }

        public IResult Register(string firstName, string lastName, string email)
        {
            MailDto mailDto = new MailDto();
            mailDto.MailTitle = "Yeni Üye";
            mailDto.MailBody = MailHtml.Register(firstName, lastName, email);
            var sendMailResult = ConstantSendMail(mailDto);
            if (sendMailResult.Success)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult CancelOrder(string firstName, string lastName, string orderCode)
        {
            MailDto mailDto = new MailDto();
            mailDto.MailTitle = "Sipar İptali";
            mailDto.MailBody = MailHtml.CancelOrder(firstName, lastName, ClaimHelper.GetUserEmail(_httpContextAccessor.HttpContext), orderCode);
            var sendMailResult = ConstantSendMail(mailDto);
            if (sendMailResult.Success)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult RefundingProduct(string firstName, string lastName, int variantId)
        {
            MailDto mailDto = new MailDto();
            mailDto.MailTitle = "Ürün İadesi";
            mailDto.MailBody = MailHtml.RefundingProduct(firstName, lastName, ClaimHelper.GetUserEmail(_httpContextAccessor.HttpContext), variantId);
            var sendMailResult = ConstantSendMail(mailDto);
            if (sendMailResult.Success)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult OrderShipped(string orderCode)
        {
            MailDto mailDto = new MailDto();
            mailDto.MailTitle = "Siparişiniz Kargoya Verildi";
            mailDto.MailBody = MailHtml.OrderShipped(orderCode);
            var sendMailResult = VariableSendMail(mailDto);
            if (sendMailResult.Success)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult OrderShippedToCustomer(string recipientEmail, string orderCode)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                return new ErrorResult("Alıcı e-posta adresi bulunamadı.");
            MailDto mailDto = new MailDto();
            mailDto.Email = recipientEmail;
            mailDto.MailTitle = "Siparişiniz Kargoya Verildi";
            mailDto.MailBody = MailHtml.OrderShipped(orderCode);
            var sendMailResult = VariableSendMail(mailDto);
            if (sendMailResult.Success)
                return new SuccessResult();
            return new ErrorResult();
        }

        public IResult VariableSendMail(MailDto mailDto)
        {
            if (mailDto != null)
            {
                Mail sendMail = new Mail()
                {
                    MailSender = mailEntity.MailSender,
                    SenderPassword = mailEntity.SenderPassword,
                    SenderSmtp = mailEntity.SenderSmtp,
                    SenderPort = mailEntity.SenderPort,
                    MailRecipientList = new List<string>()
                };

                //sendMail.MailRecipientList.Add(ClaimHelper.GetUserEmail(_httpContextAccessor.HttpContext));
                if (mailDto.Email == null)
                {
                    sendMail.MailRecipientList.Add("murat.oztrkk01@gmail.com");
                }
                else
                {
                    sendMail.MailRecipientList.Add(mailDto.Email);
                }

                mailEntity.MailSubject = mailDto.MailTitle;
                mailEntity.MailHtmlBody = mailDto.MailBody;
                _mailHelper.SendMail(mailEntity, mailDto);

                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult AdminCancelOrder()
        {
            MailDto mailDto = new MailDto();
            mailDto.MailTitle = "Siparişiniz İptal Edilmiştir.";
            mailDto.MailBody = MailHtml.AdminCancelOrder();
            VariableSendMail(mailDto);
            return new SuccessResult();
        }

        public IResult AdminRefundingProduct()
        {
            MailDto mailDto = new MailDto();
            mailDto.MailTitle = "Ürün Siparişiniz İptal Edilmiştir.";
            mailDto.MailBody = MailHtml.AdminRefundingProduct();
            VariableSendMail(mailDto);
            return new SuccessResult();

        }

        public IResult ForgotPasswordCode(string email, string code)
        {
            MailDto mailDto = new MailDto();
            mailDto.MailTitle = "Şifre sıfırlama kodunuz";
            mailDto.MailBody = MailHtml.ForgotPasswordCode(code);
            mailDto.Email = email;
            VariableSendMail(mailDto);
            return new SuccessResult();
        }
        public IResult SendPaswordResetLink(string email, string link)
        {
            MailDto mailDto = new MailDto();
            mailDto.MailTitle = "Şifre Sıfırlama Linki";
            mailDto.MailBody = MailHtml.ResetPasswordLink(link);
            mailDto.Email = email;
            VariableSendMail(mailDto);
            return new SuccessResult();
        }
    }
}
