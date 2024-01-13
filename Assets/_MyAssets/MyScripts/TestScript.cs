using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.mkadmi
{
    public class TestScript : MonoBehaviour
    {
        public bool canShowUsersInfo = false;
        public bool canChangeTheme = false;

        public async void FetchUsers()
        {
            List<UserIdMap_Model> users = await UserIdMap_Controller.Instance().GetAllUserIdMaps();

            foreach (UserIdMap_Model user in users)
            {
                Debug.Log(user.ToJson().ToString());
            }
        }

        private void Update()
        {
            if (canShowUsersInfo)
            {
                canShowUsersInfo = false;

                Debug.Log(User.Instance().ToJson());
                UserSettings.Instance().Log_User_Settings();
            }
            if (canChangeTheme)
            {
                canChangeTheme = false;
                if (UserSettings.Instance().THEME_NAME == "dark")
                UserSettings.Instance().Set_THEME_NAME("light");
                else
                    UserSettings.Instance().Set_THEME_NAME("dark");
            }
        }

    }
}
