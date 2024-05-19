import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Edit_Order_Data{
    session?: Session_Data;
    
    id:number;
    
    order_name : string;
    client_id_FK : number;
    construction_site_id_FK : number;
    status : number;
    creation_date: Date;
    default_payment_method:string;
    default_payment_date_offset:number;
    default_discount:number;
    comment : string;
}