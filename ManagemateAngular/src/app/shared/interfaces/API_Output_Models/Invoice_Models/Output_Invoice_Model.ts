export interface Output_Invoice_Model
{

    id : bigint;
    prefix : string;
    year : bigint
    month : bigint;
    number : bigint;
    order_name_FK : string;
    issue_date : Date;
    sale_date : Date;
    net_worth : DoubleRange;
    gross_worth : DoubleRange;
    comment : string;

}