using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.User
{
    public class UserPasswordResetDto : IDto
    {
        public string Link { get; set; }
        public string NewPassword { get; set; }
        public string AgainNewPassword { get; set; }
    }
}
