﻿using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Item.Input_Objects
{
    public class Get_All_Item_Data
    {
        public Session_Data session { get; set; }

        public bool get_item {  get; set; }
    }
}
