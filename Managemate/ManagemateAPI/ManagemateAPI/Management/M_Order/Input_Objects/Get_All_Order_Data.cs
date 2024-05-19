using ManagemateAPI.Management.M_Session.Input_Objects;
using System.ComponentModel.DataAnnotations;

namespace ManagemateAPI.Management.M_Order.Input_Objects
{
    public class Get_All_Order_Data
    {
        public Session_Data session { get; set; }

        public bool all_or_active_only { get; set; }

        //public int pageid { get; set; }

        //public int pageSize { get; set; }

    }
}

