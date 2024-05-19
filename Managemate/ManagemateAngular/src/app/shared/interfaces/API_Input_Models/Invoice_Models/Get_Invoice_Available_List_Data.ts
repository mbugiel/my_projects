import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Get_Invoice_Available_List_Data{
    session?: Session_Data;
    
    order_id:number;
}