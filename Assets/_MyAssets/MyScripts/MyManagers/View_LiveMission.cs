using System.Collections;
using System.Collections.Generic;
using com.mkadmi;
using Doozy.Runtime.Nody.Nodes.System;
using UnityEngine;

namespace com.mkadmi
{
    public class View_LiveMission : MonoBehaviour
    {
        [SerializeField]
        private GameObject _MissionGO;
        [SerializeField]
        private Transform _MissionLocation;

        // Start is called before the first frame update
        void Start()
        {
            SpawnMissions();
            MissionLive_Controller.Instance().SubscribeToInsert(CallBackMission);
        }

        async void SpawnMissions()
        {
            List<MissionLive_Model> missions = new List<MissionLive_Model>();
            missions = await MissionLive_Controller.Instance().GetAllMissions();

            for (int i = 0; i < missions.Count; i++)
            {
                MissionLive_Widget missionLive = Instantiate(_MissionGO, _MissionLocation).GetComponent<MissionLive_Widget>();
                missionLive.Set_Mission_Props(missions[i]);
            }
        }

        void CallBackMission(MissionLive_Model newMission)
        {
            Debug.Log("New Mission Generated!");
            MissionLive_Widget missionLive = Instantiate(_MissionGO, _MissionLocation).GetComponent<MissionLive_Widget>();
            missionLive.Set_Mission_Props(newMission);
        }
    }
}