using ManagemateAPI.Management.M_Client.Table_Model;
using ManagemateAPI.Management.M_Construction_Site.Table_Model;

namespace ManagemateAPI.Management.M_Order.Table_Model
{
    public class Order_Model
    {
        public long id { get; set; }

        public string order_name { get; set; }

        public Client_Model client_id_FK { get; set; }

        public Construction_Site_Model construction_site_id_FK { get; set; }

        public int status { get; set; }

        public DateTime creation_date { get; set; }

        public string default_payment_method { get; set; }
        public int default_payment_date_offset { get; set; }
        public double default_discount { get; set; }

        public string comment { get; set; }
    }

}
