
namespace com.mkadmi
{
    public class User : User_Model
    {
        static User _instance;

        public static User Instance()
        {
            if (_instance == null)
            {
                _instance = new User();
            }
            return _instance;
        }


    }
}