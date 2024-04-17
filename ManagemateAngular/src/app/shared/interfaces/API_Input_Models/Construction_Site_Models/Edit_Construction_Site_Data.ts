import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Edit_Construction_Site_Data{
    session?: Session_Data;
    
    id:number;
    
    construction_site_name : string;
    address : string;
    cities_list_id_fk : number;
    postal_code : string;
    comment : string;
}