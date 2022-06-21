using System;

namespace API.Models
{
    public class GetUserResponseModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }

        public DateTime CreatedDate { get; set; }
        public int UserGroupId { get; set; }
        public int UserStateId { get; set; }

        public UserGroupResponseModel UserGroup { get; set; }
        public UserStateResponseModel UserState { get; set; }
    }
}