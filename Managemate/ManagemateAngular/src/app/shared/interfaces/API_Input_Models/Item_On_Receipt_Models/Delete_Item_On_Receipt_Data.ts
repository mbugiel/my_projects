import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Delete_Item_On_Receipt_Data{
    session: Session_Data;

    item_on_receipt_id:number;
}