using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UPersian.Components;

namespace com.mkadmi {
    public class View_Home_Manager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _userNameTxt;


        [SerializeField]
        private Slider _levelSlider;
        [SerializeField]
        private TextMeshProUGUI _levelSliderText;
        [SerializeField]
        private TextMeshProUGUI _levelTxt;


        [SerializeField]
        private Image _photoImg;

        private void Start()
        {
            Fetch_Users_Info();
        }

        public void Fetch_Users_Info()
        {
            _userNameTxt.text = User.Instance().UserName;
            int levelReached = Tools.levelReached(User.Instance().XpPoints);
            _levelTxt.text = levelReached.ToString();
            _levelSliderText.text = User.Instance().XpPoints.ToString() + "/" + Tools.expNeededToReachLevel(levelReached).ToString();

            if (levelReached == 1)
            {                
                _levelSlider.value = (User.Instance().XpPoints) / (float)Tools.expNeededToReachLevel(levelReached);
            }
            else
            {
                ulong totalValue = Tools.expNeededToReachLevel(levelReached) - Tools.expNeededToReachLevel(levelReached - 1);
                ulong wentValue = User.Instance().XpPoints - Tools.expNeededToReachLevel(levelReached - 1);
                _levelSlider.value = (float)wentValue / totalValue;
            }
            Load_User_Img();
        }

        void Load_User_Img()
        {
           string imgPath = Path.Combine(User.Instance().Id, User.Instance().PhotoPath);
            //User.Instance().UserPhoto = await Tools.Fetch_User_Img(imgPath, _photoImg);
            //_photoImg.sprite = User.Instance().UserPhoto;
            Tools.Fetch_User_Img(imgPath, _photoImg);
        }

    }
}