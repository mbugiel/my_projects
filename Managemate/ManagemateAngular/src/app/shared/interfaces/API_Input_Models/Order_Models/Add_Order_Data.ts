import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Add_Order_Data{
    session?: Session_Data;

    creation_date ?: Date;
    order_name : string;
    client_id_FK : number;
    construction_site_id_FK : number;
    status : number;
    default_payment_method:string;
    default_payment_date_offset:number;
    default_discount:number;
    comment : string;
}