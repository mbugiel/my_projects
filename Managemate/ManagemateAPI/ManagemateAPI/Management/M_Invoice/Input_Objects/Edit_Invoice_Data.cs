using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Invoice.Input_Objects
{
    public class Edit_Invoice_Data
    {
        public Session_Data session { get; set; }

        public long id { get; set; }

        public string prefix { get; set; }

        public long year { get; set; }

        public long month { get; set; }

        public long number { get; set; }

        public DateOnly issue_date { get; set; }

        public DateOnly sale_date { get; set; }

        public DateOnly payment_date { get; set; }

        public string payment_method { get; set; }

        public double discount { get; set; }

        public double net_worth { get; set; }

        public double tax_worth { get; set; }

        public double gross_worth { get; set; }

        public string comment { get; set; }

        public string comment_2 { get; set; }
    }
}
