import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Add_Order_Data{
    session?: Session_Data;

    order_name : string;
    client_id_FK : number;
    construction_site_id_FK : number;
    status : number;
    comment : string;
}