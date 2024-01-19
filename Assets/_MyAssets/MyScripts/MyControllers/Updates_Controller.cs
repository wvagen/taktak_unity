using System;
using static Supabase.Realtime.PostgresChanges.PostgresChangesOptions;
using UnityEngine;

namespace com.mkadmi
{
    public class Updates_Controller
    {
        static Updates_Controller _instance;

        public static Updates_Controller Instance()
        {
            if (_instance == null)
            {
                _instance = new Updates_Controller();
            }
            return _instance;
        }

        public async void GetAndSubscribeToUpdate(Action<Updates_Model> callBack)
        {
            var response = await SB_Client.Instance()
                .From<Updates_Model>()
                .Select("*")
                .Get();

            await SB_Client.Instance().From<Updates_Model>().On(ListenType.Updates, (sender, change) =>
            {
                callBack(change.Model<Updates_Model>());
            });

            callBack(response.Model);
        }

    }
}
