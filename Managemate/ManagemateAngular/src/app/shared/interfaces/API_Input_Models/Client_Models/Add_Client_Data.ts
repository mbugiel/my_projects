import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Add_Client_Data{
    session?: Session_Data;

    surname:string;
    name:string;
    company_name:string;
    nip: string;
    phone_number: string;
    email: string;
    address: string;
    city_id_fk:number;
    postal_code: string;
    comment: string;
}