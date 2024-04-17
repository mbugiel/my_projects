using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Order.Input_Objects
{
    public class Get_Order_By_Id_Data
    {
        public Session_Data session { get; set; }

        public long orderId { get; set; }


    }
}
