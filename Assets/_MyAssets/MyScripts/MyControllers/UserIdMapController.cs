using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mkadmi
{
    public class UserIdMapController
    {
        static UserIdMapController _instance;

        public static UserIdMapController Instance()
        {
            if (_instance == null)
            {
                _instance = new UserIdMapController();
            }
            return _instance;
        }

        public async Task<List<UserIdMap>> GetAllUserIdMaps()
        {
            var response = await SB_Client.Instance()
                .From<UserIdMap>()
                .Select("*")
                .Get();

            return response.Models;
        }

        public async Task<UserIdMap> GetUserIdMapById(long id)
        {
            var response = await SB_Client.Instance()
                .From<UserIdMap>()
                .Where(user => user.Id == id)
                .Select("*")
                .Get();

            return response.Model;
        }

        public async Task<UserIdMap> CreateUserIdMap(UserIdMap userIdMap)
        {
            var response = await SB_Client.Instance()
                .From<UserIdMap>()
                .Insert(userIdMap);

            return response.Model;
        }

        public async Task<UserIdMap> UpdateUserIdMap(UserIdMap userIdMap)
        {
            var model = await SB_Client.Instance()
                .From<UserIdMap>()
                .Where(user => user.Id == userIdMap.Id)
                .Single();

            model = userIdMap;
            var response = await model.Update<UserIdMap>();

            return response.Model;
        }
    }
}