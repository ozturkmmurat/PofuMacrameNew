using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EntityParameter.PasswordReset
{
    public class PasswordResetParameter
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string CodeUrl { get; set; }
    }
}
