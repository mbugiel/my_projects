import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Edit_Item_Type_Data{
    session?: Session_Data;
    
    item_type_id : number;

    item_type : string;

    rate:number;
}