using Studio.Settings;
using UnityEngine;

namespace Studio.Utilities
{
    public class Logger
    {
        public static void Log(string message, LogTypes type)
        {
            if (!AppConstants.DEBUG_ENABLE)
            {
                return;
            }

            switch (type)
            {
                case LogTypes.Info:
                    Debug.Log("<color=#52b788>[INFO]</color> " + message);
                    break;

                case LogTypes.Warning:
                    Debug.Log("<color=#fdffb6>[WARNING]</color> " + message);
                    break;

                case LogTypes.Error:
                    Debug.Log("<color=#f72585>[ERROR]</color> " + message);
                    break;

                case LogTypes.Debug:
                    Debug.Log("<color=#8ecae6>[DEBUG]</color> " + message);
                    break;
            }
        }

        public static void NotImplementedLog(string message)
        {
            Debug.Log("<color=#fdffb6>[WARNING]</color> " + $"Type - [{message}], not implemented!");
        }
    }
}