export interface Output_Receipt_Model
{

    id : bigint;
    in_out : boolean;
    date : Date;
    element : string; 
    transport : string; 
    summary_weight : DoubleRange;
    comment : string;

}