using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.mkadmi
{
    public class MyToggleGroup : MonoBehaviour
    {
        private View_HistoryAndStats _man;

        private List<MyToggle> myToggles = new List<MyToggle>();

        public MyToggle enabledToggle;

        // Start is called before the first frame update
        void Start()
        {
            GetMyToggles();
        }

        public void Set_Me(View_HistoryAndStats _man)
        {
            this._man = _man;
        }

        void GetMyToggles()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                myToggles.Add(transform.GetChild(i).GetComponent<MyToggle>());
                myToggles[i].Set_Me(this);
            }
            enabledToggle = myToggles[0];
            enabledToggle.ClickMe();
        }

        public void Select_Toggle(MyToggle newToggle)
        {
            enabledToggle.Diselect();
            enabledToggle = newToggle;
            newToggle.Select();

            _man.Update_UI();
        }
        
    }
}
