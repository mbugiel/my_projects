import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Edit_Item_Counting_Type_Data{
    session ?: Session_Data;
    
    item_counting_type_id : number;
 
    counting_type : string;
}