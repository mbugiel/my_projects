using AuthenticationAPI.Database;
using AuthenticationAPI.Encryption;
using AuthenticationAPI.Helper.Service;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationAPI.Helper
{
    public class DbHelper
    {

        private readonly DatabaseContext _context;
        private readonly Keys _keys;
        public DbHelper(DatabaseContext context) 
        {
            _context = context;
            _keys = Crypto.GetKeys();
        }

        public string UserExist(UserExistData userExistData)
        {


            if (userExistData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {

                var login = _context.Users.Where(u => u.UsernameNormalized.Equals(Crypto.Encrypt(userExistData.Username.ToUpper(), _keys.Key, _keys.Iv))).FirstOrDefault();
                var email = _context.Users.Where(u => u.Email.Equals(Crypto.Encrypt(userExistData.Email.ToUpper(), _keys.Key, _keys.Iv))).FirstOrDefault();

                if (login != null) throw new Exception("4");//_4_LOGIN_IN_USE

                if (email != null) throw new Exception("5");//_5_EMAIL_IN_USE

                return Info.FREE_ACCOUNT;

            }

        }

        public SessionData AddUser(AddUserData user)
        {


            if (user == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else 
            {

                UserExist(new UserExistData { Email = user.Email, Username = user.Username });
                ConfirmEmail(user.Email, user.EmailToken);

                Users newUser = new Users();


                RandomNumberGenerator rng = RandomNumberGenerator.Create();

                byte[] key = new byte[16];
                byte[] iv = new byte[16];

                rng.GetBytes(key);
                rng.GetBytes(iv);



                newUser.Key = Crypto.EncryptByte(key, _keys.Key, _keys.Iv);
                newUser.Iv = Crypto.EncryptByte(iv, _keys.Key, _keys.Iv);
                newUser.Username = Crypto.Encrypt(user.Username, _keys.Key, _keys.Iv);
                newUser.UsernameNormalized = Crypto.Encrypt(user.Username.ToUpper(), _keys.Key, _keys.Iv);
                newUser.Email = Crypto.Encrypt(user.Email.ToUpper(), _keys.Key, _keys.Iv);
                newUser.Password = Crypto.Encrypt(user.Password, key, iv);
                newUser.TwoStepLogin = user.TwoStepLogin;



                _context.Users.Add(newUser);
                _context.SaveChanges();

                var addedUser = _context.Users.Where(u => u.UsernameNormalized.Equals(Crypto.Encrypt(user.Username.ToUpper(), _keys.Key, _keys.Iv))).FirstOrDefault();

                if (addedUser != null)
                {
                    return new SessionData { Token = SaveSessionToken(addedUser.Id), UserId = addedUser.Id };
                }
                else
                {
                    throw new Exception("6");//_6_ADD_USER_ERROR
                }




            }



        }


        public string HasTwoStepLogin(HasTwoStepLoginData loginData)
        {

            if (loginData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {

                var user = _context.Users.Where(u => u.UsernameNormalized.Equals(Crypto.Encrypt(loginData.Username.ToUpper(), _keys.Key, _keys.Iv))).FirstOrDefault();

                if (user != null)
                {


                    if (ValidatePassword(user, loginData.Password))
                    {

                        if (user.TwoStepLogin)
                        {

                            return Crypto.Decrypt(user.Email, _keys.Key, _keys.Iv);

                        }
                        else
                        {
                            return "#";
                        }


                    }
                    else
                    {
                        throw new Exception("8");//_8_PASSWORD_ERROR
                    }


                }
                else
                {
                    throw new Exception("7");//_7_USER_NOT_FOUND
                }


            }

        }

        public SessionData Login(LoginData loginData)
        {

            if (loginData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {

                var user = _context.Users.Where(u => u.UsernameNormalized.Equals(Crypto.Encrypt(loginData.Username.ToUpper(), _keys.Key, _keys.Iv))).FirstOrDefault();

                if (user != null)
                {
                    

                    if(ValidatePassword(user, loginData.Password))
                    {

                        if (user.TwoStepLogin)
                        {

                            ConfirmEmail(Crypto.Decrypt(user.Email, _keys.Key, _keys.Iv), loginData.EmailToken);

                            return new SessionData { Token = SaveSessionToken(user.Id), UserId = user.Id };

                        }
                        else
                        {
                            return new SessionData { Token = SaveSessionToken(user.Id), UserId = user.Id };
                        }

                        
                    }
                    else
                    {
                        throw new Exception("8");//_8_PASSWORD_ERROR
                    }


                }
                else
                {
                    throw new Exception("7");//_7_USER_NOT_FOUND
                }


            }


        }

        public bool ValidatePassword(Users user, string password)
        {
            if (user != null)
            {
                byte[] userKey = Crypto.DecryptByte(user.Key, _keys.Key, _keys.Iv);
                byte[] userIv = Crypto.DecryptByte(user.Iv, _keys.Key, _keys.Iv);

                if (Crypto.Decrypt(user.Password, userKey, userIv).Equals(password))
                {

                    return true;

                }
                else
                {
                    return false;
                }


            }
            else
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
        }


        public string ValidatePassword(ValidatePasswordData passwdData)
        {

            if (passwdData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {

                var user = IsLogged(passwdData.SessionData);

                if (user == null)
                {
                    throw new Exception("7");//_7_USER_NOT_FOUND
                }
                else
                {
                    
                    if(ValidatePassword(user, passwdData.Password))
                    {

                        return Info.VALID_PASSWORD;

                    }
                    else
                    {

                        throw new Exception("21");//INVALID PASSWORD

                    }


                }




            }

        }


        public string Logout(SessionData sessionData) 
        {

            if(sessionData == null) 
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {

                var session = _context.SessionTokens.Where(s => s.UserId.Equals(sessionData.UserId) && s.SessionToken.Equals(Crypto.Encrypt(sessionData.Token, _keys.Key, _keys.Iv))).FirstOrDefault();

                if(session == null)
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
                else
                {
                    _context.SessionTokens.Remove(session);
                    _context.SaveChanges();
                    return Info.SESSION_CLOSED;
                }

            }

        }


        public string SaveSessionToken(long userId)
        {
            if(userId <= 0)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {

                string addon = userId.ToString() + "000000000000";
                addon = addon.Substring(0, 12);

                DateTime now = DateTime.UtcNow;

                string token = RandomNumberGenerator.GetHexString(256)+now.Year+RandomNumberGenerator.GetHexString(128)+addon+RandomNumberGenerator.GetHexString(128);

                SessionTokens sessionToken = new SessionTokens
                {
                    UserId = userId,
                    SessionToken = Crypto.Encrypt(token, _keys.Key, _keys.Iv),
                    SessionLifetime = DateTime.UtcNow.AddDays(7),
                };

                _context.SessionTokens.Add(sessionToken);
                _context.SaveChanges();

                return token;

            }


        }


        public string SetPassword(SetPasswdData passwdData)
        {

            if (passwdData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {

                var user = _context.Users.Where(u => u.Email.Equals(Crypto.Encrypt(passwdData.Email.ToUpper(), _keys.Key, _keys.Iv))).FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("7");//_7_USER_NOT_FOUND
                }
                else
                {
                    ConfirmEmail(passwdData.Email, passwdData.EmailToken);

                    byte[] userKey = Crypto.DecryptByte(user.Key, _keys.Key, _keys.Iv);
                    byte[] userIv = Crypto.DecryptByte(user.Iv, _keys.Key, _keys.Iv);

                    user.Password = Crypto.Encrypt(passwdData.newPassword, userKey, userIv);
                    _context.SaveChanges();

                    return Info.CHANGE_PASSWD_SUCCESSFUL;

                }




            }


        }


        public string UpdatePassword(SessionData sessionData, string Password, string newPassword)
        {
            if(sessionData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                var user = IsLogged(sessionData);

                if (ValidatePassword(user, Password))
                {

                    byte[] userKey = Crypto.DecryptByte(user.Key, _keys.Key, _keys.Iv);
                    byte[] userIv = Crypto.DecryptByte(user.Iv, _keys.Key, _keys.Iv);

                    user.Password = Crypto.Encrypt(newPassword, userKey, userIv);
                    _context.SaveChanges();

                    return Info.CHANGE_PASSWD_SUCCESSFUL;

                }
                else
                {
                    throw new Exception("8");//_8_PASSWORD_ERROR
                }


            }


        }

        public string UpdateUsername(SessionData sessionData, string newUsername)
        {

            if (sessionData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                var user = IsLogged(sessionData);

                user.Username = Crypto.Encrypt(newUsername, _keys.Key, _keys.Iv);
                user.UsernameNormalized = Crypto.Encrypt(newUsername.ToUpper(), _keys.Key, _keys.Iv);
                _context.SaveChanges();

                return Info.CHANGE_USERNAME_SUCCESSFUL;

            }


        }

        public string UpdateEmail(SessionData sessionData, string newEmail, string emailToken)
        {

            if (sessionData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {

                var user = IsLogged(sessionData);


                ConfirmEmail(newEmail, emailToken);

                user.Email = Crypto.Encrypt(newEmail.ToUpper(), _keys.Key, _keys.Iv);

                _context.SaveChanges();

                return Info.CHANGE_EMAIL_SUCCESSFULL;



            }


        }



        public Users IsLogged(SessionData sessionData)
        {

            var session = _context.SessionTokens.Where(s => s.UserId.Equals(sessionData.UserId) && s.SessionToken.Equals(Crypto.Encrypt(sessionData.Token, _keys.Key, _keys.Iv))).FirstOrDefault();

            if(session == null)
            {
                throw new Exception("1");//_1_SESSION_NOT_FOUND
            }
            else
            {
                DateTime now = DateTime.UtcNow;
                if(session.SessionLifetime < now)
                {
                    _context.SessionTokens.Remove(session);
                    _context.SaveChanges();
                    throw new Exception("17");//_17_SESSION_EXPIRED

                }

                var user = _context.Users.Where(u => u.Id.Equals(sessionData.UserId)).FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
                else
                {
                    return user;
                }

               
            }

        }

        public string ActiveSession(SessionData sessionData)
        {

            var session = _context.SessionTokens.Where(s => s.UserId.Equals(sessionData.UserId) && s.SessionToken.Equals(Crypto.Encrypt(sessionData.Token, _keys.Key, _keys.Iv))).FirstOrDefault();

            if (session == null)
            {
                throw new Exception("1");//_1_SESSION_NOT_FOUND
            }
            else
            {
                DateTime now = DateTime.UtcNow;
                if (session.SessionLifetime < now)
                {
                    _context.SessionTokens.Remove(session);
                    _context.SaveChanges();
                    throw new Exception("17");//_17_SESSION_EXPIRED

                }

                return Info.SESSION_ACTIVE;
            }

        }

        public bool ConfirmEmail(string email, string code)
        {

            if (email == null || code == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                var confirmationCode = _context.ConfirmationCodes.Where(c => c.Email.Equals(Crypto.Encrypt(email.ToUpper(), _keys.Key, _keys.Iv))).FirstOrDefault();

                if (confirmationCode != null)
                {
                    DateTime lifetime = confirmationCode.CodeLifetime;
                    DateTime now = DateTime.UtcNow;

                    if(now > lifetime)
                    {
                        _context.ConfirmationCodes.Remove(confirmationCode);
                        _context.SaveChanges();

                        throw new Exception("9");//_9_CONFIRM_CODE_TIME_ERROR
                    }
                    else
                    {

                        if (confirmationCode.Attempts >= 4)
                        {

                            throw new Exception("10");//_10_CONFIRM_CODE_ATTEMPT_ERROR

                        }
                        else
                        {

                            if (code.Equals(Crypto.Decrypt(confirmationCode.ConfirmationCode, _keys.Key, _keys.Iv)))
                            {

                                _context.ConfirmationCodes.Remove(confirmationCode);
                                _context.SaveChanges();

                                return true;

                            }
                            else
                            {
                                ++confirmationCode.Attempts;
                                _context.SaveChanges();

                                throw new Exception("11");//_11_CONFIRM_CODE_INVALID
                            }


                        }

                        
                    }
                }
                else
                {
                    throw new Exception("12");//_12_CONFIRM_CODE_NOT_FOUND
                }

            }


            
        }
        
        public string SaveConfirmationCode(string email, string validationCode)
        {
            if(email == null || validationCode == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (validationCode.Equals(Crypto.GetValidationCode()))
                {

                    var code = RandomNumberGenerator.GetHexString(6);

                    DateTime Lifetime = DateTime.UtcNow + new TimeSpan(0, 3, 0);
                    DateTime CodeDelay = DateTime.UtcNow + new TimeSpan(0, 0, 29);

                    ConfirmationCodes confirmationCode = new ConfirmationCodes
                    {
                        Email = Crypto.Encrypt(email.ToUpper(), _keys.Key, _keys.Iv),
                        ConfirmationCode = Crypto.Encrypt(code, _keys.Key, _keys.Iv),
                        CodeLifetime = Lifetime,
                        Attempts = 0,
                        Delay = CodeDelay

                    };

                    var old_code = _context.ConfirmationCodes.Where(c => c.Email.Equals(confirmationCode.Email)).FirstOrDefault();
                    if (old_code != null)
                    {

                        if(old_code.Delay > DateTime.UtcNow)
                        {
                            throw new Exception("20");//_18_CONFIRM_CODE_DELAY_ERROR
                        }
                        else
                        {
                            _context.ConfirmationCodes.Remove(old_code);
                        }


                    }

                    _context.ConfirmationCodes.Add(confirmationCode);
                    _context.SaveChanges();

                    return code;

                }
                else
                {
                    throw new Exception("13");//_13_VALIDATION_TOKEN_ERROR
                }

            }
        }

        public void DeleteExpiredCodes()
        {

            DateTime now = DateTime.UtcNow;

            List<ConfirmationCodes> ExpiredCodes = _context.ConfirmationCodes.Where(c => c.CodeLifetime < now).ToList();

            if (ExpiredCodes != null)
            {

                ExpiredCodes.ForEach(code => _context.ConfirmationCodes.Remove(code));
                _context.SaveChanges();

            }

        }


        public void DeleteExpiredSessions()
        {

            DateTime now = DateTime.UtcNow;

            List<SessionTokens> ExpiredSessions = _context.SessionTokens.Where(c => c.SessionLifetime < now).ToList();

            if (ExpiredSessions != null)
            {

                ExpiredSessions.ForEach(session => _context.SessionTokens.Remove(session));
                _context.SaveChanges();

            }

        }


        public byte[] EncryptData(EncryptData clientData)
        {

            if (clientData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                var user = IsLogged(clientData.SessionData);

                byte[] userKey;
                byte[] userIv;
                byte[] EncryptedData;

                try
                {

                    userKey = Crypto.DecryptByte(user.Key, _keys.Key, _keys.Iv);
                    userIv = Crypto.DecryptByte(user.Iv, _keys.Key, _keys.Iv);

                }
                catch (Exception)
                {

                    throw new Exception("3");//_3_DECRYPTION_ERROR
                }

                try
                {

                    EncryptedData = Crypto.Encrypt(clientData.DataToEncrypt, userKey, userIv);

                    return EncryptedData;

                }
                catch (Exception)
                {

                    throw new Exception("2");//_2_ENCRYPTION_ERROR
                }


            }


        }

        public string DecryptData(DecryptData clientData)
        {

            if (clientData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                var user = IsLogged(clientData.SessionData);

                try
                {

                    byte[] userKey = Crypto.DecryptByte(user.Key, _keys.Key, _keys.Iv);
                    byte[] userIv = Crypto.DecryptByte(user.Iv, _keys.Key, _keys.Iv);

                    string DecryptedData = Crypto.Decrypt(clientData.EncryptedData, userKey, userIv);

                    return DecryptedData;

                }
                catch (Exception)
                {
                    throw new Exception("3");//_3_DECRYPTION_ERROR
                }

            }


        }


        public List<EncryptedObject> EncryptDataList(EncryptDataList clientListData)
        {

            if (clientListData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                var user = IsLogged(clientListData.SessionData);

                byte[] userKey;
                byte[] userIv;

                try
                {

                    userKey = Crypto.DecryptByte(user.Key, _keys.Key, _keys.Iv);
                    userIv = Crypto.DecryptByte(user.Iv, _keys.Key, _keys.Iv);

                }
                catch (Exception)
                {

                    throw new Exception("3");//_3_DECRYPTION_ERROR
                }

                try
                {

                    List<EncryptedObject> EncryptedData = new List<EncryptedObject>();

                    clientListData.DataToEncrypt.ForEach(piece => EncryptedData.Add(new EncryptedObject { id = piece.id, encryptedValue = Crypto.Encrypt(piece.decryptedValue, userKey, userIv) }));

                    return EncryptedData;

                }
                catch (Exception)
                {

                    throw new Exception("2");//_2_ENCRYPTION_ERROR
                }

            }

        }


        public List<DecryptedObject> DecryptDataList(DecryptDataList clientListData)
        {

            if (clientListData == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                var user = IsLogged(clientListData.SessionData);

                byte[] userKey;
                byte[] userIv;

                try
                {

                    userKey = Crypto.DecryptByte(user.Key, _keys.Key, _keys.Iv);
                    userIv = Crypto.DecryptByte(user.Iv, _keys.Key, _keys.Iv);

                }
                catch (Exception)
                {

                    throw new Exception("3");//_3_DECRYPTION_ERROR
                }

                try
                {

                    List<DecryptedObject> DecryptedData = new List<DecryptedObject>();

                    clientListData.DataToDecrypt.ForEach(piece => DecryptedData.Add(new DecryptedObject { id = piece.id, decryptedValue = Crypto.Decrypt(piece.encryptedValue, userKey, userIv) }));

                    return DecryptedData;

                }
                catch (Exception)
                {

                    throw new Exception("2");//_2_ENCRYPTION_ERROR
                }

            }

        }

    }
}
