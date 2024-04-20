namespace AuthenticationAPI.Helper.Service
{
    public class DecryptDataList
    {
        public SessionData SessionData { get; set; }
        public List<EncryptedObject> DataToDecrypt { get; set; }
    }
}
