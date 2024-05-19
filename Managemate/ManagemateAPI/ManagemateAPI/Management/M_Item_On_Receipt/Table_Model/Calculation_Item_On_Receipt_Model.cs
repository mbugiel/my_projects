using ManagemateAPI.Database.Tables;

namespace ManagemateAPI.Management.M_Item_On_Receipt.Table_Model
{
    public class Calculation_Item_On_Receipt_Model
    {
        public long id { get; set; }

        public long item_id { get; set; }

        public double count { get; set; }

        public bool overwritten { get; set; }

        public int days_on_construction_site { get; set; }

        public DateTime receipt_date { get; set; }
    }
}
