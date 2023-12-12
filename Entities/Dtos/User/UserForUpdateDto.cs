using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.User
{
    public class UserForUpdateDto : IDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string OldPassword { get; set; }
        public string AgainNewPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
    }
}
