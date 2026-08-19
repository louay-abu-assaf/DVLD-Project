using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess_Tier
{
    public class UserData
    {

        public static DataTable GetAllUsers()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from UsersInfo";

            SqlCommand command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dataTable.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dataTable;
        }

        public static bool GetUserInfoByID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from Users where UserID =  @ID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                }

                IsFound = true;

                reader.Close();

            }
            catch (Exception ex)
            {
                IsFound = false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }


        public static bool GetUserInfoByUserName(ref int UserID, ref int PersonID, string UserName, ref string Password, ref bool IsActive)
        {

            bool IsFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);


            string Query = "select * from Users where UserName =  @UserName";


            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    UserID = (int)reader["UserID"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                    IsFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool GetUserInfoByPersonID(int PersonID, ref int UserID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);
            string query = "SELECT * FROM Users WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool GetUserFullInfo(ref int UserID, string UserName, ref int PersonID, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from Users where UserName = @UserName ";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                }
                IsFound = true;

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }



        public static DataTable SearchUser(string SerachType, string SearchValue)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "";

            switch (SerachType)
            {
                case "User ID":
                    Query = "select * from UsersInfo where UserID like @UserID";
                    break;
                case "Person ID":
                    Query = " select * from UsersInfo where PersonID like @PersonID";
                    break;
                case "User Name":
                    Query = "select * from UsersInfo where UserName like @UserName";
                    break;
                case "Full Name":
                    Query = "select * from UsersInfo where FullName like @FullName";
                    break;
                case "is Active":

                    if (SearchValue == "All")
                    {
                        Query = "select * from UsersInfo ";
                    }
                    else
                    {
                        Query = "select * from UsersInfo where IsActive = @IsActive";
                    }

                    break;
                default:
                    return null;



            }

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserID", "%" + SearchValue.Trim() + "%");
            command.Parameters.AddWithValue("@PersonID", "%" + SearchValue.Trim() + "%");
            command.Parameters.AddWithValue("@UserName", "%" + SearchValue.Trim() + "%");
            command.Parameters.AddWithValue("@FullName", "%" + SearchValue.Trim() + "%");
            command.Parameters.AddWithValue("@IsActive", SearchValue.Trim());

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;


        }

        public static int AddNewUser(int PersonID, string UserName, string Password, byte isActive)
        {
            int UserID = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = @"insert into Users (personID,UserName,Password,isActive) values (@PersonID,@UserName,@Password,@IsActive);
                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", isActive);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

            return UserID;
        }

        public static bool UpdateUserInfo(int ID, string UserName, string Password, int IsActive)
        {
            int rowEfficted = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "update Users set UserName = @UserName , Password = @Password , isActive = @isActive where UserID = @UserID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserID", ID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@isActive", IsActive);


            try
            {
                connection.Open();

                rowEfficted = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return (rowEfficted > 0);
        }



        public static bool DeleteUserInfo(int UserID)
        {
            int rowEfficted = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = @"delete from Users where UserID = @UserID;";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                rowEfficted = command.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return (rowEfficted > 0);

        }

        public static bool CheckLogin(string username, string password)
        {
            bool IsLogin = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select isFound = 1 from Users where UserName = @UserName and Password = @Password ";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("UserName", username);
            command.Parameters.AddWithValue("Password", password);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    IsLogin = true;
                }
            }
            catch (Exception ex)
            {
                IsLogin = false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return IsLogin;
        }




        public static bool IsUserNameTaken(string username)
        {
            bool isExist = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select isFound = 1 from Users where UserName = @UserName ";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserName", username);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isExist = true;
                }

                reader.Close();
            }
            catch (Exception e)
            {
                isExist = false;
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return isExist;
        }

        public static bool isUserExist(int PersonID)
        {
            bool isFound = false;
            string Query = "select isFound = 1 from Users where  Users.PersonID = @PersonID;";

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int rowEfficted = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "update Users set   Password = @NewPassword  where UserID = @UserID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@NewPassword", NewPassword);


            try
            {
                connection.Open();

                rowEfficted = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return (rowEfficted > 0);
        }


        public static bool GetUserInfoByUsernameAndPassword(string UserName, string Password,
           ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string query = "SELECT * FROM Users WHERE Username = @Username and Password=@Password;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Username", UserName);
            command.Parameters.AddWithValue("@Password", Password);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];


                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}
