using UnityEngine;

namespace TandC.GeometryAstro.Settings
{
    public class AppConstants
    {
        public const string LOCAL_APP_DATA_FILE_PATH = "/BARAKUDA0002AGENT.data";
        public const string LOCAL_PURCHASE_DATA_FILE_PATH = "/BARAKUDA0003AGENT.data";
        public const string LOCAL_PLAYER_VAULT_DATA_FILE_PATH = "/BARAKUDA0004AGENT.data";
        public const string LOCAL_PLAYER_DATA_FILE_PATH = "/BARAKUDA0005AGENT.data";
        public const string LOCAL_UPGRADE_DATA_FILE_PATH = "/BARAKUDA0006AGENT.data";
        
        public const string ADDITIONAL_LOCAL_DATA_FILE_PATH = "/15FDFTG842SDJTN248STH.data";

        private const string ENCRYPT_SALT = "GA_2026_s@lt_v1";

        private static string _encryptKey;
        public static string EncryptKeyData
        {
            get
            {
                if (string.IsNullOrEmpty(_encryptKey))
                {
                    _encryptKey = GenerateDeviceKey();
                }
                return _encryptKey;
            }
        }

        private static string GenerateDeviceKey()
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            string combined = deviceId + ENCRYPT_SALT;
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combined));
                return System.Convert.ToBase64String(hash).Substring(0, 32);
            }
        }

        public static string PATH_TO_GAMES_CACHE = $"{Application.persistentDataPath}/Game/Cache";

        public static bool IS_TEST_MODE = false;

        public static bool DEBUG_ENABLE = true;

        public static bool LANGUAGE_CAN_CHANGE_IN_GAME = true;
    }
}