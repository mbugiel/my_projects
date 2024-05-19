namespace ManagemateAPI.Management.M_Invoice.Table_Model
{
    public class Invoice_Model_List
    {
        public long id { get; set; }

        public string prefix { get; set; }

        public long year { get; set; }

        public long month { get; set; }

        public long number { get; set; }

        public string order_name_FK { get; set; }

        public DateTime issue_date { get; set; }

        public DateTime sale_date { get; set; }

        public double net_worth { get; set; }

        public double gross_worth { get; set; }

        public string comment { get; set; }

    }
}
