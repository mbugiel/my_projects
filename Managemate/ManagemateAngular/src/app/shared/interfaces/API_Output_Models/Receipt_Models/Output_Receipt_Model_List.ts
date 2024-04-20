import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Receipt_Model } from "./Output_Receipt_Model";

export interface Output_Receipt_Model_List extends Base_Server_Response
{

    responseDate : Array<Output_Receipt_Model>;

}