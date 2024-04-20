import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Company_Model } from "./Output_Company_Model";

export interface Output_Company_Model_List extends Base_Server_Response
{

    responseDate : Array<Output_Company_Model>;

}