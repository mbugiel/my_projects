import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Item_Type_Model } from "./Output_Item_Type_Model";

export interface Output_Item_Type_Model_List extends Base_Server_Response
{

    responseData : Array<Output_Item_Type_Model>;

}