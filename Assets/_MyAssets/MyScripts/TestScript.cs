using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.mkadmi
{
    public class TestScript : MonoBehaviour
    {

        public async void FetchUsers()
        {
            List<UserIdMap> users = await UserIdMapController.Instance().GetAllUserIdMaps();

            foreach (UserIdMap user in users)
            {
                Debug.Log(user.ToJson().ToString());
            }
        }
    }
}
