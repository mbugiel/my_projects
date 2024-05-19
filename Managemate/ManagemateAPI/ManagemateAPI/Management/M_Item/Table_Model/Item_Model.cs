using ManagemateAPI.Database.Tables;
using ManagemateAPI.Management.M_Item_Counting_Type.Table_Model;
using ManagemateAPI.Management.M_Item_Trading_Type.Table_Model;

namespace ManagemateAPI.Management.M_Item.Table_Model
{
    public class Item_Model
    {
        public long id { get; set; }
        public string catalog_number { get; set; }
        public string product_name { get; set; }
        public string item_type { get; set; }
        public double weight_kg { get; set; }
        public double count { get; set; }
        public double blocked_count { get; set; }
        public string price { get; set; }
        public double tax_pct { get; set; }
        public Item_Trading_Type_Model item_trading_type_id_FK { get; set; }
        public Item_Counting_Type_Model item_counting_type_id_FK { get; set; }
        public string comment { get; set; }

    }
}
