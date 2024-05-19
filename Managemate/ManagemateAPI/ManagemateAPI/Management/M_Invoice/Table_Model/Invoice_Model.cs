using ManagemateAPI.Database.Tables;

namespace ManagemateAPI.Management.M_Invoice.Table_Model
{
    public class Invoice_Model
    {
        public long id { get; set; }

        public string prefix { get; set; }

        public bool sale_lease {  get; set; }

        public int year { get; set; }

        public int month { get; set; }

        public long number { get; set; }

        public string order_name_FK { get; set; }

        public DateTime issue_date { get; set; }

        public DateTime sale_date { get; set; }

        public DateTime payment_date { get; set; }

        public string payment_method { get; set; }

        public double discount { get; set; }

        public double net_worth { get; set; }

        public double tax_worth { get; set; }

        public double gross_worth { get; set; }

        public string comment { get; set; }

        public string comment_2 { get; set; }

    }
}
