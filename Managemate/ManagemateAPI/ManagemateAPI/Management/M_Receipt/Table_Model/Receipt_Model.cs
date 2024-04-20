using ManagemateAPI.Management.M_Item_On_Receipt.Table_Model;

namespace ManagemateAPI.Management.M_Receipt.Table_Model
{
    public class Receipt_Model
    {
        public long id { get; set; }

        public bool in_out { get; set; }

        public DateTime date { get; set; }

        public string element { get; set; }

        public string transport { get; set; }

        public double summary_weight { get; set; }

        public string comment { get; set; }

        public List<Item_On_Receipt_Model> items_on_receipt { get; set; }

    }
}
