﻿using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Item_Counting_Type.Input_Objects
{
    public class Add_Item_Counting_Type_Data
    {
        public Session_Data session { get; set; }

        public string counting_type { get; set; }
    }
}
