using System.Collections.Generic;
using Domain.Enums;
using Newtonsoft.Json;

namespace Domain
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public UserGroupCode Code { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}