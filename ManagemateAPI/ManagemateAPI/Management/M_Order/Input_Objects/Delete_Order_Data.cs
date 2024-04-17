using ManagemateAPI.Management.M_Session.Input_Objects;
using System.ComponentModel.DataAnnotations;

namespace ManagemateAPI.Management.M_Order.Input_Objects
{
    public class Delete_Order_Data
    {
        public Session_Data session { get; set; }

        public long id { get; set; }

    }
}
