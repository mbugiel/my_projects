import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Item_On_Receipt_Return_Available_Model } from "./Item_On_Receipt_Return_Available_Model";

export interface Output_Item_On_Receipt_Return_Available_Model_Response extends Base_Server_Response{

    responseData:Array<Output_Item_On_Receipt_Return_Available_Model>;

}