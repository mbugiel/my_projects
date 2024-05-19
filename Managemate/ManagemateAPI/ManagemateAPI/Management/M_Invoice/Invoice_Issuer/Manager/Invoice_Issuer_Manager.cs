using ManagemateAPI.Information;

namespace ManagemateAPI.Management.M_Invoice.Invoice_Issuer.Manager
{
    public class Invoice_Issuer_Manager
    {

        public static string? INVOICE_CSS = null;
        public static string? INVOICE_INVOICEBODY = null;
        public static string? INVOICE_HEADER = null;
        public static string? INVOICE_SELLERINFO = null;
        public static string? INVOICE_CLIENTINFO = null;
        public static string? INVOICE_MAINTABLE = null;
        public static string? INVOICE_MAINTABLEHEAD = null;
        public static string? INVOICE_MAINTABLEFOOTER = null;
        public static string? INVOICE_SUMMARYTABLE = null;
        public static string? INVOICE_FOOTER = null;

        public static string? INVOICE_LANGUAGE_PL = null;
        public static string? INVOICE_LANGUAGE_EN = null;


        public static string GET_INVOICE_CSS()
        {

            if(INVOICE_CSS == null || INVOICE_CSS == "") 
            {
                Read_Files();
            }

            return INVOICE_CSS;

        }

        public static string GET_INVOICE_INVOICEBODY()
        {

            if (INVOICE_INVOICEBODY == null || INVOICE_INVOICEBODY == "")
            {
                Read_Files();
            }

            return INVOICE_INVOICEBODY;

        }

        public static string GET_INVOICE_HEADER()
        {

            if (INVOICE_HEADER == null || INVOICE_HEADER == "")
            {
                Read_Files();
            }

            return INVOICE_HEADER;

        }

        public static string GET_INVOICE_SELLERINFO()
        {

            if (INVOICE_SELLERINFO == null || INVOICE_SELLERINFO == "")
            {
                Read_Files();
            }

            return INVOICE_SELLERINFO;

        }

        public static string GET_INVOICE_CLIENTINFO()
        {

            if (INVOICE_CLIENTINFO == null || INVOICE_CLIENTINFO == "")
            {
                Read_Files();
            }

            return INVOICE_CLIENTINFO;

        }

        public static string GET_INVOICE_MAINTABLE()
        {

            if (INVOICE_MAINTABLE == null || INVOICE_MAINTABLE == "")
            {
                Read_Files();
            }

            return INVOICE_MAINTABLE;

        }

        public static string GET_INVOICE_MAINTABLEHEAD()
        {

            if (INVOICE_MAINTABLEHEAD == null || INVOICE_MAINTABLEHEAD == "")
            {
                Read_Files();
            }

            return INVOICE_MAINTABLEHEAD;

        }

        public static string GET_INVOICE_MAINTABLEFOOTER()
        {

            if (INVOICE_MAINTABLEFOOTER == null || INVOICE_MAINTABLEFOOTER == "")
            {
                Read_Files();
            }

            return INVOICE_MAINTABLEFOOTER;

        }

        public static string GET_INVOICE_SUMMARYTABLE()
        {

            if (INVOICE_SUMMARYTABLE == null || INVOICE_SUMMARYTABLE == "")
            {
                Read_Files();
            }

            return INVOICE_SUMMARYTABLE;

        }

        public static string GET_INVOICE_FOOTER()
        {

            if (INVOICE_FOOTER == null || INVOICE_FOOTER == "")
            {
                Read_Files();
            }

            return INVOICE_FOOTER;

        }

        public static string GET_INVOICE_LANGUAGE(string language)
        {

            if (INVOICE_LANGUAGE_PL == null || INVOICE_LANGUAGE_PL == "" || INVOICE_LANGUAGE_EN == null || INVOICE_LANGUAGE_EN == "")
            {
                Read_Files();
            }

            return language == "pl" ? INVOICE_LANGUAGE_PL : INVOICE_LANGUAGE_EN;

        }


        private static void Read_Files()
        {

            if 
            (
                File.Exists(System_Path.INVOICE_CSS) &&
                File.Exists(System_Path.INVOICE_INVOICEBODY) &&
                File.Exists(System_Path.INVOICE_HEADER) &&
                File.Exists(System_Path.INVOICE_SELLERINFO) &&
                File.Exists(System_Path.INVOICE_CLIENTINFO) &&
                File.Exists(System_Path.INVOICE_MAINTABLE) &&
                File.Exists(System_Path.INVOICE_MAINTABLEHEAD) &&
                File.Exists(System_Path.INVOICE_MAINTABLEFOOTER) &&
                File.Exists(System_Path.INVOICE_SUMMARYTABLE) &&
                File.Exists(System_Path.INVOICE_FOOTER) &&
                File.Exists(System_Path.INVOICE_LANGUAGE_FOLDER+"pl.json") &&
                File.Exists(System_Path.INVOICE_LANGUAGE_FOLDER+"en.json")
            )
            {
                INVOICE_CSS = File.ReadAllText(System_Path.INVOICE_CSS);

                INVOICE_INVOICEBODY = File.ReadAllText(System_Path.INVOICE_INVOICEBODY);

                INVOICE_HEADER = File.ReadAllText(System_Path.INVOICE_HEADER);

                INVOICE_SELLERINFO = File.ReadAllText(System_Path.INVOICE_SELLERINFO);

                INVOICE_CLIENTINFO = File.ReadAllText(System_Path.INVOICE_CLIENTINFO);

                INVOICE_MAINTABLE = File.ReadAllText(System_Path.INVOICE_MAINTABLE);

                INVOICE_MAINTABLEHEAD = File.ReadAllText(System_Path.INVOICE_MAINTABLEHEAD);

                INVOICE_MAINTABLEFOOTER = File.ReadAllText(System_Path.INVOICE_MAINTABLEFOOTER);

                INVOICE_SUMMARYTABLE = File.ReadAllText(System_Path.INVOICE_SUMMARYTABLE);

                INVOICE_FOOTER = File.ReadAllText(System_Path.INVOICE_FOOTER);

                INVOICE_LANGUAGE_PL = File.ReadAllText(System_Path.INVOICE_LANGUAGE_FOLDER+"pl.json");

                INVOICE_LANGUAGE_EN = File.ReadAllText(System_Path.INVOICE_LANGUAGE_FOLDER+"en.json");

            }
            else
            {

                throw new Exception("26"); // file read error

            }

        }

    }
}
