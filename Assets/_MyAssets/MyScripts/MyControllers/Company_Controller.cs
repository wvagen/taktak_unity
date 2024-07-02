using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Supabase.Realtime.PostgresChanges.PostgresChangesOptions;

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
    }
}
