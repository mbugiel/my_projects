using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Receipt.Input_Objects
{
    public class Edit_Receipt_Data
    {
        public Session_Data session { get; set; }

        public long receipt_id { get; set; }

        public string element { get; set; }

        public DateTime date { get; set; }

        public string transport { get; set; }

        public string comment { get; set; }

    }
}
