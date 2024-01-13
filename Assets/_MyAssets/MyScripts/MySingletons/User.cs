
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

        public void SetMe(User_Model user)
        {
            Id = user.Id;
            Number = user.Number;
            UserName = user.UserName;
            Name = user.Name;
            Surname = user.Surname;
            OauthId = user.OauthId;
            OauthType = user.OauthType;
            PhoneNumber = user.PhoneNumber;
            PhotoPath = user.PhotoPath;
            VirtualCoins = user.VirtualCoins;
            XpPoints = user.XpPoints;
            Status = user.Status;
            Rating = user.Rating;
            ReportCount = user.ReportCount;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
        }


    }
}