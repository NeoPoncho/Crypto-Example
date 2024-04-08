namespace DeliveryProject.Models
{
    public static class DB
    {
        private const string encryptionPassphrase = "UIr<Fy+{Z#fcL:xlaNP_)P2@zgKh#M";

        private const string LocalDBfilename = "DatabaseDelivery.db3";

        public static string LocalDatabasePath
        {
            get
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(folderPath, LocalDBfilename);
            }
        }

        public static string EncryptionKey
        {
            get
            {
                return Path.Combine(encryptionPassphrase);
            }
        }
    }
}
