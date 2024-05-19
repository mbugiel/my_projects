namespace ManagemateAPI.Management.M_Invoice.Invoice_Issuer.Models
{
    public class invoice_including_row_object
    {
        public double tax_pct { get; set; }
        public double net_total { get; set; }
        public double tax_total { get; set; }
        public double gross_total { get; set; }
    }
}
