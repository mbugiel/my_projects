export interface Output_Item_Model
{

    id : number;
    catalog_number : string;
    product_name : string;
    item_type : string;
    weight_kg : number;
    count : number;
    blocked_count : number;
    price : string;
    tax_pct : number;
    trading_type_pl : string;
    trading_type_eng : string;
    counting_type:string;
    comment ?: string;

}