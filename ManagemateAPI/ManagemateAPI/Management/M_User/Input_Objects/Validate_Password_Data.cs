﻿using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_User.Input_Objects
{
    public class Validate_Password_Data
    {
        public Session_Data SessionData { get; set; }
        public string Password { get; set; }
    }
}
