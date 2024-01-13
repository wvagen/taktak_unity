using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace com.mkadmi {
    public class AuthManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _emailField;
        [SerializeField]
        private TMP_InputField _passField;

        public async void SignIn_Email()
        {
            AlertCanvas.Instance().Loading(true);
            Supabase.Gotrue.Session session = await SB_Client.Instance().Auth.SignIn(_emailField.text, _passField.text);
            Debug.Log(session.User.Id);
            UserIdMap_Model userMap = await UserIdMap_Controller.Instance().GetUserByUserCredential(session.User.Email);
            User_Model user = await UserController.Instance().GetUserById(session.User.Id);
            Debug.Log(user.ToJson());
            User.Instance().SetMe(user);

            AlertCanvas.Instance().Loading(false);
        }

    }
}
