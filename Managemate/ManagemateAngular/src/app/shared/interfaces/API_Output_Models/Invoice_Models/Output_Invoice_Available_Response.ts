import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Invoice_Available } from "./Output_Invoice_Available";

export interface Output_Invoice_Available_Response extends Base_Server_Response
{

    responseData : Array<Output_Invoice_Available>;

}