import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Add_Item_Data{
    session?: Session_Data;

    catalog_number: string;
    product_name: string;
    item_type_id_FK: number;
    weight_kg: number;
    count: number;
    blocked_count: number;
    price: number;
    tax_pct: number;
    item_trading_type_id_FK: number;
    item_counting_type_id_FK: number;
    comment: string;
}