import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Get_By_ID_Invoice_Data{
    session: Session_Data;
    
    id_to_get:number;
}