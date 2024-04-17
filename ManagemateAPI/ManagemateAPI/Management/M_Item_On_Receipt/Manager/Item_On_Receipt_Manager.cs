using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Item_On_Receipt.Input_Objects;
using ManagemateAPI.Management.M_Item_On_Receipt.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;
using Microsoft.EntityFrameworkCore;

namespace ManagemateAPI.Management.M_Item_On_Receipt.Manager
{
    public class Item_On_Receipt_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Item_On_Receipt_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<string> AddItemOnReceipt(Add_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input_obj.session))
                {
                    _context = new DB_Context(input_obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var receipt = _context.Receipt.Where(r => r.id.Equals(input_obj.receipt_id_FK)).FirstOrDefault();
                    var item = _context.Item.Where(i => i.id.Equals(input_obj.item_id_FK)).FirstOrDefault();

                    if (receipt != null && item != null)
                    {

                        var existing_item_on_receipt = _context.Item_On_Receipt.Where(i => i.receipt_id_FK.Equals(receipt) && i.item_id_FK.Equals(item)).FirstOrDefault();

                        if (existing_item_on_receipt != null)
                        {
                            throw new Exception("18");//item already exists on receipt
                        }
                        else
                        {

                            Item_On_Receipt new_item_on_receipt = new Item_On_Receipt
                            {
                                receipt_id_FK = receipt,
                                item_id_FK = item,
                                count = input_obj.count,
                                summary_weight = input_obj.summary_weight,
                                annotation = await Crypto.Encrypt(input_obj.session, input_obj.annotation)
                            };

                            _context.Item_On_Receipt.Add(new_item_on_receipt);
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


        public async Task<string> EditItemOnReceipt(Edit_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input_obj.session))
                {
                    _context = new DB_Context(input_obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var editing_item_on_receipt = _context.Item_On_Receipt.Where(i => i.id.Equals(input_obj.id)).FirstOrDefault();

                    var item = _context.Item.Where(i => i.id.Equals(input_obj.item_id_FK)).FirstOrDefault();

                    if (editing_item_on_receipt == null || item == null)
                    {
                        throw new Exception("19");//Order not found
                    }
                    else
                    {

                        var existing_item_on_receipt = _context.Item_On_Receipt.Where(i => i.receipt_id_FK.Equals(editing_item_on_receipt.receipt_id_FK) && i.item_id_FK.Equals(item)).FirstOrDefault();

                        if (existing_item_on_receipt != null)
                        {
                            throw new Exception("18");//item already exists on receipt
                        }
                        else
                        {

                            editing_item_on_receipt.item_id_FK = item;
                            editing_item_on_receipt.count = input_obj.count;
                            editing_item_on_receipt.summary_weight = input_obj.summary_weight;
                            editing_item_on_receipt.annotation = await Crypto.Encrypt(input_obj.session, input_obj.annotation);

                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_CHANGED;
                        }


                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }


        public async Task<string> DeleteItemOnReceipt(Delete_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input_obj.session))
                {
                    _context = new DB_Context(input_obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var existing_item_on_receipt = _context.Item_On_Receipt.Where(i => i.id.Equals(input_obj.item_on_receipt_id)).FirstOrDefault();

                    if (existing_item_on_receipt == null)
                    {
                        throw new Exception("19");//Order not found
                    }
                    else
                    {
                        _context.Item_On_Receipt.Remove(existing_item_on_receipt);
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


        public async Task<List<Item_On_Receipt_Model>> GetItemsOnReceipt(Get_Items_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input_obj.session))
                {
                    _context = new DB_Context(input_obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var current_receipt = _context.Receipt.Where(r => r.id.Equals(input_obj.receipt_id)).FirstOrDefault();

                    if (current_receipt == null)
                    {
                        throw new Exception("19");// receipt not found
                    }
                    else
                    {

                        List<Item_On_Receipt> items_on_receipt = _context.Item_On_Receipt.Where(i => i.receipt_id_FK.Equals(current_receipt)).Include(i => i.item_id_FK).ThenInclude(it => it.item_counting_type_id_FK).ToList();

                        if (items_on_receipt == null)
                        {
                            throw new Exception("13");// no items on receipt found
                        }
                        else
                        {
                            List<Item_On_Receipt_Model> items_on_receipt_model = new List<Item_On_Receipt_Model>();

                            List<Encrypted_Object> items_on_receipt_annotations = new List<Encrypted_Object>();
                            List<Encrypted_Object> items_on_receipt_names = new List<Encrypted_Object>();

                            foreach (var item in items_on_receipt)
                            {
                                items_on_receipt_model.Add(new Item_On_Receipt_Model
                                {
                                    id = item.id,
                                    catalog_number = item.item_id_FK.catalog_number,
                                    counting_type = item.item_id_FK.item_counting_type_id_FK.counting_type,
                                    count = item.count,
                                    weight = item.item_id_FK.weight_kg,
                                    summary_weight = item.summary_weight

                                });

                                items_on_receipt_annotations.Add(new Encrypted_Object { id = item.id, encryptedValue = item.annotation });
                                items_on_receipt_names.Add(new Encrypted_Object { id = item.id, encryptedValue = item.item_id_FK.product_name });
                            }


                            List<Decrypted_Object> decrypted_items_on_receipt_annotations = await Crypto.DecryptList(input_obj.session, items_on_receipt_annotations);
                            List<Decrypted_Object> decrypted_items_on_receipt_names = await Crypto.DecryptList(input_obj.session, items_on_receipt_names);

                            foreach (var item_decrypted in items_on_receipt_model)
                            {

                                var annotation = decrypted_items_on_receipt_annotations.Where(d => d.id.Equals(item_decrypted.id)).FirstOrDefault();

                                if (annotation != null)
                                {
                                    item_decrypted.annotation = annotation.decryptedValue;
                                }
                                else
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }

                                var product_name = decrypted_items_on_receipt_names.Where(d => d.id.Equals(item_decrypted.id)).FirstOrDefault();

                                if (product_name != null)
                                {
                                    item_decrypted.product_name = product_name.decryptedValue;
                                }
                                else
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }

                            }

                            return items_on_receipt_model;


                        }

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
