﻿namespace AuthenticationAPI.Helper.Service
{
    public class AddUserData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool TwoStepLogin { get; set; }
        public string EmailToken { get; set; }

    }
}