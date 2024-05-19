import {Session_Data} from "../../API_Other_Models/Session_Models/SessionData";

export interface Issue_Invoice_Data{

    session?: Session_Data;
    
    order_id: number;
    invoice_type: boolean;
    language_tag: string;
    month: number;
    year: number;
    issue_date: Date;
    sale_date: Date;
    payment_date: Date;
    payment_method: string;
    discount: number;
    comment: string;

}