import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Get_All_Receipt_Data{
    session?: Session_Data;

    order_id: number;
    in_out: boolean;
}