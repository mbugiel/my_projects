using ManagemateAPI.Management.M_Session.Input_Objects;
using Microsoft.Extensions.Configuration;

namespace ManagemateAPI.Information
{
    public static class System_Path
    {
        public static string CURRENT_DIRECTORY = "#";
        public static string WRITE_DIRECTORY = "#";

        public static string AUTHENTICATION_API = "#";
        public static string HTML_TEMPLATE_PATH_1 = "#";
        public static string HTML_TEMPLATE_PATH_2 = "#";
        public static string ENCRYPT_KEYS_PATH = "#";
        public static string PASSWD_PATH = "#";
        public static string APPCODE_PATH = "#";


        //INVOICE\\
        public static string INVOICE_ELEMENTS_ROOT = "#";

        public static string INVOICE_CSS = INVOICE_ELEMENTS_ROOT + "invoice_css.html";
        public static string INVOICE_INVOICEBODY = INVOICE_ELEMENTS_ROOT + "invoice_invoiceBody.html";
        public static string INVOICE_HEADER = INVOICE_ELEMENTS_ROOT + "invoice_header.html";
        public static string INVOICE_SELLERINFO = INVOICE_ELEMENTS_ROOT + "invoice_sellerInfo.html";
        public static string INVOICE_CLIENTINFO = INVOICE_ELEMENTS_ROOT + "invoice_clientInfo.html";
        public static string INVOICE_MAINTABLE = INVOICE_ELEMENTS_ROOT + "invoice_mainTable.html";
        public static string INVOICE_MAINTABLEHEAD = INVOICE_ELEMENTS_ROOT + "invoice_mainTableHead.html";
        public static string INVOICE_MAINTABLEFOOTER = INVOICE_ELEMENTS_ROOT + "invoice_mainTableFooter.html";
        public static string INVOICE_SUMMARYTABLE = INVOICE_ELEMENTS_ROOT + "invoice_summaryTable.html";
        public static string INVOICE_FOOTER = INVOICE_ELEMENTS_ROOT + "invoice_footer.html";

        public static string INVOICE_LANGUAGE_FOLDER = INVOICE_ELEMENTS_ROOT + "Language/";
    }
}
