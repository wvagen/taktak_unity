using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mkadmi
{
    public class UserController
    {
        static UserController _instance;

        public static UserController Instance()
        {
            if (_instance == null)
            {
                _instance = new UserController();
            }
            return _instance;
        }


        public async Task<List<User_Model>> GetAllUsers()
        {
            var response = await SB_Client.Instance()
                .From<User_Model>()
                .Select("*")
                .Get();

            return response.Models;
        }

        public async Task<User_Model> GetUserByUserName(string userName)
        {
            var response = await SB_Client.Instance()
                .From<User_Model>()
                .Where(user => user.UserName == userName)
                .Get();

            return response.Model;
        }

        public async Task<User_Model> GetUserById(string id)
        {
            var response = await SB_Client.Instance()
                .From<User_Model>()
                .Where(user => user.Id == id)
                .Get();

            return response.Model;
        }

        public async Task<User_Model> CreateUser(User_Model user)
        {
            var response = await SB_Client.Instance()
                .From<User_Model>()
                .Insert(user);

            return response.Model;
        }

        public async Task<User_Model> UpdateUser(User_Model user)
        {
            var model = await SB_Client.Instance()
                .From<User_Model>()
                .Where(u => u.Id == user.Id)
                .Single();

            model = user;
            var response = await model.Update<User_Model>();

            return response.Model;
        }
    }
}
