import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Edit_Receipt_Data{
    session?: Session_Data;
    
    receipt_id:number;
    
    in_out : boolean;
    order_id_FK : number;
    element : string;
    date : Date;
    transport : string;
    comment : string;
}