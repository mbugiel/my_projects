using ManagemateAPI.Database.Context;
using ManagemateAPI.Encryption;
using ManagemateAPI.Database.Tables;
using Microsoft.EntityFrameworkCore;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Management.M_Order.Input_Objects;
using ManagemateAPI.Management.M_Order.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Client.Table_Model;
using ManagemateAPI.Management.M_Construction_Site.Table_Model;

namespace ManagemateAPI.Management.M_Order.Manager
{
    public class Order_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Order_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<string> AddOrder(Add_Order_Data input)
        {

            if (input == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input.session))
                {
                    _context = new DB_Context(input.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var client = _context.Client.Where(c => c.id.Equals(input.client_id_FK)).FirstOrDefault();
                    var construction_site = _context.Construction_Site.Where(con => con.id.Equals(input.construction_site_id_FK)).FirstOrDefault();

                    if (client != null && construction_site != null)
                    {

                        var existing_order = _context.Order.Where(o => o.order_name.Equals(Crypto.Encrypt(input.session, input.order_name)) || o.client_id_FK.Equals(client) && o.construction_site_id_FK.Equals(construction_site)).FirstOrDefault();

                        if (existing_order != null)
                        {
                            throw new Exception("18");//Order already exists
                        }
                        else
                        {

                            Order newOrder = new Order
                            {
                                order_name = await Crypto.Encrypt(input.session, input.order_name),
                                client_id_FK = client,
                                construction_site_id_FK = construction_site,
                                status = input.status,
                                creation_date = DateTime.UtcNow, //.AddHours(1)
                                comment = await Crypto.Encrypt(input.session, input.comment)
                            };

                            _context.Order.Add(newOrder);
                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_ADDED;


                        }

                    }
                    else
                    {
                        throw new Exception("19");// objects not found
                    }



                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }


        public async Task<string> EditOrder(Edit_Order_Data input)
        {

            if (input == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input.session))
                {
                    _context = new DB_Context(input.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var editing_order = _context.Order.Where(o => o.id.Equals(input.id)).FirstOrDefault();
                    var client = _context.Client.Where(c => c.id.Equals(input.client_id_FK)).FirstOrDefault();
                    var construction_site = _context.Construction_Site.Where(con => con.id.Equals(input.construction_site_id_FK)).FirstOrDefault();

                    if (editing_order == null || client == null || construction_site == null)
                    {
                        throw new Exception("19");//Order not found
                    }
                    else
                    {
                        editing_order.order_name = await Crypto.Encrypt(input.session, input.order_name);
                        editing_order.client_id_FK = client;
                        editing_order.construction_site_id_FK = construction_site;
                        editing_order.status = input.status;
                        editing_order.creation_date = input.creation_date;
                        editing_order.comment = await Crypto.Encrypt(input.session, input.comment);

                        _context.SaveChanges();

                        return Info.SUCCESSFULLY_CHANGED;
                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }


        public async Task<string> DeleteOrder(Delete_Order_Data input)
        {

            if (input == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input.session))
                {
                    _context = new DB_Context(input.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var existing_order = _context.Order.Where(o => o.id.Equals(input.id)).FirstOrDefault();

                    if (existing_order == null)
                    {
                        throw new Exception("19");//Order not found
                    }
                    else
                    {
                        _context.Order.Remove(existing_order);
                        _context.SaveChanges();

                        return Info.SUCCESSFULLY_DELETED;
                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }


        public async Task<List<Order_Model_List>> GetOrders(Get_Orders_Data input)
        {

            if (input == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input.session))
                {
                    _context = new DB_Context(input.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    List<Order> Orders = _context.Order.Include(o => o.client_id_FK).Include(o => o.construction_site_id_FK).ToList();

                    if (Orders == null)
                    {
                        throw new Exception("19");//Order not found
                    }
                    else
                    {

                        List<Encrypted_Object> order_names = new List<Encrypted_Object>();
                        List<Encrypted_Object> order_comments = new List<Encrypted_Object>();
                        List<Encrypted_Object> order_client_names = new List<Encrypted_Object>();
                        List<Encrypted_Object> order_construction_sites_names = new List<Encrypted_Object>();


                        List<Order_Model_List> Orders_decrypted = new List<Order_Model_List>();

                        foreach (var order in Orders)
                        {
                            Orders_decrypted.Add(new Order_Model_List
                            {
                                id = order.id,
                                //client_name = item.client_id_FK,
                                //construction_site_name = item.construction_site_id_FK,
                                status = order.status,
                                creation_date = order.creation_date

                            });

                            order_names.Add(new Encrypted_Object { id = order.id, encryptedValue = order.order_name });
                            order_comments.Add(new Encrypted_Object { id = order.id, encryptedValue = order.comment });

                            if (order.client_id_FK.company_name != null)
                            {
                                order_client_names.Add(new Encrypted_Object { id = order.id, encryptedValue = order.client_id_FK.company_name });
                            }
                            else
                            {
                                order_client_names.Add(new Encrypted_Object { id = order.id, encryptedValue = order.client_id_FK.name });
                            }

                            order_construction_sites_names.Add(new Encrypted_Object { id = order.id, encryptedValue = order.construction_site_id_FK.construction_site_name });


                        }

                        List<Decrypted_Object> order_names_decrypted = await Crypto.DecryptList(input.session, order_names);
                        List<Decrypted_Object> order_comments_decrypted = await Crypto.DecryptList(input.session, order_comments);
                        List<Decrypted_Object> order_client_names_decrypted = await Crypto.DecryptList(input.session, order_client_names);
                        List<Decrypted_Object> order_construction_sites_names_decrypted = await Crypto.DecryptList(input.session, order_construction_sites_names);

                        foreach (var order in Orders_decrypted)
                        {
                            var name = order_names_decrypted.Where(o => o.id.Equals(order.id)).FirstOrDefault();

                            if (name == null)
                            {
                                throw new Exception("3");//error while decrypting data 
                            }
                            else
                            {
                                order.order_name = name.decryptedValue;
                            }


                            var comment = order_comments_decrypted.Where(o => o.id.Equals(order.id)).FirstOrDefault();

                            if (comment == null)
                            {
                                throw new Exception("3");//error while decrypting data 
                            }
                            else
                            {
                                order.comment = comment.decryptedValue;
                            }


                            var client_name = order_client_names_decrypted.Where(o => o.id.Equals(order.id)).FirstOrDefault();

                            if (client_name == null)
                            {
                                throw new Exception("3");//error while decrypting data 
                            }
                            else
                            {
                                order.client_name = client_name.decryptedValue;
                            }



                            var construction_site_name = order_construction_sites_names_decrypted.Where(o => o.id.Equals(order.id)).FirstOrDefault();

                            if (construction_site_name == null)
                            {
                                throw new Exception("3");//error while decrypting data 
                            }
                            else
                            {
                                order.construction_site_name = construction_site_name.decryptedValue;
                            }

                        }

                        return Orders_decrypted;



                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }



        /*
        public async Task<List<Order_Model_List>> GetOrders(Get_Orders_Data order)
        {

            if (order == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(order.session))
                {
                    _context = new DB_Context(order.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    List<Order> Orders = _context.Order.Include(o => o.client_id_FK).Include(o => o.construction_site_id_FK).ToList();

                    if (Orders == null)
                    {
                        throw new Exception("19");//Order not found
                    }
                    else
                    {

                        int page_length = order.pageSize;
                        int start_position = order.pageid * page_length;


                        if (Orders.Count() > start_position)
                        {

                            if (Orders.Count() >= start_position + page_length)
                            {
                                Orders = Orders.Slice(start_position, page_length);

                            }
                            else
                            {
                                int valid_length = Orders.Count() - start_position;

                                Orders = Orders.Slice(start_position, valid_length);
                            }

                            List<Encrypted_Object> order_names = new List<Encrypted_Object>();
                            List<Encrypted_Object> order_comments = new List<Encrypted_Object>();
                            List<Encrypted_Object> order_client_names = new List<Encrypted_Object>();
                            List<Encrypted_Object> order_construction_sites_names = new List<Encrypted_Object>();


                            List<Order_Model_List> Orders_decrypted = new List<Order_Model_List>();

                            foreach (var item in Orders)
                            {
                                Orders_decrypted.Add(new Order_Model_List
                                {
                                    id = item.id,
                                    //client_name = item.client_id_FK,
                                    //construction_site_name = item.construction_site_id_FK,
                                    status = item.status,
                                    creation_date = item.creation_date

                                });

                                order_names.Add(new Encrypted_Object { id = item.id, encryptedValue = item.order_name });
                                order_comments.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment });

                                if (item.client_id_FK.company_name != null)
                                {
                                    order_client_names.Add(new Encrypted_Object { id = item.id, encryptedValue = item.client_id_FK.company_name });
                                }
                                else
                                {
                                    order_client_names.Add(new Encrypted_Object { id = item.id, encryptedValue = item.client_id_FK.name });
                                }

                                order_construction_sites_names.Add(new Encrypted_Object { id = item.id, encryptedValue = item.construction_site_id_FK.construction_site_name });


                            }

                            List<Decrypted_Object> order_names_decrypted = await Crypto.DecryptList(order.session, order_names);
                            List<Decrypted_Object> order_comments_decrypted = await Crypto.DecryptList(order.session, order_comments);
                            List<Decrypted_Object> order_client_names_decrypted = await Crypto.DecryptList(order.session, order_client_names);
                            List<Decrypted_Object> order_construction_sites_names_decrypted = await Crypto.DecryptList(order.session, order_construction_sites_names);

                            foreach (var item in Orders_decrypted)
                            {
                                var name = order_names_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                if (name == null)
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }
                                else
                                {
                                    item.order_name = name.decryptedValue;
                                }


                                var comment = order_comments_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                if (comment == null)
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }
                                else
                                {
                                    item.comment = comment.decryptedValue;
                                }


                                var client_name = order_client_names_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                if (client_name == null)
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }
                                else
                                {
                                    item.client_name = client_name.decryptedValue;
                                }



                                var construction_site_name = order_construction_sites_names_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                if (construction_site_name == null)
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }
                                else
                                {
                                    item.construction_site_name = construction_site_name.decryptedValue;
                                }

                            }

                            return Orders_decrypted;



                        }
                        else
                        {
                            throw new Exception("19");//no more orders
                        }



                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }

        */


        public async Task<Order_Model> GetOrderById(Get_Order_By_Id_Data input)
        {

            if (input == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input.session))
                {
                    _context = new DB_Context(input.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var selected_order = _context.Order.Include(o => o.construction_site_id_FK).ThenInclude(c => c.cities_list_id_FK).Include(o => o.client_id_FK).ThenInclude(c => c.city_id_FK).Where(o => o.id.Equals(input.orderId)).FirstOrDefault();

                    if (selected_order == null)
                    {
                        throw new Exception("19");//Order not found
                    }
                    else
                    {

                        List<Encrypted_Object> encrypted_fields =
                        [
                            //encrypted fields in Order table
                            new Encrypted_Object { id = 0, encryptedValue = selected_order.order_name },
                            new Encrypted_Object { id = 1, encryptedValue = selected_order.comment },
                            //encrypted fields in Client table
                            new Encrypted_Object { id = 2, encryptedValue = selected_order.client_id_FK.surname },
                            new Encrypted_Object { id = 3, encryptedValue = selected_order.client_id_FK.name },
                            new Encrypted_Object { id = 4, encryptedValue = selected_order.client_id_FK.company_name },
                            new Encrypted_Object { id = 5, encryptedValue = selected_order.client_id_FK.NIP },
                            new Encrypted_Object { id = 6, encryptedValue = selected_order.client_id_FK.phone_number },
                            new Encrypted_Object { id = 7, encryptedValue = selected_order.client_id_FK.email },
                            new Encrypted_Object { id = 8, encryptedValue = selected_order.client_id_FK.address },
                            new Encrypted_Object { id = 9, encryptedValue = selected_order.client_id_FK.city_id_FK.city },
                            new Encrypted_Object { id = 10, encryptedValue = selected_order.client_id_FK.postal_code },
                            new Encrypted_Object { id = 11, encryptedValue = selected_order.client_id_FK.comment },
                            //encrypted fields in Construction_Site table
                            new Encrypted_Object { id = 12, encryptedValue = selected_order.construction_site_id_FK.construction_site_name },
                            new Encrypted_Object { id = 13, encryptedValue = selected_order.construction_site_id_FK.address },
                            new Encrypted_Object { id = 14, encryptedValue = selected_order.construction_site_id_FK.cities_list_id_FK.city },
                            new Encrypted_Object { id = 15, encryptedValue = selected_order.construction_site_id_FK.postal_code },
                            new Encrypted_Object { id = 16, encryptedValue = selected_order.construction_site_id_FK.comment },
                        ];

                        List<Decrypted_Object> decrypted_firlds = await Crypto.DecryptList(input.session, encrypted_fields);

                        Order_Model order_model = new Order_Model
                        {
                            id = selected_order.id,
                            status = selected_order.status,
                            creation_date = selected_order.creation_date,
                            client_id_FK = new Client_Model { id = selected_order.client_id_FK.id },
                            construction_site_id_FK = new Construction_Site_Model { id = selected_order.construction_site_id_FK.id }
                        };

                        foreach (var order in decrypted_firlds)
                        {
                            if (order == null)
                            {
                                throw new Exception("3");//error while decrypting data 
                            }
                            else
                            {

                                switch (order.id)
                                {
                                    case 0:
                                        order_model.order_name = order.decryptedValue; break;

                                    case 1:
                                        order_model.comment = order.decryptedValue; break;

                                    //client:

                                    case 2:
                                        order_model.client_id_FK.surname = order.decryptedValue; break;

                                    case 3:
                                        order_model.client_id_FK.name = order.decryptedValue; break;

                                    case 4:
                                        order_model.client_id_FK.company_name = order.decryptedValue; break;

                                    case 5:
                                        order_model.client_id_FK.NIP = order.decryptedValue; break;

                                    case 6:
                                        order_model.client_id_FK.phone_number = order.decryptedValue; break;

                                    case 7:
                                        order_model.client_id_FK.email = order.decryptedValue; break;

                                    case 8:
                                        order_model.client_id_FK.address = order.decryptedValue; break;

                                    case 9:
                                        order_model.client_id_FK.city_id_FK = order.decryptedValue; break;

                                    case 10:
                                        order_model.client_id_FK.postal_code = order.decryptedValue; break;

                                    case 11:
                                        order_model.client_id_FK.comment = order.decryptedValue; break;

                                    //construction_site:
                                    case 12:
                                        order_model.construction_site_id_FK.construction_site_name = order.decryptedValue; break;

                                    case 13:
                                        order_model.construction_site_id_FK.address = order.decryptedValue; break;

                                    case 14:
                                        order_model.construction_site_id_FK.cities_list_id_FK = order.decryptedValue; break;

                                    case 15:
                                        order_model.construction_site_id_FK.postal_code = order.decryptedValue; break;

                                    case 16:
                                        order_model.construction_site_id_FK.comment = order.decryptedValue; break;
                                    default:
                                        throw new Exception("3");//error while decrypting data 
                                }



                            }

                        }

                        return order_model;

                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }

    }

}
