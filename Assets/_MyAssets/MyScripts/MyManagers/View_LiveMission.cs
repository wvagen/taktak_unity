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
            //MissionLive_Controller.Instance().SubscribeToInsert(CallBackMission);
        }

        async void SpawnMissions()
        {
            List<Joint_Company_MissionLive_Model> missions = new List<Joint_Company_MissionLive_Model>();
            missions = await JointModels_Controller.Instance().GetMissionLiveWithCompanies(10,0);

            for (int i = 0; i < missions.Count; i++)
            {
                MissionLive_Widget missionLive = Instantiate(_MissionGO, _MissionLocation).GetComponent<MissionLive_Widget>();
                await missionLive.Set_MissionWithCompanyAsync(missions[i]);
            }
        }

        void CallBackMission(MissionLive_Model newMission)
        {
            Debug.Log("New Mission Generated!");
            MissionLive_Widget missionLive = Instantiate(_MissionGO, _MissionLocation).GetComponent<MissionLive_Widget>();
            missionLive.Set_Mission_PropsAsync(newMission);
        }
    }
}