import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Add_Item_On_Receipt_Data{
    session?: Session_Data;

    receipt_id_FK : number;
    item_id_FK : number;
    count : number;
    annotation : string;
}