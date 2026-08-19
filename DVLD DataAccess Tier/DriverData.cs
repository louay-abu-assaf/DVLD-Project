using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess_Tier
{
    public class DriverData
    {
        public static DataTable GetAllDriverInfo()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from Drivers_View";

            SqlCommand command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool GetDriverInfoWithDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from Drivers where DriverID = @DriverID";



            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    DriverID = (int)reader["DriverID"];
                    PersonID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];

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



        public static bool GetDriverInfoWithPersonID(ref int DriverID, int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from Drivers where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    DriverID = (int)reader["DriverID"];
                    PersonID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];

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



        public static DataTable SearchDrivers(string SearchValue, string SearchType)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "";
            switch (SearchType)
            {
                case "Driver ID":
                    Query = "select * from Drivers_View where DriverID like @SearchValue";
                    break;
                case "Person ID":
                    Query = "select * from Drivers_View where PersonID like @SearchValue";
                    break;
                case "National No":
                    Query = "select * from Drivers_View where NationalNo  like @SearchValue";
                    break;
                case "Full Name":
                    Query = "select * from Drivers_View where FullName like @SearchValue";
                    break;

                default:
                    Query = "select * from Drivers_View ";
                    break;
            }

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@SearchValue", "%" + SearchValue.Trim() + "%");

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
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

            return dt;


        }


        public static int AddNewDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            int NewID = -1;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = @"insert into Drivers (PersonID,CreatedByUserID,CreatedDate) Values (@PersonID,@CreatedByUserID,@CreatedDate)
                SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);

            try
            {
                connection.Open();


                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    NewID = insertedID;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                connection.Close();
            }

            return NewID;

        }


        public static bool UpdateDriverInfo(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            int RowEfficted = -1;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = @"Update Drivers set(PersonID = @PersonID ,CreatedByUserID = @CreatedByUserID , CreatedDate = @CreatedDate )
                                        where DriverID = @DriverID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);



            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    RowEfficted = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return (RowEfficted > 0);
        }

        public static bool DeleteDriverInfo(int DriverID)
        {
            int RowEfficted = -1;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "Delete Drivers where DriverID = @DriverID";

            SqlCommand commnad = new SqlCommand(Query, connection);

            commnad.Parameters.AddWithValue("@DriverID,", DriverID);

            try
            {
                connection.Open();

                object result = commnad.ExecuteScalar();

                if (result != null)
                {
                    RowEfficted = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                connection.Close();
            }
            return (RowEfficted > 0);

        }


    }
}

