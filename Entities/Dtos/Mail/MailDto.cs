using Core.Entities;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Mail
{
    public class MailDto : UserMail, IDto
    {
        public string MailTitle { get; set; }
    }
}
