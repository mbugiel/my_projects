import { Output_Client_Model } from "../Client_Models/Output_Client_Model"
import { Output_Construction_Site_Model } from "../Construction_Site_Models/Output_Construction_Site_Model"

export interface Output_Order_Advanced_Model
{

    id: bigint;
    order_name: string;
    client_id_FK: Output_Client_Model;
    construction_site_id_FK: Output_Construction_Site_Model;
    status: number;
    creation_date: Date;
    comment: string;

}