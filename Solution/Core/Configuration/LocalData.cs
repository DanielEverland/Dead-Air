using System.IO;
using Serialization;

namespace Configuration
{
    /// <summary>
    /// Handles data stored in the Local directory on Windows
    /// </summary>
    public static class LocalData {
    
        static LocalData()
        {
            Directories.EnsurePathExists(Directories.Persistant);
        }

        public static int AccountID
        {
            get
            {
                if (_accountID == null)
                    LoadAccountID();

                return _accountID.Value;
            }
        }

        private static int? _accountID;

        private const string ACCOUNT_ID_NAME = "Account";

        private static string AccountIDFullPath
        {
            get
            {
                return $@"{Directories.Persistant}\{ACCOUNT_ID_NAME}";
            }
        }

        private static void LoadAccountID()
        {
            if (!File.Exists(AccountIDFullPath))
            {
                CreateAccountID();
            }

            byte[] data = File.ReadAllBytes(AccountIDFullPath);
            _accountID = ByteConverter.Deserialize<int>(data);
        }
        private static void CreateAccountID()
        {
            int id = Utility.RandomInt();

            byte[] data = ByteConverter.Serialize(id);
            File.WriteAllBytes(AccountIDFullPath, data);
        }
    }
}
