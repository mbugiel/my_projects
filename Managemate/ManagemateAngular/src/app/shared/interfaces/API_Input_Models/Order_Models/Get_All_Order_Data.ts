import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Get_All_Order_Data{
    session?: Session_Data;
    all_or_active_only:boolean;
}