using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionLive_User_Widget : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _UserNameTxt;
    [SerializeField]
    private TextMeshProUGUI _UserLevelTxt;
    [SerializeField]
    private List<Image> _StarsImgs;
    [SerializeField]
    private Image _ProfilePictureImg;

    public string userName { get; set; }
    int userLevelAmount { get; set; }
    float starsNote { get; set; }
    Sprite profilePic { get; set; }

    public void SetMe(string userName, int userLevelAmount, float starsNote, Sprite profilePic)
    {
        this.userName = userName;
        this.userLevelAmount = userLevelAmount;
        this.starsNote = starsNote;
        this.profilePic = profilePic;
        Update_UI();
    }

    void Update_UI()
    {
        _UserNameTxt.text = userName;
        _UserLevelTxt.text = userLevelAmount.ToString();
        int i = 0;
        for (i = 0; i < (int)starsNote; i++)
        {
            _StarsImgs[i].fillAmount = 1;
        }
        if (i != 5) _StarsImgs[i].fillAmount = (starsNote - (int)starsNote);

        _ProfilePictureImg.sprite = profilePic;
    }
}
