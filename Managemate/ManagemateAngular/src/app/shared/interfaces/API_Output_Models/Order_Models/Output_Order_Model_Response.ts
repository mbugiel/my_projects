import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Order_Model } from "./Output_Order_Model";

export interface Output_Order_Model_Response extends Base_Server_Response
{

    responseData : Array<Output_Order_Model>;

}