﻿namespace ManagemateAPI.Mail.Input_Objects
{
    public class Send_Email_Data
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool TwoStepLogin { get; set; }
        public int Template { get; set; }

    }
}
