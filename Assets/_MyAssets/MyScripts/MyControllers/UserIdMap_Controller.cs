using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mkadmi
{
    public class UserIdMap_Controller
    {
        static UserIdMap_Controller _instance;

        public static UserIdMap_Controller Instance()
        {
            if (_instance == null)
            {
                _instance = new UserIdMap_Controller();
            }
            return _instance;
        }

        public async Task<List<UserIdMap_Model>> GetAllUserIdMaps()
        {
            var response = await SB_Client.Instance()
                .From<UserIdMap_Model>()
                .Select("*")
                .Get();

            return response.Models;
        }

        public async Task<UserIdMap_Model> GetUserIdMapById(long id)
        {
            var response = await SB_Client.Instance()
                .From<UserIdMap_Model>()
                .Where(user => user.Id == id)
                .Select("*")
                .Get();

            return response.Model;
        }

        public async Task<UserIdMap_Model> CreateUserIdMap(UserIdMap_Model userIdMap)
        {
            var response = await SB_Client.Instance()
                .From<UserIdMap_Model>()
                .Insert(userIdMap);

            return response.Model;
        }

        public async Task<UserIdMap_Model> UpdateUserIdMap(UserIdMap_Model userIdMap)
        {
            var model = await SB_Client.Instance()
                .From<UserIdMap_Model>()
                .Where(user => user.Id == userIdMap.Id)
                .Single();

            model = userIdMap;
            var response = await model.Update<UserIdMap_Model>();

            return response.Model;
        }
    }
}