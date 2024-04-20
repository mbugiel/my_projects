export interface Output_Item_On_Receipt_Model
{

    id : bigint;
    catalog_number : string;
    product_name : string;
    counting_type : string;
    count : DoubleRange;
    weight : DoubleRange;
    summary_weight : DoubleRange;
    annotation : string;

}