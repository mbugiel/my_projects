import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Edit_Client_Data{
    session?: Session_Data;
    
    id:number;
    
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