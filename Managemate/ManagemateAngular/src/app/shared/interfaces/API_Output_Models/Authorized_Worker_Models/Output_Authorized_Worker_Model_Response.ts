import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Authorized_Worker_Model } from "./Output_Authorized_Worker_Model";

export interface Output_Authorized_Worker_Model_Response extends Base_Server_Response
{

    responseData : Output_Authorized_Worker_Model;

}