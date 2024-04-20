import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Construction_Site_Model } from "./Output_Construction_Site_Model";

export interface Output_Construction_Site_Model_List extends Base_Server_Response
{

    responseData : Array<Output_Construction_Site_Model>;

}