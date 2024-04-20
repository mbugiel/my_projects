import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Edit_Authorized_Worker_Data{
    session: Session_Data;
    
    id:number;
    client_id_FK: 0;
    name: string;
    surname: string;
    phone_number: string;
    email: string;
    contact: boolean;
    collection: boolean;
    comment: string;
}