using System.Collections.Generic;
using Domain.Enums;

namespace Domain
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public UserGroupCode Code { get; set; }
        public string Description { get; set; }

        public List<User> Users { get; set; }
    }
}