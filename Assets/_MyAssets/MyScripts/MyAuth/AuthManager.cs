using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace com.mkadmi {
    public class AuthManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _emailField;
        [SerializeField]
        private TMP_InputField _passField;

        [SerializeField]
        private TMP_InputField _signUp_emailField;
        [SerializeField]
        private TMP_InputField __signUp_passField;
        [SerializeField]
        private TMP_InputField __signUp_confirmPassField;
        [SerializeField]
        private ToggleGroup _toggleGroup;

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
            AlertCanvas.Instance().Load_Scene(UserSettings.SCENE_MAIN_MENU);
        }

        public async void SignUp_Email()
        {
            if (!CheckPassword_Correspandance()) return;
            if (!CheckPassword_Constraints()) return;

            AlertCanvas.Instance().Loading(true);
            Supabase.Gotrue.Session session = await SB_Client.Instance().Auth.SignIn(_emailField.text, _passField.text);
            Debug.Log(session.User.Id);
            UserIdMap_Model userMap = await UserIdMap_Controller.Instance().GetUserByUserCredential(session.User.Email);
            User_Model user = await UserController.Instance().GetUserById(session.User.Id);
            Debug.Log(user.ToJson());
            User.Instance().SetMe(user);
            AlertCanvas.Instance().Loading(false);
            AlertCanvas.Instance().Load_Scene(UserSettings.SCENE_MAIN_MENU);
        }

        bool CheckPassword_Correspandance()
        {
            bool isPassMatch = false;

            if (_signUp_emailField.text != __signUp_confirmPassField.text)
            {
                isPassMatch = false;
                AlertCanvas.Instance().ShowWarningPanel("Password missmatch!", "The passwords you enereted are not the same");
            }
            else
            {
                isPassMatch = true;
            }

            return isPassMatch;
        }

        bool CheckPassword_Constraints()
        {
            bool isPassConstrainted = false;


            return isPassConstrainted;
        }

    }
}
