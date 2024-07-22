using System;

namespace com.mkadmi
{
    public class Auth_VM
    {
        [Serializable]
        public class SignInEmailVM
        {
            public string email;
            public string password;
        }

        [Serializable]
        public class SignUpEmailVM
        {
            public string email;
            public string password;
            public string user_name;
            public string name;
            public string surname;
            public string birthdate;
            public string oauth_id;
            public string phone_number;
            public string photo_path;
            public string gender;

        }

    }
}
