import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Delete_Receipt_Data{
    session: Session_Data;

    receipt_id:number;
}