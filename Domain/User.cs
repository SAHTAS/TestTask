using System;

namespace Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? BlockedDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int UserGroupId { get; set; }
        public int UserStateId { get; set; }

        public UserGroup UserGroup { get; set; }
        public UserState UserState { get; set; }
    }
}