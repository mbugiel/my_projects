namespace AuthenticationAPI.Helper.Service
{
    public class EncryptDataList
    {
        public SessionData SessionData { get; set; }
        public List<DecryptedObject> DataToEncrypt { get; set; }
    }
}
