using Entities.Dtos.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation.User
{
    public class UserPasswordResetDtoValidator : AbstractValidator<UserPasswordResetDto>
    {
        public UserPasswordResetDtoValidator()
        {
            RuleFor(u => u.NewPassword).Equal(u => u.AgainNewPassword).WithMessage("Yeni şifre ve şifre tekrarı ile aynı olmalı.").WithName("Şifre Tekrarı");
            RuleFor(u => u.NewPassword).MinimumLength(6).MaximumLength(20).WithMessage("Yeni Şifreniz en az 6 karakter en fazla 20 karakter olabilir.").WithName("Yeni Şifre");
            RuleFor(u => u.NewPassword).Matches("[A-Z]").WithMessage("Yeni Şifreniz bir veya daha fazla büyük harf içermelidir.")
            .Matches(@"\d").WithMessage("Şifreniz bir veya daha fazla rakam içermelidir.").WithName("Yeni şifre");
        }
    }
}
