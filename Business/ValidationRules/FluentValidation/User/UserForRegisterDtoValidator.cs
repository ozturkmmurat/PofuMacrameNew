using Entities.Dtos.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation.User
{
    public class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterDtoValidator()
        {

            RuleFor(u => u.FirstName).NotEmpty().WithMessage("Ad alanı boş bırakılamaz").WithName("Ad");
            RuleFor(u => u.FirstName).NotEmpty().MinimumLength(3).MaximumLength(20).WithMessage("Ad alanı en az 3 en fazla 20 karakter olabilir").WithName("Ad");
            RuleFor(u => u.LastName).NotEmpty().WithMessage("Ad alanı boş bırakılamaz").WithName("Soyad");
            RuleFor(u => u.LastName).NotEmpty().MinimumLength(3).MaximumLength(20).WithMessage("Soyad alanı en az 3 en fazla 20 karakter olabilir").WithName("Soyad");
            RuleFor(u => u.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir mail adresi giriniz").WithName("Email");
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6).MaximumLength(20).WithMessage("Şifreniz en az 6 karakter en fazla 20 karakter olabilir.").WithName("Şifre");
            RuleFor(u => u.Password).NotEmpty().Matches("[A-Z]").WithMessage("Şifreniz bir veya daha fazla büyük harf içermelidir.")
            .Matches(@"\d").WithMessage("Şifreniz bir veya daha fazla rakam içermelidir.").WithName("Şifre");
        }
    }
}