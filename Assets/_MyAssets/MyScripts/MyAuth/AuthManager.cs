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

        public async void SignIn()
        {
            var session = await SB_Client.Instance().Auth.SignIn(_emailField.text, _passField.text);
            Debug.Log(session);
        }

    }
}
