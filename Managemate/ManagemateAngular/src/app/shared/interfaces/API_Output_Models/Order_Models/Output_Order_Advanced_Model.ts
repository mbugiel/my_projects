import { Output_Client_Model } from "../Client_Models/Output_Client_Model"
import { Output_Construction_Site_Model } from "../Construction_Site_Models/Output_Construction_Site_Model"

export interface Output_Order_Advanced_Model
{

    id: number;
    order_name: string;
    client_id_FK: Output_Client_Model;
    construction_site_id_FK: Output_Construction_Site_Model;
    status: number;
    creation_date: Date;
    default_payment_method:string;
    default_payment_date_offset:number;
    default_discount:number;
    comment: string;

}