using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Item_On_Receipt.Input_Objects
{
    public class Edit_Item_On_Receipt_Data
    {
        public Session_Data session { get; set; }

        public long id { get; set; }

        public long item_id_FK { get; set; }

        public double count { get; set; }

        public double summary_weight { get; set; }

        public string annotation { get; set; }

    }
}
