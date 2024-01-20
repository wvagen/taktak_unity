using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mkadmi
{
    public class MissionLive_Controller
    {
        static MissionLive_Controller _instance;

        public static MissionLive_Controller Instance()
        {
            if (_instance == null)
            {
                _instance = new MissionLive_Controller();
            }
            return _instance;
        }


        public async Task<List<MissionLive_Model>> GetAllMissions()
        {
            var response = await SB_Client.Instance()
                .From<MissionLive_Model>()
                .Select("*")
                .Get();

            return response.Models;
        }

        public async Task<MissionLive_Model> GetMissionById(long id)
        {
            var response = await SB_Client.Instance()
                .From<MissionLive_Model>()
                .Where(mission => mission.Id == id)
                .Get();

            return response.Model;
        }

        public async Task<MissionLive_Model> CreateMission(MissionLive_Model mission)
        {
            var response = await SB_Client.Instance()
                .From<MissionLive_Model>()
                .Insert(mission);

            return response.Model;
        }

        public async Task<MissionLive_Model> UpdateMission(MissionLive_Model mission)
        {
            var model = await SB_Client.Instance()
                .From<MissionLive_Model>()
                .Where(u => u.Id == mission.Id)
                .Single();

            model = mission;
            var response = await model.Update<MissionLive_Model>();

            return response.Model;
        }
    }
}
