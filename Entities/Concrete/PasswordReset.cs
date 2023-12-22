using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    //User tablosuyla birebir iliskili
    public class PasswordReset : IEntity
    {
        public int UserId { get; set; }
        public int Status { get; set; }
        public string Url { get; set; }
        public string CodeUrl { get; set; }
        public string Code { get; set; }
        public DateTime ResetDate { get; set; }
        public DateTime? ResetEndDate { get; set; }


        public PasswordReset()
        {
            ResetDate = DateTime.Now;
        }
    }
}
