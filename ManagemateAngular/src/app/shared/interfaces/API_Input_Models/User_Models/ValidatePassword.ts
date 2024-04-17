import { Session_Data } from "../../API_Other_Models/Session_Models/SessionData";

export interface ValidatePassword{
    sessionData?: Session_Data;
    password: string;
}