import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Edit_Item_On_Receipt_Data{
    session: Session_Data;
    
    id:number;
    
    receipt_id_FK : number;
    item_id_FK : number;
    count : number;
    summary_weight : number;
    annotation : string;
}