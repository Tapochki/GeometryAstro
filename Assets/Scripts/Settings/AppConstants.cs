using UnityEngine;

namespace ChebDoorStudio.Settings
{
    public class AppConstants
    {
        public const string LOCAL_USER_DATA_FILE_PATH = "/BARAKUDA0001AGENT.data";
        public const string LOCAL_APP_DATA_FILE_PATH = "/BARAKUDA0002AGENT.data";
        public const string LOCAL_PURCHASE_DATA_FILE_PATH = "/BARAKUDA0003AGENT.data";
        public const string LOCAL_PLAYER_VAULT_DATA_FILE_PATH = "/BARAKUDA0004AGENT.data";
        public const string LOCAL_SHOP_DATA_FILE_PATH = "/BARAKUDA0005AGENT.data";
        public const string LOCAL_SKIN_DATA_FILE_PATH = "/BARAKUDA0006AGENT.data";

        public const string ADDITIONAL_LOCAL_DATA_FILE_PATH = "/15FDFTG842SDJTN248STH.data";
        public const string ENCRYPT_KEY_DATA = "PRIVATE_KEY_GAME_NAME_noetonetochno";

        public static string PATH_TO_GAMES_CACHE = $"{Application.persistentDataPath}/Game/Cache";

        public static bool IS_TEST_MODE = false;

        public static bool DEBUG_ENABLE = true;

        public static bool LANGUAGE_CAN_CHANGE_IN_GAME = true;

        public static bool UPDATE_LOCALISATION_FROM_SPREADSHEET = true;

        public static int COUNT_OF_SKILLS_FOR_CHOOSING = 3;

        public static float TIME_TO_EVOLUTION_ENEMY = 300.0f;
    }
}