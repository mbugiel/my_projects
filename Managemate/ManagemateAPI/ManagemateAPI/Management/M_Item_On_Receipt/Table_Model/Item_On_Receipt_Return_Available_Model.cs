using ManagemateAPI.Database.Tables;

namespace ManagemateAPI.Management.M_Item_On_Receipt.Table_Model
{
    public class Item_On_Receipt_Return_Available_Model
    {

        public string catalog_number { get; set; }

        public string product_name { get; set; }

        public string counting_type { get; set; }

        public long item_id { get; set; }

        public double available_count { get; set; }

        public double weight { get; set; }

    }
}
