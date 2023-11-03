using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.User
{
    public class UserDto : IDto
    {
        public int UserId { get; set; }
        public int UserOperationClaimId { get; set; }
        public int OperationClaimId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string OperationClaimName { get; set; }
    }
}
