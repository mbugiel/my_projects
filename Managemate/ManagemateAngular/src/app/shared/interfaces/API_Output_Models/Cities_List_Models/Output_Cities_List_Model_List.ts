import { Base_Server_Response } from "../../API_Shared_Response_Elements/Base_Server_Response";
import { Output_Cities_List_Model } from "./Output_Cities_List_Model";

export interface Output_Cities_List_Model_List extends Base_Server_Response{

    responseData : Array<Output_Cities_List_Model>;

}