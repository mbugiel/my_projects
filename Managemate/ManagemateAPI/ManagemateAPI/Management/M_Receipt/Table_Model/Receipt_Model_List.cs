namespace ManagemateAPI.Management.M_Receipt.Table_Model
{
    public class Receipt_Model_List
    {
        public long id { get; set; }

        public bool in_out { get; set; }

        public bool reservation { get; set; }

        public DateTime date { get; set; }

        public string element { get; set; }

        public string transport { get; set; }

        public double summary_weight { get; set; }

        public string comment { get; set; }

    }
}
