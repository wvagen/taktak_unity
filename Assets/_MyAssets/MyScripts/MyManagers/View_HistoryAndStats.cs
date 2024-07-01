using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UPersian.Components;
using Doozy.Runtime.UIManager.Components;

namespace com.mkadmi {
    public class View_HistoryAndStats : MonoBehaviour
    {
        [SerializeField]
        private Image _photoImg;

        [SerializeField]
        private RtlText _selectedDeadlineTxt;

        [SerializeField]
        private RtlText _selectedCoinsTxt;

        [SerializeField]
        private RtlText _MissionBodyTxt;

        [SerializeField]
        private MyToggleGroup _RewardGroup;

        [SerializeField]
        private MyToggleGroup _DeadlineGroup;

        private void Awake()
        {
            Init();
            Load_User_Img();
        }

        void Init()
        {
            _RewardGroup.Set_Me(this);
            _DeadlineGroup.Set_Me(this);
        }

        void Load_User_Img()
        {
            string imgPath = Path.Combine(User.Instance().Id, User.Instance().PhotoPath);
            //User.Instance().UserPhoto = await Tools.Fetch_User_Img(imgPath, _photoImg);
            //_photoImg.sprite = User.Instance().UserPhoto;
            Tools.Fetch_User_Img(imgPath, _photoImg);
        }

        public void Submit_Request()
        {
            AlertCanvas.Instance().ShowWarningPanel("Submit", "Are you sure you want to submit a new mission ?", Upload_New_Mission, null);
        }

        async void Upload_New_Mission()
        {
            MissionLive_Model newMission = new MissionLive_Model();
            newMission.Location = "Mestir";
            newMission.RewardXp = 300;
            newMission.RewardVirtCoins = RewardValue();
            newMission.Title = _MissionBodyTxt.text;
            //newMission.RequesterId = User.Instance().Id;
            newMission.Deadline = DeadlineValue();
            string[] commitersID = {""};
            newMission.CommittersId = commitersID;
            await MissionLive_Controller.Instance().CreateMission(newMission);
            AlertCanvas.Instance().Show_SnackBar("Success", 1);
            
        }

        public void Update_UI()
        {
            _selectedCoinsTxt.text = RewardValue().ToString();
            _selectedDeadlineTxt.text = (DeadlineValue() - CorrectTime.Instance().realDate).TotalHours.ToString() + " hours";
        }


        int RewardValue()
        {
            switch (_RewardGroup.enabledToggle.transform.GetSiblingIndex())
            {
                case 0: return 1;
                case 1: return 5;
                case 2: return 10;
                case 3: return 20;
                case 4: return 50;
                case 5: return 100;
                default: return 0;
            }
        }

        DateTime DeadlineValue()
        {
            DateTime currentDate = CorrectTime.Instance().realDate;

            switch (_DeadlineGroup.enabledToggle.transform.GetSiblingIndex())
            {
                case 0: return currentDate.AddHours(1);
                case 1: return currentDate.AddHours(5);
                case 2: return currentDate.AddHours(10);
                case 3: return currentDate.AddDays(1);
                case 4: return currentDate.AddDays(7);
                case 5: return currentDate.AddMonths(1);
                default: return currentDate;
            }
        }

    }
}
