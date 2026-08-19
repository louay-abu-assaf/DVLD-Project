using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess_Tier
{
    public class DetainLicenseData
    {

        public static int AddDetainLicense(int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int NewDetainLicenseID = -1;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = @"Insert into DetainedLicenses (LicenseID,DetainDate,FineFees,CreatedByUserID,IsReleased,ReleaseDate,ReleasedByUserID,ReleaseApplicationID) values (@LicenseID,@DetainDate,@FineFees,@CreatedByUserID,@IsReleased,@ReleaseDate,@ReleasedByUserID,@ReleaseApplicationID);
             SELECT SCOPE_IDENTITY();";

            SqlCommand commnad = new SqlCommand(Query, connection);
            commnad.Parameters.AddWithValue("LicenseID", LicenseID);
            commnad.Parameters.AddWithValue("DetainDate", DetainDate);
            commnad.Parameters.AddWithValue("FineFees", FineFees);
            commnad.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            commnad.Parameters.AddWithValue("IsReleased", IsReleased);
            if (ReleaseDate == DateTime.MinValue)
            {
                commnad.Parameters.AddWithValue("ReleaseDate", DBNull.Value);
            }
            else
            {
                commnad.Parameters.AddWithValue("ReleaseDate", ReleaseDate);
            }

            if (ReleasedByUserID == -1)
            {
                commnad.Parameters.AddWithValue("ReleasedByUserID", DBNull.Value);

            }
            else
            {
                commnad.Parameters.AddWithValue("ReleasedByUserID", ReleasedByUserID);
            }
            if (ReleaseApplicationID == -1)
            {
                commnad.Parameters.AddWithValue("ReleaseApplicationID", DBNull.Value);
            }
            else
            {
                commnad.Parameters.AddWithValue("ReleaseApplicationID", ReleaseApplicationID);
            }

            try
            {
                connection.Open();
                int result = Convert.ToInt32(commnad.ExecuteScalar());
                if (result > 0)
                {
                    NewDetainLicenseID = result;
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

            return NewDetainLicenseID;

        }

        public static bool UpdateDetainLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int RowsEfficted = -1;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = @"Update DetainedLicenses  set LicenseID = @LicenseID ,DetainDate = @DetainDate,FineFees = @FineFees ,CreatedByUserID = @CreatedByUserID ,IsReleased = @IsReleased,ReleaseDate = @ReleaseDate ,ReleasedByUserID = @ReleasedByUserID ,ReleaseApplicationID  = @ReleaseApplicationID where DetainID = @DetainID;";

            SqlCommand commnad = new SqlCommand(Query, connection);
            commnad.Parameters.AddWithValue("DetainID", DetainID);
            commnad.Parameters.AddWithValue("LicenseID", LicenseID);
            commnad.Parameters.AddWithValue("DetainDate", DetainDate);
            commnad.Parameters.AddWithValue("FineFees", FineFees);
            commnad.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            commnad.Parameters.AddWithValue("IsReleased", IsReleased);
            if (ReleaseDate == DateTime.MinValue)
            {
                commnad.Parameters.AddWithValue("ReleaseDate", DBNull.Value);
            }
            else
            {
                commnad.Parameters.AddWithValue("ReleaseDate", ReleaseDate);
            }

            if (ReleasedByUserID == -1)
            {
                commnad.Parameters.AddWithValue("ReleasedByUserID", DBNull.Value);

            }
            else
            {
                commnad.Parameters.AddWithValue("ReleasedByUserID", ReleasedByUserID);
            }
            if (ReleaseApplicationID == -1)
            {
                commnad.Parameters.AddWithValue("ReleaseApplicationID", DBNull.Value);
            }
            else
            {
                commnad.Parameters.AddWithValue("ReleaseApplicationID", ReleaseApplicationID);
            }

            try
            {
                connection.Open();
                int result = commnad.ExecuteNonQuery();
                if (result > 0)
                {
                    RowsEfficted = result;
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

            return RowsEfficted > 0;

        }

        public static DataTable GetAllDetainedLicense()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select * from DetainedLicenses_View ";

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


        public static bool GetDetainedLicenseByID(ref int DetainID, int LicenseID, ref DateTime DetainDate, ref float FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime? ReleaseDate, ref int ReleasedByUserID, ref int? ReleaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "SELECT TOP 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID AND IsReleased = 0 ORDER BY DetainDate DESC";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    DetainID = (int)reader["DetainID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    FineFees = Convert.ToSingle(reader["FineFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsReleased = (bool)reader["IsReleased"];
                    if (reader["ReleaseDate"] != DBNull.Value)
                    {
                        ReleaseDate = (DateTime)reader["ReleaseDate"];

                    }
                    else
                    {
                        ReleaseDate = null;
                    }

                    if (reader["ReleasedByUserID"] != DBNull.Value)
                    {
                        ReleasedByUserID = (int)reader["ReleasedByUserID"];

                    }
                    else
                    {
                        ReleasedByUserID = -1;
                    }
                    if (reader["ReleaseApplicationID"] != DBNull.Value)
                    {
                        ReleaseApplicationID = (int)reader["ReleaseApplicationID"];

                    }
                    else
                    {
                        ReleaseApplicationID = null;
                    }

                    isFound = true;
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

            return isFound;

        }

        public static bool isDetained( int LicenseID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "SELECT TOP 1 found = 1 FROM DetainedLicenses WHERE LicenseID = @LicenseID AND IsReleased = 0 ORDER BY DetainDate DESC";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);

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
            }
            finally
            {
                connection.Close();
            }

            return isFound;

        }


        public static DataTable Search(string SearchType, string SearchValue)
        {
            DataTable dt = new DataTable();

            string query = "";
            SqlCommand command = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                switch (SearchType)
                {
                    case "Detain ID":
                        query = "SELECT * FROM DetainedLicenses_View WHERE DetainID = @SearchValue";
                        command = new SqlCommand(query, connection);

                        if (int.TryParse(SearchValue, out int id))
                            command.Parameters.AddWithValue("@SearchValue", id);
                        else
                            return dt;
                        break;
                    case "National No":
                        query = "SELECT * FROM DetainedLicenses_View WHERE NationalNo LIKE @SearchValue";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@SearchValue", "%" + SearchValue.Trim() + "%");
                        break;

                    case "Full Name":
                        query = "SELECT * FROM DetainedLicenses_View WHERE FullName LIKE '%' + @SearchValue + '%'";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@SearchValue", SearchValue.Trim());
                        break;

                    case "Is Released":
                        query = "SELECT * FROM DetainedLicenses_View WHERE IsReleased = @SearchValue";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@SearchValue", SearchValue.Trim());
                        break;

                    case "Release Application ID":
                        query = "SELECT * FROM DetainedLicenses_View WHERE ReleaseApplicationID LIKE @SearchValue";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@SearchValue", "%" + SearchValue.Trim() + "%");
                        break;

                    default:
                        query = "SELECT * FROM DetainedLicenses_View";
                        command = new SqlCommand(query, connection);
                        break;
                }

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                        dt.Load(reader);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return dt;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string query = @"select IsDetained=1 
                            from detainedLicenses 
                            where 
                            LicenseID=@LicenseID 
                            and IsReleased=0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    IsDetained = Convert.ToBoolean(result);
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return IsDetained;


        }


        public static bool ReleaseDetainedLicense(int DetainID,
                 int ReleasedByUserID, int ReleaseApplicationID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate,
                              ReleaseApplicationID = @ReleaseApplicationID   
                              WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


    }
}
