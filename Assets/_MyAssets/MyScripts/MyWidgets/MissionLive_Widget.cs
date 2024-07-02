using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
using TMPro;
using System;

namespace com.mkadmi {
    public class MissionLive_Widget : MonoBehaviour
    {
        #region Mission Area
        [SerializeField]
        private TextMeshProUGUI _RewardVirCoinsTxt;
        [SerializeField]
        private TextMeshProUGUI _RewardExpTxt;
        [SerializeField]
        private TextMeshProUGUI _CommittersTxt;
        [SerializeField]
        private TextMeshProUGUI _PriceTxt;
        [SerializeField]
        private RtlText _MissionDescription;
        #endregion

        [SerializeField]
        private MissionLive_Company_Widget _MyCompanyWidget;

        #region Slider Area
        [SerializeField]
        private Slider _TimeSlider;
        [SerializeField]
        private Image _SliderFillImg;
        [SerializeField]
        private Color[] _sliderColors;
        [SerializeField]
        private TextMeshProUGUI _sliderRemainingTimeTxt;
        #endregion

        MissionLive_Model missionLiveModel;

        public void Set_Company_Profile(Company_Model companyModel)
        {
            _MyCompanyWidget.SetMe(companyModel);
        }

        void Set_Timer(float timeLimit)
        {
            StartCoroutine(StartSlider(timeLimit));
        }

        public void Set_Mission_Props(MissionLive_Model missionLiveModel)
        {
            this.missionLiveModel = missionLiveModel;
            _MissionDescription.text = missionLiveModel.Title;
            _RewardVirCoinsTxt.text = missionLiveModel.RewardVirtCoins.ToString();
            _RewardExpTxt.text = missionLiveModel.RewardXp.ToString();
            _CommittersTxt.text = missionLiveModel.CommittersId.Length.ToString();
            _PriceTxt.text = missionLiveModel.ItemPrice.ToString("F2") + " TND";
            Set_Timer((float)(missionLiveModel.Deadline - CorrectTime.Instance().realDate).TotalSeconds);
        }
    
    IEnumerator StartSlider(float timeSeconds)
        {
            float remainingTime = timeSeconds;
            TimeSpan t = TimeSpan.FromSeconds(remainingTime);

            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;

                if (remainingTime <= 0)
                {
                    remainingTime = 0;
                    _TimeSlider.value = 0;
                }
                else _TimeSlider.value = remainingTime / timeSeconds;

                t = TimeSpan.FromSeconds(remainingTime);
                _sliderRemainingTimeTxt.text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                    t.Hours,
                    t.Minutes,
                    t.Seconds);

                if (_TimeSlider.value > 0.7f) _SliderFillImg.color = _sliderColors[0];
                else if (_TimeSlider.value > 0.3f) _SliderFillImg.color = _sliderColors[1];
                else _SliderFillImg.color = _sliderColors[2];

                yield return new WaitForEndOfFrame();
            }
        }

    }
}
