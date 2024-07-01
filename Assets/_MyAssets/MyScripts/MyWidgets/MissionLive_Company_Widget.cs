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
        Sprite profilePic { get; set; }

        Company_Model companyModel;
        public void SetMe(Company_Model companyModel string userName, int userLevelAmount, float starsNote, Sprite profilePic)
        {
            this.companyModel = companyModel;
            Update_UI();
        }

        void Update_UI()
        {
            this.userName = companyModel.CompanyName;
            this.userLevelAmount = userLevelAmount;
            this.starsNote = starsNote;
            this.profilePic = profilePic;
            _CompanyNameTxt.text = companyModel.CompanyName;
            _CompanyLevelTxt.text = userLevelAmount.ToString();
            int i = 0;
            for (i = 0; i < (int)starsNote; i++)
            {
                _StarsImgs[i].fillAmount = 1;
            }
            if (i != 5) _StarsImgs[i].fillAmount = (starsNote - (int)starsNote);

            _ProfilePictureImg.sprite = profilePic;
        }
    }
}
