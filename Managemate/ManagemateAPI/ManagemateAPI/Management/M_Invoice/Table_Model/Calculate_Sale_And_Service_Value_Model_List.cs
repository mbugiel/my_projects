namespace ManagemateAPI.Management.M_Invoice.Table_Model
{
    public class Calculate_Sale_And_Service_Value_Model_List
    {
        public long item_id { get; set; }
        public string name { get; set; }
        public double count { get; set; }
        public double net_worth { get; set; }
        public double tax_pct { get; set; }
        public double tax_worth { get; set; }
        public double gross_worth { get; set; }


    }
}
