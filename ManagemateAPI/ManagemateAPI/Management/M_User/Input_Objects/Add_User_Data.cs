﻿namespace ManagemateAPI.Management.M_User.Input_Objects
{
    public class Add_User_Data
    {

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool TwoStepLogin { get; set; }
        public string EmailToken { get; set; }

    }
}
