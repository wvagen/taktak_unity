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

        int appCloudedVersion = 0;
        float timer = 0;
        float nextTargetTimer = 0;
        Updates_Model model;

        public async void SubToEvent()
        {
            Debug.Log("Subscribing ...");
            await SB_Client.Instance().From<Updates_Model>().On(ListenType.Updates, (sender, change) =>
            {
                model = change.Model<Updates_Model>();
                Updates_Model oldModel = change.OldModel<Updates_Model>();

                Debug.LogFormat("Old Model {0}: New Model {1}",oldModel.IOS_Version, model.IOS_Version);
            });
        }

        public void GetValue()
        {
            Debug.Log("Value " + model.IOS_Version);
        }



        // Update is called once per frame
        void Update()
        {
            if (!isConnected && isNotConnectedPanelDisplayed && isNotConnectedPanelDisplayedOnceAtLeast && !neverShowAgainThePanel)
            {
                isNotConnectedPanelDisplayed = false;
                _NoConnectionPanel.Show();
            }

            Check_Connection();
        }

        void Update_Handler()
        {
            if (PlayerPrefs.GetInt(UserSettings.CURRENT_APP_VERSION_KEY, 1) > UserSettings.APP_VERSION_UPDATE)
            {
                _UpdateRequiredPanel.Show();
            }
            else
            {
                //Check_For_Update();
            }
        }


        //void Check_For_Update()
        //{
        //    FirebaseDatabase.DefaultInstance
        // .GetReference(Constants.APP_VERSION)
        // .GetValueAsync().ContinueWith(task =>
        // {
        //     if (task.IsFaulted)
        //     {
        //         isTaskFailed = true;
        //     }
        //     else if (task.IsCompleted)
        //     {
        //         DataSnapshot snapshot = task.Result;
        //         if (Constants.APP_VERSION_UPDATE < (int.Parse(snapshot.Value.ToString())))
        //         {
        //             appCloudedVersion = int.Parse(snapshot.Value.ToString());
        //             PlayerPrefs.SetInt(Constants.CURRENT_APP_VERSION, appCloudedVersion);
        //             _UpdateRequiredPanel.Show();
        //         }
        //     }
        // });
        //}

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