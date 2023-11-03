using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class UserAddress : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NeighbourhoodId { get; set; }
        public string AddressTitle { get; set; }
    }
}
