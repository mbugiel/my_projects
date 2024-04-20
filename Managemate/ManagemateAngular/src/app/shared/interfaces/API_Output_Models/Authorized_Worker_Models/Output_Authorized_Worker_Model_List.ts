import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Authorized_Worker_Model } from "./Output_Authorized_Worker_Model";

export interface Output__Model_List extends Base_Server_Response
{

    responseDate : Array<Output_Authorized_Worker_Model>;

}