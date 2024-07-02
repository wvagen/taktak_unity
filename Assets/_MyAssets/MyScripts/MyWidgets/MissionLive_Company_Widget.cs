using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.mkadmi
{
    public class MissionLive_Company_Widget : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _CompanyNameTxt;
        [SerializeField]
        private TextMeshProUGUI _CompanyLevelTxt;
        [SerializeField]
        private List<Image> _StarsImgs;
        [SerializeField]
        private Image _ProfilePictureImg;

        Company_Model companyModel;
        public void SetMe(Company_Model companyModel)
        {
            this.companyModel = companyModel;
            Update_UI();
        }

        void Update_UI()
        {
            this._CompanyNameTxt.text = companyModel.CompanyName;
            this._CompanyLevelTxt.text = Tools.levelReached(companyModel.XpPoints).ToString();
            _CompanyNameTxt.text = companyModel.CompanyName;
            _CompanyLevelTxt.text = Tools.levelReached(companyModel.XpPoints).ToString();
            float starsNote = companyModel.Rating ?? 0;

            int i = 0;
            for (i = 0; i < (int)companyModel.Rating; i++)
            {
                _StarsImgs[i].fillAmount = 1;
            }
            if (i != 5) _StarsImgs[i].fillAmount = (starsNote - (int)starsNote);

            _ProfilePictureImg.sprite = companyModel.CompanySprite;
        }
    }
}
