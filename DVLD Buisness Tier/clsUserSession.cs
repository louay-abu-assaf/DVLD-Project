namespace DVLD_Buisness_Tier
{
    public class clsUserSession
    {

        private static clsUserSession _instance;
        public int UserId { get; private set; }
        public int PersonID { get; private set; }
        public string UserName { get; private set; }

        private clsUserSession() { }

        public static clsUserSession GetInstance()
        {
            if (_instance == null)
            {
                _instance = new clsUserSession();
            }
            return _instance;
        }

        public void SetUserData(int userId, int personID, string userName)
        {
            UserId = userId;
            PersonID = personID;
            UserName = userName;
        }



    }
}
