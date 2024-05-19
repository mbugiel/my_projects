using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Invoice.Input_Objects
{
    public class Add_Invoice_Data
    {
        public Session_Data session { get; set; }

        public string prefix { get; set; }

        public bool sale_lease { get; set; }

        public int year { get; set; }

        public int month { get; set; }

        public long number { get; set; }

        public long order_id_FK { get; set; }

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
