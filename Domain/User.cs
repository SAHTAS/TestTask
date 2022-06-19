using System;
using Newtonsoft.Json;

namespace Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserGroupId { get; set; }
        public int UserStateId { get; set; }

        public UserGroup UserGroup { get; set; }
        public UserState UserState { get; set; }
    }
}