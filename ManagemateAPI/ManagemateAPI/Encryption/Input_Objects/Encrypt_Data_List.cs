﻿using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Encryption.Input_Objects
{
    public class Encrypt_Data_List
    {
        public Session_Data SessionData { get; set; }
        public List<Decrypted_Object> DataToEncrypt { get; set; }
    }
}
