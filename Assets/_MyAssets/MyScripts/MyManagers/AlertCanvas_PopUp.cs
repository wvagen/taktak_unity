using System;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UPersian.Components;

namespace com.mkadmi
{
    public class AlertCanvas_PopUp : MonoBehaviour
    {
        [SerializeField]
        private UIToggle _PopUpPanel;       
        [SerializeField]
        private RtlText _TitleTxt,_BodyTxt;

        Action okBtnCallBack, noBtnCallBack;


        public void ShowPanel(string title, string body, Action okCallBack = null, Action noCallBack = null)
        {
            if (okCallBack != null)
            {
                okBtnCallBack = okCallBack;
            }

            if (noCallBack != null)
            {
                noBtnCallBack = noCallBack;     
            }

            _TitleTxt.text = title;
            _BodyTxt.text = body;

            _PopUpPanel.IsOn = true;
        }

        public void HidePanel()
        {

            if (okBtnCallBack != null)
            {
                okBtnCallBack.Invoke();
                okBtnCallBack = null;
            }

            if (noBtnCallBack != null)
            {
                noBtnCallBack.Invoke();
                noBtnCallBack = null;
            }

            _PopUpPanel.IsOn = false;
        }

    }
}
