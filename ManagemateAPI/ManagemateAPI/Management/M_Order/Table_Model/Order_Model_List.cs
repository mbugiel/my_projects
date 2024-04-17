namespace ManagemateAPI.Management.M_Order.Table_Model
{
    public class Order_Model_List
    {
        public long id { get; set; }

        public string order_name { get; set; }

        public string client_name { get; set; }

        public string construction_site_name { get; set; }

        public int status { get; set; }

        public DateTime creation_date { get; set; }

        public string comment { get; set; }
    }

}
