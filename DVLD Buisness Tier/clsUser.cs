using DVLD_DataAccess_Tier;
using System.Data;

namespace DVLD_Buisness_Tier
{

    public class clsUser
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int UserID { get; set; }
        public string UserName { get; set; }
        public int PersonID { get; set; }
        public clsPerson PersonInfo;
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public clsUser()
        {
            this.UserID = -1;
            this.UserName = null;
            this.PersonID = -1;
            this.Password = null;
            this.IsActive = false;

            Mode = enMode.AddNew;
        }

        public clsUser(int UserID, string UserName, int PersonID, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.PersonID = PersonID;
            this.Password = Password;
            this.IsActive = IsActive;
            this.PersonInfo = clsPerson.Find(PersonID);

            Mode = enMode.Update;
        }

        public static bool IsLogin(string username, string password)
        {
            return UserData.CheckLogin(username, password);
        }

        public static DataTable GetAllUsers()
        {
            return UserData.GetAllUsers();
        }

        public static clsUser Find(string username)
        {

            int UserID = -1;
            int PersonID = -1;
            string password = null;
            bool isActive = false;


            if (UserData.GetUserInfoByUserName(ref UserID, ref PersonID, username, ref password, ref isActive))
            {
                return new clsUser(UserID, username, PersonID, password, isActive);
            }
            else
            {
                return new clsUser();
            }
        }

        public static clsUser Find(int UserID)
        {
            string UserName = null;
            int PersonID = -1;
            string password = null;
            bool isActive = false;

            if (UserData.GetUserInfoByID(UserID, ref PersonID, ref UserName, ref password, ref isActive))
            {
                return new clsUser(UserID, UserName, PersonID, password, isActive);
            }
            else
            {
                return new clsUser();
            }
        }


        public static clsUser FindByPersonID(int PersonID)
        {
            string UserName = null;
            string Password = null;
            bool IsActive = false;
            int UserID = -1;

            if (UserData.GetUserInfoByPersonID(PersonID, ref UserID, ref UserName, ref Password, ref IsActive))
            {
                return new clsUser(UserID, UserName, PersonID, Password, IsActive);
            }

            return new clsUser();
        }


        public static clsUser FindByUsernameAndPassword(string UserName, string Password)
        {
            int UserID = -1;
            int PersonID = -1;

            bool IsActive = false;

            bool IsFound = UserData.GetUserInfoByUsernameAndPassword
                                (UserName, Password, ref UserID, ref PersonID, ref IsActive);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID, UserName, PersonID, Password, IsActive);
            else
                return null;
        }

        public static DataTable SearchUser(string SearchType, string SearchValue)
        {
            return UserData.SearchUser(SearchType, SearchValue);
        }

        public static int AddNewUser(int PersonID, string UserName, string Password, byte isActive)
        {
            int UserID = -1;
            UserID = UserData.AddNewUser(PersonID, UserName, Password, isActive);

            return UserID;
        }

        public static bool isUserNameTaken(string UserName)
        {
            return UserData.IsUserNameTaken(UserName);
        }

        public static bool isUserExist(int PersonID)
        {
            return UserData.isUserExist(PersonID);
        }

        public static bool DeleteUser(int UserID)
        {
            return UserData.DeleteUserInfo(UserID);
        }

        public bool Update()
        {
            return UserData.UpdateUserInfo(this.UserID, this.UserName, this.Password, (byte)(this.IsActive ? 1 : 0));
        }
        private bool _AddNewUser()
        {
            this.UserID = UserData.AddNewUser(this.PersonID, this.UserName, this.Password, (byte)(this.IsActive ? 1 : 0));

            return (this.UserID != -1);
        }

        private bool _UpdateUserInfo()
        {
            return UserData.UpdateUserInfo(this.UserID, this.UserName, this.Password, (byte)(this.IsActive ? 1 : 0));
        }


        public bool ChangePassword(string NewPassword)
        {
            return UserData.ChangePassword(this.UserID, NewPassword);
        }


        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUserInfo();
            }

            return false;
        }


    }
}
