import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Edit_Company_Data{
    session: Session_Data;
    
    id:number;
    
    name: string;
    surname: string;
    company_name: string;
    nip: string;
    phone_number: string;
    email: string;
    address: string;
    city_id_FK: number;
    postal_code: string;
    bank_name: string;
    bank_number: string;
    web_page: string;
    money_sign: string
}