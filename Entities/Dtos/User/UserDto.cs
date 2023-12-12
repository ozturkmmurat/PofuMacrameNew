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
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public string OperationClaimName { get; set; }
        public string UserCity { get; set; }
        public string Country { get; set; }
        public string AddressTitle { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
