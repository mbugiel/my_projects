using ManagemateAPI.Management.M_Session.Input_Objects;
using System.ComponentModel.DataAnnotations;

namespace ManagemateAPI.Management.M_Order.Input_Objects
{
    public class Add_Order_Data
    {
        public Session_Data session { get; set; }

        public string order_name { get; set; }

        public long client_id_FK { get; set; }

        public long construction_site_id_FK { get; set; }

        public int status { get; set; }

        //public DateTime creation_date { get; set; }

        public string comment { get; set; }

    }
}
