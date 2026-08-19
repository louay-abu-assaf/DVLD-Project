using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess_Tier
{
    public class ApplicationTypeData
    {
        static public DataTable GetAllApplicationType()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string query = "select * from ApplicationTypes ";

            SqlCommand command = new SqlCommand(query, connection);

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


        static public bool GetApplicationTypeInfoByID(int id, ref string Title, ref float fee)
        {

            bool IsFound = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from ApplicationTypes where ApplicationTypeID = @ID ";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ID", id);


            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Title = (string)reader["ApplicationTypeTitle"];
                    fee = Convert.ToSingle(reader["ApplicationFees"]);

                    IsFound = true;

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

            return IsFound;
        }


        static public bool GetApplicationTypeInfoByName(ref int id, string Title, ref float fee)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);
            string Query = "select * from ApplicationTypes where ApplicationTypes.ApplicationTypeTitle = @ApplicationTypeName";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeName", Title);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    id = (int)reader["ApplicationTypeID"];
                    fee = Convert.ToSingle(reader["ApplicationFees"]);
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


        static public bool UpdateApplicationType(int id, string Title, float fee)
        {
            int Result = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "Update ApplicationTypes set ApplicationTypeTitle = @Title , ApplicationFees = @ApplicationFees where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", id);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@ApplicationFees", fee);

            try
            {
                connection.Open();
                Result = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return (Result > 0);


        }
    }
}
