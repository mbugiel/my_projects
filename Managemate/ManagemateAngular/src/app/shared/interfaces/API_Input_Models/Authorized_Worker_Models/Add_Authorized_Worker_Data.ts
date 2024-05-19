import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Add_Authorized_Worker_Data{
    session?: Session_Data;
    
    client_id_FK: number;
    name: string;
    surname: string;
    phone_number: string;
    email: string;
    contact: boolean;
    collection: boolean;
    comment: string;
}