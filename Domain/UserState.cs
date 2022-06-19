﻿using System.Collections.Generic;
using Domain.Enums;
using Newtonsoft.Json;

namespace Domain
{
    public class UserState
    {
        public int UserStateId { get; set; }
        public UserStateCode Code { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}