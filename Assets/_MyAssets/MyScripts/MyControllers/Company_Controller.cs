using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supabase.Interfaces;
using static Supabase.Realtime.PostgresChanges.PostgresChangesOptions;
using UnityEngine;
using static Doozy.Runtime.UIManager.Nodes.BackButtonNode;

namespace com.mkadmi
{
    public class Company_Controller
    {
        static Company_Controller _instance;

        public static Company_Controller Instance()
        {
            if (_instance == null)
            {
                _instance = new Company_Controller();
            }
            return _instance;
        }

        public async Task<Company_Model> GetMissionById(long id)
        {
            var response = await SB_Client.Instance()
                .From<Company_Model>()
                .Where(company => company.Id == id)
                .Get();

            return response.Model;
        }

        public async Task GetAllCompanies()
        {
            Debug.Log("Trying to get all companies and their missions");
            try
            {
            var response = await SB_Client.Instance().From<Company_Model>()
                                 .Select("*, mission_live!inner(*)")
                                 .Get();

                Debug.Log(response.Content);
            var companies = Company_Model.FromJson(response.Content);

                foreach (var missionLive in companies.LiveMissions)
                {
                    Debug.Log($"Company Name: {companies.CompanyName}");
                    Debug.Log($"Mission Live: {missionLive.Title}");
                }
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }

    }
}
