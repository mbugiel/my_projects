import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Invoice_Model } from "./Output_Invoice_Model";

export interface Output_Invoice_Model_List extends Base_Server_Response
{

    responseDate : Array<Output_Invoice_Model>;

}