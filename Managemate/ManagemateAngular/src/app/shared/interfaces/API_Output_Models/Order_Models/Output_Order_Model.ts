export interface Output_Order_Model
{

    id : bigint;
    order_name : string;
    client_name : string;
    construction_site_name : string;
    status : number;
    creation_date : Date;
    default_payment_method:string;
    default_payment_date_offset:number;
    default_discount:number;
    comment : string;

}