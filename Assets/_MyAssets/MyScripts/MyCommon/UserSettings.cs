using UnityEngine;

public class UserSettings
{
    #region Player Prefs
    const string THEME_NAME_KEY = "light";
    #endregion

    #region Scene Name
    public const string SCENE_AUTH = "0_AuthScene";
    public const string SCENE_MAIN_MENU = "1_MainScene";

    #endregion

    #region
    public const short MAX_LEVEL = 50;
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
