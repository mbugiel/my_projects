import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import {Session_Data} from './SessionData';

export interface Session_Data_Response extends Base_Server_Response{

    responseData:Session_Data;

}