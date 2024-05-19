using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Invoice.Input_Objects
{
    public class Invoice_Issuer_Data
    {
        public Session_Data session { get; set; }
        public long order_id { get; set; }

        //True - Lease | False - Sale/Service
        public bool invoice_type { get; set; }
        public string language_tag { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public DateTime issue_date { get; set; }
        public DateTime sale_date { get; set; }
        public DateTime payment_date { get; set; }
        public string payment_method { get; set; }
        public double discount { get; set; }
        public string comment { get; set; }
    }
}
