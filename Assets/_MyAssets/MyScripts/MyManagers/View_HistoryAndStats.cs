using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UPersian.Components;

namespace com.mkadmi {
    public class View_HistoryAndStats : MonoBehaviour
    {
        [SerializeField]
        private Image _photoImg;

        [SerializeField]
        private RtlText _MissionBodyTxt;

        private void Start()
        {
            Load_User_Img();
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
            newMission.RewardVirtCoins = 10;
            newMission.Title = _MissionBodyTxt.text;
            newMission.RequesterId = User.Instance().Id;
            long[] commitersID = { 39827, 3209 };
            newMission.CommittersId = commitersID;
            await MissionLive_Controller.Instance().CreateMission(newMission);
        }
    }
}
