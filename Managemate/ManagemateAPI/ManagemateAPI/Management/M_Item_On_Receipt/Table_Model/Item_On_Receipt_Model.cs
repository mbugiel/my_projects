using ManagemateAPI.Database.Tables;

namespace ManagemateAPI.Management.M_Item_On_Receipt.Table_Model
{
    public class Item_On_Receipt_Model
    {
        public long id { get; set; }

        public string catalog_number { get; set; }

        public string product_name { get; set; }

        public string counting_type { get; set; }

        public double count { get; set; }

        public double weight { get; set; }

        public double summary_weight { get; set; }

        public string annotation { get; set; }
    }
}
