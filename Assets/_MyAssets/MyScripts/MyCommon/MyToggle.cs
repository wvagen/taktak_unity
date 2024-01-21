using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.mkadmi
{
    public class MyToggle : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> objects;

        public bool isON;

        private MyToggleGroup _myToggleGroup;

        public void Set_Me(MyToggleGroup _myToggleGroup)
        {
            this._myToggleGroup = _myToggleGroup;
        }

        public void Select()
        {
            isON = true;
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].SetActive(true);
            }
        }

        public void Diselect()
        {
            isON = false;
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].SetActive(false);
            }
        }

        public void ClickMe()
        {
            _myToggleGroup.Select_Toggle(this);
        }
        
    }
}
