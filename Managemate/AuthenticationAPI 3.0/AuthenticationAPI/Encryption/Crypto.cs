using AuthenticationAPI.Helper;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationAPI.Encryption
{
    public static class Crypto
    {

        private static byte[] key;
        private static byte[] iv;
        private static string passwd;
        private static string validationCode = "#####";

        public static byte[] Encrypt(string simpletext, byte[] key, byte[] iv)
        {
            byte[] cipheredtext;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(simpletext);
                            streamWriter.Flush();
                        }

                        cipheredtext = memoryStream.ToArray();
                    }
                }
            }
            return cipheredtext;
        }


        public static string Decrypt(byte[] cipheredtext, byte[] key, byte[] iv)
        {
            string simpletext = String.Empty;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream(cipheredtext))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            simpletext = streamReader.ReadToEnd();
                            streamReader.Close();
                        }
                    }
                }
            }
            return simpletext;
        }



        public static byte[] EncryptByte(byte[] simpleByte, byte[] key, byte[] iv)
        {
            byte[] cipheredByte;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (BinaryWriter streamWriter = new BinaryWriter(cryptoStream))
                        {
                            streamWriter.Write(simpleByte);
                            streamWriter.Flush();
                        }

                        cipheredByte = memoryStream.ToArray();
                    }
                }
            }
            return cipheredByte;
        }


        public static byte[] DecryptByte(byte[] cipheredByte, byte[] key, byte[] iv)
        {
            byte[] simpleByte;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream(cipheredByte))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (BinaryReader streamReader = new BinaryReader(cryptoStream))
                        {
                            simpleByte = streamReader.ReadBytes(16);
                            streamReader.Close();
                        }
                    }
                }
            }
            return simpleByte;
        }



        public static Keys GetKeys()
        {
            if(key == null || iv == null ||  key.Length == 0 || iv.Length == 0)ReadKeys();

            return new Keys { Key = key, Iv = iv };
        }


        public static string GetPasswd()
        {
            if (passwd == null ||  passwd.Length == 0 || passwd.Equals("") || passwd.Equals(string.Empty)) ReadPasswd();

            return passwd;
        }



        public static string GetValidationCode()
        {
            return validationCode;
        }

        public static void ReadKeys()
        {
            var path = SystemPath.ENCRYPT_KEYS_PATH;

            if (File.Exists(path))
            {
                var stream = File.Open(path, FileMode.Open);

                using (BinaryReader reader = new BinaryReader(stream))
                {

                    key = reader.ReadBytes(16);
                    iv = reader.ReadBytes(16);

                    reader.Close();

                }

                stream.Close();


            }
            else
            {
                var stream = File.Create(path);
                using(BinaryWriter writer = new BinaryWriter(stream))
                {
                    RandomNumberGenerator rng = RandomNumberGenerator.Create();
                    key  = new byte[16];
                    iv = new byte[16];

                    rng.GetBytes(key);
                    rng.GetBytes(iv);

                    writer.Write(key);
                    writer.Write(iv);
                    
                    writer.Flush();
                    writer.Close();
                }
                stream.Close();
            }
        }

        

        public static void ReadPasswd()
        {

            var path = SystemPath.PASSWD_PATH;
            Keys keysPasswd = GetKeys();

            if (File.Exists(path))
            {
                var stream = File.Open(path, FileMode.Open);

                using (BinaryReader reader = new BinaryReader(stream))
                {

                    byte[] pass = reader.ReadBytes(128);

                    passwd = Decrypt(pass, keysPasswd.Key, keysPasswd.Iv);

                    reader.Close();

                }

                stream.Close();


            }
            else
            {
                var stream = File.Create(path);
                using (BinaryWriter writer = new BinaryWriter(stream))
                {

                    StreamReader reader = new StreamReader(path+".txt");

                    passwd = reader.ReadToEnd();
                    reader.Close();

                    byte[] pass = Encrypt(passwd, keysPasswd.Key, keysPasswd.Iv);

                    writer.Write(pass);

                    writer.Flush();
                    writer.Close();
                }
                stream.Close();
            }
        }



    }
}
