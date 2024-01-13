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
            Supabase.Gotrue.Session session = await SB_Client.Instance().Auth.SignIn(_emailField.text, _passField.text);
            Debug.Log(session.User);
            UserIdMap_Model user = await UserIdMap_Controller.Instance().GetUserByUserCredential(session.User.Email);

            Debug.Log(user.ToJson());
        }

    }
}
