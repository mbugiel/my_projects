import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Client_Model } from "./Output_Client_Model";

export interface Output_Client_Model_Response extends Base_Server_Response
{

    responseData : Output_Client_Model;

}