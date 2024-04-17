export interface Output_Item_Model
{

    id : bigint;
    catalog_number : string;
    product_name : string;
    item_type : string;
    weight_kg : DoubleRange;
    count : bigint;
    blocked_count : bigint;
    price : string;
    tax_pct : DoubleRange;
    trading_type_pl : string;
    trading_type_eng : string;
    counting_type:string;
    comment ?: string;

}