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


/*
 * This is the Order_Manager with methods dedicated to the Order table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get record by id,
 * get all the records.
 */
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

        /* 
         * Add_Order method
         * This method is used to add new records to the Order table.
         * 
         * It accepts Add_Order_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Order(Add_Order_Data input)
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
                                creation_date = input.creation_date,
                                default_payment_method = await Crypto.Encrypt(input.session, input.default_payment_method),
                                default_payment_date_offset = input.default_payment_date_offset,
                                default_discount = input.default_discount,
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

        /* 
         * Edit_Order method
         * This method is used to edit a record in the Order table.
         * 
         * It accepts Edit_Order_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Order(Edit_Order_Data input)
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
                        editing_order.default_payment_method = await Crypto.Encrypt(input.session, input.default_payment_method);
                        editing_order.default_payment_date_offset = input.default_payment_date_offset;
                        editing_order.default_discount = input.default_discount;
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

        /*
         * Delete_Order method
         * This method is used to a record from the Order table.
         *  
         * It accepts Delete_Order_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Order(Delete_Order_Data input)
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

        /*
         * Get_Order_By_ID method
         * This method gets a record from the Order table by its ID and returns it.
         * 
         * It accepts Get_Order_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Order_Model> Get_Order_By_ID(Get_Order_By_Id_Data input)
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
                            new Encrypted_Object { id = 17, encryptedValue = selected_order.default_payment_method },
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
                            default_payment_date_offset = selected_order.default_payment_date_offset,
                            default_discount = selected_order.default_discount,
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

                                    case 17:
                                        order_model.default_payment_method = order.decryptedValue; break;
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

        /*
         * Get_All_Order method
         * This method gets all of the records in the Order table and returns them in a list.
         * 
         * It accepts Get_All_Order_Data object as input.
         */
        public async Task<List<Order_Model_List>> Get_All_Order(Get_All_Order_Data input)
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

                    List<Order> Orders;

                    if (input.all_or_active_only)
                    {
                        Orders = _context.Order.Where(o => o.status.Equals(1)).Include(o => o.client_id_FK).Include(o => o.construction_site_id_FK).ToList(); // active only
                    }
                    else
                    {

                        Orders = _context.Order.Include(o => o.client_id_FK).Include(o => o.construction_site_id_FK).ToList(); // all of orders

                    }


                    if (Orders == null)
                    {
                        throw new Exception("19");//Order not found
                    }
                    else
                    {

                        List<Encrypted_Object> order_names = new List<Encrypted_Object>();
                        List<Encrypted_Object> order_default_payment_methods = new List<Encrypted_Object>();
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
                                creation_date = order.creation_date,
                                default_payment_date_offset = order.default_payment_date_offset,
                                default_discount = order.default_discount

                            });

                            order_names.Add(new Encrypted_Object { id = order.id, encryptedValue = order.order_name });
                            order_default_payment_methods.Add(new Encrypted_Object { id = order.id, encryptedValue = order.default_payment_method });
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
                        List<Decrypted_Object> order_default_payment_methods_decrypted = await Crypto.DecryptList(input.session, order_default_payment_methods);
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


                            var default_payment_method = order_default_payment_methods_decrypted.Where(o => o.id.Equals(order.id)).FirstOrDefault();

                            if (default_payment_method == null)
                            {
                                throw new Exception("3");//error while decrypting data 
                            }
                            else
                            {
                                order.default_payment_method = default_payment_method.decryptedValue;
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
    }
}
