using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
using TMPro;

public class MissionLive_Widget : MonoBehaviour
{
    [SerializeField]
    private RtlText _MissionDescription;

    [SerializeField]
    private TextMeshProUGUI _RewardVirCoinsTxt;
    [SerializeField]
    private TextMeshProUGUI _RewardExpTxt;
    [SerializeField]
    private TextMeshProUGUI _CommittersTxt;

    [SerializeField]
    private MissionLive_User_Widget _MyUserWidget;

    public void SetMe(string userName,int userLevelAmount,float starsNote,Sprite profilePic)
    {
        _MyUserWidget.SetMe(userName, userLevelAmount, starsNote, profilePic);
    }
}
