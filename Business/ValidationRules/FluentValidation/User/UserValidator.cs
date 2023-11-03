using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation.User
{
    public class UserValidator : AbstractValidator<Core.Entities.Concrete.User>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty().WithName("Ad");
            RuleFor(u => u.FirstName).MinimumLength(3).WithName("Ad");
            RuleFor(u => u.LastName).NotEmpty().WithName("Soyad");
            RuleFor(u => u.LastName).MinimumLength(3).WithName("Soyad");
            RuleFor(u => u.Email).NotEmpty().WithName("Email");
            RuleFor(u => u.Email).EmailAddress().WithMessage("Kullanıcı email adresi geçerli formatta değil.").WithName("Email");
        }
    }
}