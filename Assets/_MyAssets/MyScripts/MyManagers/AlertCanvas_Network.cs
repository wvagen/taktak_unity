using Doozy.Runtime.UIManager.Containers;
using Supabase.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Supabase.Realtime.PostgresChanges.PostgresChangesOptions;

namespace com.mkadmi
{
    public class AlertCanvas_Network : MonoBehaviour
    {
        [SerializeField]
        private UIContainer _NoConnectionPanel;
        [SerializeField]
        private UIContainer _UpdateRequiredPanel;

        public static bool isConnected = true;

        static bool isNotConnectedPanelDisplayedOnceAtLeast = false;
        static bool neverShowAgainThePanel = false;

        bool isNotConnectedPanelDisplayed = false;

        float timer = 0;
        float nextTargetTimer = 0;

        private void Start()
        {
            Updates_Controller.Instance().GetAndSubscribeToUpdate(Update_Handler);
        }

        public void UpdateBtn()
        {
            Application.OpenURL(UserSettings.UPDATE_LINK);
        }

        // Update is called once per frame
        void Update()
        {
            Connection_Manager();
        }

        void Connection_Manager()
        {
            if (!isConnected && isNotConnectedPanelDisplayed && isNotConnectedPanelDisplayedOnceAtLeast && !neverShowAgainThePanel)
            {
                isNotConnectedPanelDisplayed = false;
                _NoConnectionPanel.Show();
            }

            Check_Connection();
        }

        void Update_Handler(Updates_Model updateModel)
        {
            if (updateModel.IOS_Version > UserSettings.APP_VERSION)
            {
                _UpdateRequiredPanel.Show();
            }
        }

        void Check_Connection()
        {
            timer += Time.deltaTime;

            if (timer >= nextTargetTimer)
            {
                nextTargetTimer += UserSettings.PING_RATE;
                Try_To_Connect(false);
            }
        }

        public void Try_To_Connect(bool canDisplayDisconnectedPanel)
        {
            if (canDisplayDisconnectedPanel)
            {
                isNotConnectedPanelDisplayedOnceAtLeast = false;
                nextTargetTimer += UserSettings.PING_RATE;
                _NoConnectionPanel.Hide();
            }
            Ping();
        }

        void Ping()
        {

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                isConnected = false;
                isNotConnectedPanelDisplayed = true;
                if (!isNotConnectedPanelDisplayedOnceAtLeast)
                {
                    isNotConnectedPanelDisplayedOnceAtLeast = true;
                }
                _NoConnectionPanel.Show();
            }
            else
            {
                isNotConnectedPanelDisplayed = false;
                isNotConnectedPanelDisplayedOnceAtLeast = false;
                isConnected = true;
                neverShowAgainThePanel = false;
                _NoConnectionPanel.Hide();

            }
        }


    }
}