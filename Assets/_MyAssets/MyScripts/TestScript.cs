using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace com.mkadmi
{
    public class TestScript : MonoBehaviour
    {
        public bool canShowUsersInfo = false;
        public bool canChangeTheme = false;
        public bool canFetchFakeData = false;
        public bool canSpawnMissionLive = false;
        public bool canGenerateFakeMissionData = false;

        public bool canShowPanel = false;
        public int panelIndexToShow = 0;

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

        public ulong XpPoints ;

        public string Status ;

        public float? Rating ;

        public short? ReportCount;

        public GameObject missionLiveGO;
        public Transform missionLiveLocation;
        public string userName;
        public int userLevelAmount;
        public float starsNote;
        public Sprite profilePic;
        public List<Sprite> randomPics;

        public string missionDesc;
        public int rewardCoins;
        public int expAmount;
        public int committers;

        public float timeSeconds = 10;

        public string snackBarContent;
        public int snackBarType;
        public bool showSnackbar;

        private void Awake()
        {
            if (canFetchFakeData)
            {
                canFetchFakeData = false;
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
                FindObjectOfType<View_Home_Manager>().Fetch_Users_Info();
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
            if (canFetchFakeData)
            {
                canFetchFakeData = false;
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
                FindObjectOfType<View_Home_Manager>().Fetch_Users_Info();
            }

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
            if (canSpawnMissionLive)
            {
                canSpawnMissionLive = false;
                GenerateRandomMission();
            }
            if (canGenerateFakeMissionData)
            {
                canGenerateFakeMissionData = false;
                userName = GenerateRandomWord(5);
                missionDesc = userName = GenerateRandomWord(25);
                starsNote = UnityEngine.Random.Range(0, 5);
                userLevelAmount = UnityEngine.Random.Range(0, 100);
                rewardCoins = UnityEngine.Random.Range(10, 100);
                expAmount = UnityEngine.Random.Range(10, 100);
                committers = UnityEngine.Random.Range(0, 100);
                timeSeconds = UnityEngine.Random.Range(10, 3600);
                profilePic = randomPics[UnityEngine.Random.Range(0, randomPics.Count)];
            }

            if (canShowPanel)
            {
                canShowPanel = false;
                switch (panelIndexToShow)
                {
                    case 0: AlertCanvas.Instance().ShowInfoPanel("Mouadh", "Mkadmi", YesCallBack);break;
                    case 1: AlertCanvas.Instance().ShowWarningPanel("Mouadh", "Mkadmi", YesCallBack,NoCallback); break;
                    case 2: AlertCanvas.Instance().ShowErrorPanel("Mouadh", "Mkadmi", YesCallBack); break;
                    case 3: AlertCanvas.Instance().ShowInfoPanel(GenerateRandomWord(5), GenerateRandomWord(20)); break;
                }
            }

            if (showSnackbar)
            {
                showSnackbar = false;
                FindAnyObjectByType<AlertCanvas>().Show_SnackBar(snackBarContent, snackBarType);
            }
        }

        void YesCallBack()
        {
            Debug.Log("Yes CallBack function has been invoked!");
        }

        void NoCallback()
        {
            Debug.Log("No CallBack function has been invoked!");
        }

        public void GenerateRandomMission()
        {
            //To get random Users ID
            int randomLengthCommitters = UnityEngine.Random.Range(10, 100);
            string[] randomCommittersID = new string[randomLengthCommitters];
            for (int i = 0; i < randomLengthCommitters; i++)
            {
                randomCommittersID[i] = GenerateRandomWord(5);
            }

            //End of gettings random users IDs

            Company_Model company_Model = new Company_Model();
            company_Model.CompanyName = GenerateRandomWord(5);
            company_Model.XpPoints = (ulong)UnityEngine.Random.Range(0, 50000);
            company_Model.Rating = (ulong)UnityEngine.Random.Range(0, 5);
            company_Model.CompanySprite = randomPics[UnityEngine.Random.Range(0, randomPics.Count)];

            MissionLive_Model missionLiveModel = new MissionLive_Model();

            missionLiveModel.RewardVirtCoins = UnityEngine.Random.Range(10, 100);
            missionLiveModel.Deadline = DateTime.Today.AddDays(2);
            missionLiveModel.RewardXp = UnityEngine.Random.Range(10, 100);
            missionLiveModel.ItemPrice = UnityEngine.Random.Range(0.1f, 30f);
            missionLiveModel.Title = GenerateRandomWord(5);

            MissionLive_Widget missionLive = Instantiate(missionLiveGO, missionLiveLocation).GetComponent<MissionLive_Widget>();
            missionLive.Set_Company_Profile(company_Model);
            missionLive.Set_Mission_Props(missionLiveModel);
        }

        static string GenerateRandomWord(int maxLength)
        {
            // Define the characters that can be used in the random word
            const string allowedCharacters = "abcdefghijklmnopqrstuvwxyz0123456789 ";

            // Initialize a StringBuilder to build the random word
            StringBuilder randomWordBuilder = new StringBuilder();

            // Generate random word
            int length = UnityEngine.Random.Range(1, maxLength + 1);
            for (int i = 0; i < length; i++)
            {
                // Append a random character from the allowedCharacters
                randomWordBuilder.Append(allowedCharacters[UnityEngine.Random.Range(0, allowedCharacters.Length)]);
            }

            // Convert StringBuilder to string and return
            return randomWordBuilder.ToString();
        }

    }
}
