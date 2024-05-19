using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Invoice.Input_Objects
{
    public class Calculate_Lease_Value_Data
    {
        public Session_Data session { get; set; }

        public long order_id { get; set; }

        public int month {  get; set; }
        public int year { get; set; }

    }
}
