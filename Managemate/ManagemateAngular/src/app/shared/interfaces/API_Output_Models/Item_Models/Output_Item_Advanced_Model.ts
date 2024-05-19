import { Output_Item_Counting_Type_Model } from "../Item_Counting_Type_Models/Output_Item_Counting_Type_Model";
import { Output_Item_Trading_Type_Model } from "../Item_Trading_Type_Models/Output_Item_Trading_Type_Model";

export interface Output_Item_Advanced_Model
{

    id : number;
    catalog_number : string;
    product_name : string;
    item_type : string;
    weight_kg : DoubleRange;
    count : bigint;
    blocked_count : bigint;
    price : string;
    tax_pct : DoubleRange;
    item_trading_type_id_FK : Output_Item_Trading_Type_Model;
    item_counting_type_id_FK : Output_Item_Counting_Type_Model;
    comment ?: string;

}