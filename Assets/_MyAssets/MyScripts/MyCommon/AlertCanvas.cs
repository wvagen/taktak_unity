using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace com.mkadmi
{
    public class AlertCanvas : MonoBehaviour
    {
        [SerializeField]
        private UIContainer loadingPanel;

        static AlertCanvas _instance = null;

        public static AlertCanvas Instance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AlertCanvas>();
            }

            return _instance;
        }

        public void Loading(bool isShowing)
        {
            if (isShowing) {
                loadingPanel.Show();
            }
            else
            {
                loadingPanel.Hide();
            }

        }
    }
}
