using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess_Tier
{
    public class LicenseClassData
    {
        static public DataTable GetAllLicenseInfo()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from LicenseClasses";

            SqlCommand cmd = new SqlCommand(Query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

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

        static public float GetLicenseClassFee(string LicenseClassName)
        {
            float fee = 0;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select ClassFees from LicenseClasses where ClassName = @ClassName";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ClassName", LicenseClassName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    fee = Convert.ToSingle(reader["ClassFees"]);
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
            return fee;
        }

        static public bool GetLicenseClassinfoByName(ref int LicenseClassID, string LicenseClassName, ref string ClassDescription, ref int MinimumAllowedAge, ref int DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from LicenseClasses where ClassName = @ClassName";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ClassName", LicenseClassName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    LicenseClassID = (int)reader["LicenseClassID"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = Convert.ToInt32(reader["MinimumAllowedAge"]);
                    DefaultValidityLength = Convert.ToInt32(reader["DefaultValidityLength"]);
                    ClassFees = Convert.ToSingle(reader["ClassFees"]);

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


        static public bool GetLicenseClassinfoByID(int LicenseClassID, ref string LicenseClassName, ref string ClassDescription, ref int MinimumAllowedAge, ref int DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from LicenseClasses where LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    LicenseClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = Convert.ToInt32(reader["MinimumAllowedAge"]);
                    DefaultValidityLength = Convert.ToInt32(reader["DefaultValidityLength"]);
                    ClassFees = Convert.ToSingle(reader["ClassFees"]);

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

    }
}
