using UnityEngine;

public class UserSettings
{
    #region Player Prefs
    const string THEME_NAME_KEY = "light";
    public const string CURRENT_APP_VERSION_KEY = "current_version";
    #endregion

    #region Scene Name
    public const string SCENE_AUTH = "Client_0_AuthScene";
    public const string SCENE_MAIN_MENU = "Client_1_MainScene";

    #endregion

    #region Params
    public const short MAX_LEVEL = 50;
    public const short PING_RATE = 5;
    #endregion

    #region bucket paths

    public const string USER_FILES_PATH = "User_Files";

    #endregion


#if UNITY_ANDROID

    public const short APP_VERSION = 0;

#elif UNITY_IOS
    public const short APP_VERSION = 0;
#else
    public const short APP_VERSION = 0;
#endif

    #region update links

#if UNITY_ANDROID

    public const string UPDATE_LINK = "https://google.fr/";

#elif UNITY_IOS
    public const string UPDATE_LINK = "https://google.fr/";
#else
    public const string UPDATE_LINK = "https://google.fr/";
#endif

    #endregion

    #region API Links
    public static string TIME_API = "http://worldtimeapi.org/api/ip";
    #endregion

    public string THEME_NAME { get; set; } = "light";

    static UserSettings _instance;

    public static UserSettings Instance()
    {
        if (_instance == null)
        {
            _instance = new UserSettings();

            //All settings must be mentionned
            _instance.THEME_NAME = PlayerPrefs.GetString(THEME_NAME_KEY, "light");
        }

        return _instance;
    }

    public void Set_THEME_NAME (string themeName)
    {
        PlayerPrefs.SetString(THEME_NAME_KEY, themeName);
        _instance.THEME_NAME = themeName;
    }

    public void Log_User_Settings()
    {
        Debug.LogFormat("Theme: {0}", THEME_NAME);
    }

}
