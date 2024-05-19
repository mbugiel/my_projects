import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Add_Receipt_Data{
    session ?: Session_Data;

    creation_date ?: Date;
    in_out : boolean;
    reservation:boolean;
    order_id_FK : number;
    element : string;
    transport : string;
    // comment : string;
}