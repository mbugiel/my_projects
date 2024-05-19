import { Output_Item_On_Receipt_Model } from "../Item_On_Receipt_Models/Output_Item_On_Receipt_Model";

export interface Output_Receipt_Advanced_Model
{

    id : number;
    in_out : boolean;
    order_id: number;
    date : Date;
    element : string; 
    transport : string; 
    summary_weight : DoubleRange;
    comment : string;
    items_on_receipt: Array<Output_Item_On_Receipt_Model>;

}