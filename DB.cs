namespace Application.Models
{
    public static class DB
    {
        private const string encryptionPassphrase = "UIr<Fy+{Z#fcL:xlaNP_)P2@zgKh#M";

        public static string EncryptionKey
        {
            get
            {
                return Path.Combine(encryptionPassphrase);
            }
        }
    }
}
