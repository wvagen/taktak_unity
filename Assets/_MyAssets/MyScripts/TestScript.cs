using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.mkadmi
{
    public class TestScript : MonoBehaviour
    {
        public bool canShowUsersInfo = false;
        public bool canChangeTheme = false;
        public bool canFetchFakeData = false;

        public string Id ;

        public long Number ;

        public string UserName ;

        public string Name ;

        public string Surname ;

        public string OauthId ;

        public string OauthType ;

        public string PhoneNumber ;

        public string PhotoPath ;

        public long VirtualCoins ;

        public double XpPoints ;

        public string Status ;

        public float? Rating ;

        public short? ReportCount;

        private void Awake()
        {
            if (canFetchFakeData)
            {
                User_Model user = new User_Model();

                user.Id = Id;
                user.Number = Number;
                user.UserName = UserName;
                user.Name = Name;
                user.Surname = Surname;
                user.OauthId = OauthId;
                user.OauthType = OauthType;
                user.PhoneNumber = PhoneNumber;
                user.PhotoPath = PhotoPath;
                user.VirtualCoins = VirtualCoins;
                user.XpPoints = XpPoints;
                user.Status = Status;
                user.Rating = Rating;
                user.ReportCount = ReportCount;

                User.Instance().SetMe(user);
            }
        }

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
