using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Receipt.Input_Objects
{
    public class Add_Receipt_Data
    {

        public Session_Data session { get; set; }

        public bool in_out { get; set; }

        public long order_id_FK { get; set; }

        public string element { get; set; }

        public string transport { get; set; }

        public string comment { get; set; }

    }
}
