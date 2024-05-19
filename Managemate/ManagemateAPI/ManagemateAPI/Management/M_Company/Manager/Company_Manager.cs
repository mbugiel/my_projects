using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Company.Input_Objects;
using ManagemateAPI.Management.M_Company.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

/*
 * This is the Company_Manager with methods dedicated to the Company table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * get record
 */
namespace ManagemateAPI.Management.M_Company.Manager
{
    public class Company_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Company_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* 
         * Add_Company method
         * This method is used to add new records to the Company table.
         * 
         * It accepts Add_Company_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Company(Add_Company_Data input_obj)
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

                    var company_exist = _context.Company.FirstOrDefault();

                    if (company_exist == null)
                    {

                        var city = _context.Cities_List.Where(i => i.id.Equals(input_obj.city_id_FK)).FirstOrDefault();

                        if (city != null)
                        {


                            List<Decrypted_Object> decrypted_fields =
                            [

                                new Decrypted_Object { id = 0, decryptedValue = input_obj.name },
                                new Decrypted_Object { id = 1, decryptedValue = input_obj.surname },
                                new Decrypted_Object { id = 2, decryptedValue = input_obj.company_name },
                                new Decrypted_Object { id = 3, decryptedValue = input_obj.NIP },
                                new Decrypted_Object { id = 4, decryptedValue = input_obj.phone_number },
                                new Decrypted_Object { id = 5, decryptedValue = input_obj.email },
                                new Decrypted_Object { id = 6, decryptedValue = input_obj.address },
                                new Decrypted_Object { id = 7, decryptedValue = input_obj.postal_code },
                                new Decrypted_Object { id = 8, decryptedValue = input_obj.bank_name },
                                new Decrypted_Object { id = 9, decryptedValue = input_obj.bank_number },
                                new Decrypted_Object { id = 10, decryptedValue = input_obj.web_page },
                                new Decrypted_Object { id = 11, decryptedValue = input_obj.money_sign },
                                new Decrypted_Object { id = 12, decryptedValue = input_obj.money_sign_decimal }

                            ];

                            List<Encrypted_Object> encrypted_fields = await Crypto.EncryptList(input_obj.session, decrypted_fields);


                            Company new_company = new Company
                            {
                                city_id_FK = city
                            };

                            if(input_obj.company_logo != null && input_obj.file_type != null)
                            {
                                new_company.company_logo = await Crypto.EncryptByte(input_obj.session, input_obj.company_logo);
                                new_company.file_type = input_obj.file_type;
                            }

                            foreach (var item in encrypted_fields)
                            {

                                if (item == null)
                                {
                                    throw new Exception("2");//error while encrypting data 
                                }
                                else
                                {

                                    switch (item.id)
                                    {
                                        case 0:
                                            new_company.name = item.encryptedValue; break;

                                        case 1:
                                            new_company.surname = item.encryptedValue; break;

                                        case 2:
                                            new_company.company_name = item.encryptedValue; break;

                                        case 3:
                                            new_company.NIP = item.encryptedValue; break;

                                        case 4:
                                            new_company.phone_number = item.encryptedValue; break;

                                        case 5:
                                            new_company.email = item.encryptedValue; break;

                                        case 6:
                                            new_company.address = item.encryptedValue; break;

                                        case 7:
                                            new_company.postal_code = item.encryptedValue; break;

                                        case 8:
                                            new_company.bank_name = item.encryptedValue; break;

                                        case 9:
                                            new_company.bank_number = item.encryptedValue; break;

                                        case 10:
                                            new_company.web_page = item.encryptedValue; break;

                                        case 11:
                                            new_company.money_sign = item.encryptedValue; break;

                                        case 12:
                                            new_company.money_sign_decimal = item.encryptedValue; break;

                                        default:
                                            throw new Exception("2");//error while encrypting data 
                                    }
                                }
                            }
                            _context.Company.Add(new_company);
                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_ADDED+" - "+input_obj?.company_logo?.Length+" - "+new_company?.company_logo?.Length+" - ";
                        }
                        else
                        {
                            throw new Exception("19");// objects not found
                        }


                    }
                    else
                    {

                        throw new Exception("23"); //_23_COMPANY_EXISTS_ERROR

                    }


                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }

        /* 
         * Edit_Company method
         * This method is used to edit a record in the Company table.
         * 
         * It accepts Edit_Company_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Company(Edit_Company_Data input_obj)
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

                    var editing_data = _context.Company.FirstOrDefault();

                    var city = _context.Cities_List.Where(i => i.id.Equals(input_obj.city_id_FK)).FirstOrDefault();

                    if (city != null && editing_data != null)
                    {


                        List<Decrypted_Object> decrypted_fields =
                        [

                            new Decrypted_Object { id = 0, decryptedValue = input_obj.name },
                            new Decrypted_Object { id = 1, decryptedValue = input_obj.surname },
                            new Decrypted_Object { id = 2, decryptedValue = input_obj.company_name },
                            new Decrypted_Object { id = 3, decryptedValue = input_obj.NIP },
                            new Decrypted_Object { id = 4, decryptedValue = input_obj.phone_number },
                            new Decrypted_Object { id = 5, decryptedValue = input_obj.email },
                            new Decrypted_Object { id = 6, decryptedValue = input_obj.address },
                            new Decrypted_Object { id = 7, decryptedValue = input_obj.postal_code },
                            new Decrypted_Object { id = 8, decryptedValue = input_obj.bank_name },
                            new Decrypted_Object { id = 9, decryptedValue = input_obj.bank_number },
                            new Decrypted_Object { id = 10, decryptedValue = input_obj.web_page },
                            new Decrypted_Object { id = 11, decryptedValue = input_obj.money_sign }

                        ];

                        List<Encrypted_Object> encrypted_fields = await Crypto.EncryptList(input_obj.session, decrypted_fields);


                        editing_data.city_id_FK = city;
                        
                        if(input_obj.company_logo != null && input_obj.file_type != null)
                        {
                            if(input_obj.company_logo.Length > 1572864)
                            {
                                throw new Exception("29"); //File is to large to send it later to user
                            }

                            editing_data.company_logo = await Crypto.EncryptByte(input_obj.session, input_obj.company_logo);
                            editing_data.file_type = input_obj.file_type;
                        }
                        else
                        {

                            editing_data.company_logo = null;
                            editing_data.file_type = null;

                        }



                        foreach (var item in encrypted_fields)
                        {
                            if (item == null)
                            {
                                throw new Exception("2");//error while encrypting data 
                            }
                            else
                            {

                                switch (item.id)
                                {
                                    case 0:
                                        editing_data.name = item.encryptedValue; break;

                                    case 1:
                                        editing_data.surname = item.encryptedValue; break;

                                    case 2:
                                        editing_data.company_name = item.encryptedValue; break;

                                    case 3:
                                        editing_data.NIP = item.encryptedValue; break;

                                    case 4:
                                        editing_data.phone_number = item.encryptedValue; break;

                                    case 5:
                                        editing_data.email = item.encryptedValue; break;

                                    case 6:
                                        editing_data.address = item.encryptedValue; break;

                                    case 7:
                                        editing_data.postal_code = item.encryptedValue; break;

                                    case 8:
                                        editing_data.bank_name = item.encryptedValue; break;

                                    case 9:
                                        editing_data.bank_number = item.encryptedValue; break;

                                    case 10:
                                        editing_data.web_page = item.encryptedValue; break;

                                    case 11:
                                        editing_data.money_sign = item.encryptedValue; break;

                                    default:
                                        throw new Exception("2");//error while encrypting data 
                                }
                            }
                        }

                        _context.SaveChanges();

                        return Info.SUCCESSFULLY_CHANGED;
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
         * Get_Company method
         * This method gets a record from the Company table by its ID and returns it.
         * 
         * It accepts Get_Company_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Company_Model> Get_Company(Get_Company_Data input_obj)
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

                    var company = _context.Company.Include(c => c.city_id_FK).FirstOrDefault();

                    if (company == null)
                    {
                        throw new Exception("19");// company data not found
                    }
                    else
                    {

                        Company_Model company_model = new Company_Model();

                        if(company.company_logo != null && company.file_type != null)
                        {
                            if (company.company_logo.Length > 1572864)
                            {
                                company_model.company_logo = null; //File is to large to send it later to user
                                company_model.file_type = null; 
                            }
                            else
                            {
                                company_model.company_logo = await Crypto.DecryptByte(input_obj.session, company.company_logo);
                                company_model.file_type = company.file_type;
                            }


                        }

                        List<Encrypted_Object> encrypted_fields =
                        [

                            new Encrypted_Object { id = 0, encryptedValue = company.name },
                            new Encrypted_Object { id = 1, encryptedValue = company.surname },
                            new Encrypted_Object { id = 2, encryptedValue = company.company_name },
                            new Encrypted_Object { id = 3, encryptedValue = company.NIP },
                            new Encrypted_Object { id = 4, encryptedValue = company.phone_number },
                            new Encrypted_Object { id = 5, encryptedValue = company.email },
                            new Encrypted_Object { id = 6, encryptedValue = company.address },
                            new Encrypted_Object { id = 7, encryptedValue = company.postal_code },
                            new Encrypted_Object { id = 8, encryptedValue = company.bank_name },
                            new Encrypted_Object { id = 9, encryptedValue = company.bank_number },
                            new Encrypted_Object { id = 10, encryptedValue = company.web_page },
                            new Encrypted_Object { id = 11, encryptedValue = company.money_sign },
                            new Encrypted_Object { id = 12, encryptedValue = company.city_id_FK.city },
                            new Encrypted_Object { id = 13, encryptedValue = company.money_sign_decimal }

                        ];

                        List<Decrypted_Object> decrypted_fields = await Crypto.DecryptList(input_obj.session, encrypted_fields);


                        foreach (var item in decrypted_fields)
                        {
                            if (item == null)
                            {
                                throw new Exception("3");//error while decrypting data 
                            }
                            else
                            {

                                switch (item.id)
                                {
                                    case 0:
                                        company_model.name = item.decryptedValue; break;

                                    case 1:
                                        company_model.surname = item.decryptedValue; break;

                                    case 2:
                                        company_model.company_name = item.decryptedValue; break;

                                    case 3:
                                        company_model.NIP = item.decryptedValue; break;

                                    case 4:
                                        company_model.phone_number = item.decryptedValue; break;

                                    case 5:
                                        company_model.email = item.decryptedValue; break;

                                    case 6:
                                        company_model.address = item.decryptedValue; break;

                                    case 7:
                                        company_model.postal_code = item.decryptedValue; break;

                                    case 8:
                                        company_model.bank_name = item.decryptedValue; break;

                                    case 9:
                                        company_model.bank_number = item.decryptedValue; break;

                                    case 10:
                                        company_model.web_page = item.decryptedValue; break;

                                    case 11:
                                        company_model.money_sign = item.decryptedValue; break;

                                    case 12:
                                        company_model.city_name = item.decryptedValue; break;

                                    case 13:
                                        company_model.money_sign_decimal = item.decryptedValue; break;

                                    default:
                                        throw new Exception("2");//error while encrypting data 
                                }
                            }
                        }

                        return company_model;
                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }
        }


        




        // FOR DEVELOPMENT PURPOSES ONLY
        public async Task<string> DEV_Delete_Company(DEV_Delete_Company_Data obj)
        {
            if(obj == null)
            {
                throw new Exception("14");
            }
            else
            {
                if(await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    //Checking if ID given in the input object corresponds to ID of any record in the Company table.
                    var id_exits = _context.Company.Where(i => i.id.Equals(obj.id)).FirstOrDefault();
                    if (id_exits == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }
                    else
                    {
                        //Deleting the record
                        _context.Company.Remove(id_exits);
                        //Saving changes
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
    }
}
