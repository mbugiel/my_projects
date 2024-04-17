using ManagemateAPI.Database.Tables;

namespace ManagemateAPI.Management.M_Item.Table_Model
{
    public class Item_Model_List
    {
        public long id { get; set; }
        public string catalog_number { get; set; }
        public string product_name { get; set; }
        public string item_type { get; set; }
        public double weight_kg { get; set; }
        public long count { get; set; }
        public long blocked_count { get; set; }
        public string price { get; set; }
        public double tax_pct { get; set; }
        public string trading_type_pl { get; set; }
        public string trading_type_eng { get; set; }
        public string counting_type { get; set; }
        public string comment { get; set; }
    }
}
