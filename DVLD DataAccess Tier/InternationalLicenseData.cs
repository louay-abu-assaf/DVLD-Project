using DVLD_DataAccess_Tier;
using System;
using System.Data;
using System.Data.SqlClient;

public class InternationalLicenseData
{


    public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID,
          ref int ApplicationID,
          ref int DriverID, ref int IssuedUsingLocalLicenseID,
          ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
    {
        bool isFound = false;

        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

        SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {

                // The record was found
                isFound = true;
                ApplicationID = (int)reader["ApplicationID"];
                DriverID = (int)reader["DriverID"];
                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                IssueDate = (DateTime)reader["IssueDate"];
                ExpirationDate = (DateTime)reader["ExpirationDate"];


                IsActive = (bool)reader["IsActive"];
                CreatedByUserID = (int)reader["DriverID"];


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



    static public int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
        DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
    {
        int result = -1;
        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string Query = @"update InternationalLicenses set isActive = 0 where DriverID = @DriverID;
                        
                        INSERT INTO InternationalLicenses 
                        (ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID) 
                        VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);
                                  SELECT SCOPE_IDENTITY();";

        SqlCommand command = new SqlCommand(Query, connection);

        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        command.Parameters.AddWithValue("@DriverID", DriverID);
        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
        command.Parameters.AddWithValue("@IssueDate", IssueDate);
        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
        command.Parameters.AddWithValue("@IsActive", IsActive);
        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

        try
        {
            connection.Open();
            result = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return result;
    }

    static public bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
        DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
    {
        int result = -1;
        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string Query = @"UPDATE InternationalLicenses SET 
                        ApplicationID = @ApplicationID,
                        DriverID = @DriverID,
                        IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                        IssueDate = @IssueDate,
                        ExpirationDate = @ExpirationDate,
                        IsActive = @IsActive,
                        CreatedByUserID = @CreatedByUserID
                        WHERE InternationalLicenseID = @InternationalLicenseID";

        SqlCommand command = new SqlCommand(Query, connection);

        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        command.Parameters.AddWithValue("@DriverID", DriverID);
        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
        command.Parameters.AddWithValue("@IssueDate", IssueDate);
        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
        command.Parameters.AddWithValue("@IsActive", IsActive);
        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

        try
        {
            connection.Open();
            result = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return (result > 0);
    }

    static public bool DeleteInternationalLicense(int InternationalLicenseID)
    {
        int result = -1;
        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string Query = "DELETE FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

        SqlCommand command = new SqlCommand(Query, connection);

        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

        try
        {
            connection.Open();
            result = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return (result > 0);
    }

    static public bool FindByDriverID(ref int InternationalLicenseID, ref int ApplicationID, int DriverID, ref int IssuedUsingLocalLicenseID,
        ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
    {
        bool isFound = false;
        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string Query = "SELECT * FROM InternationalLicenses WHERE DriverID = @DriverID";

        SqlCommand command = new SqlCommand(Query, connection);
        command.Parameters.AddWithValue("@DriverID", DriverID);

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                InternationalLicenseID = (int)reader["InternationalLicenseID"];
                ApplicationID = (int)reader["ApplicationID"];
                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                IssueDate = (DateTime)reader["IssueDate"];
                ExpirationDate = (DateTime)reader["ExpirationDate"];
                IsActive = (bool)reader["IsActive"];
                CreatedByUserID = (int)reader["CreatedByUserID"];

                isFound = true;
            }
            reader.Close();
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

    static public bool FindByLocalLicenseID(ref int InternationalLicenseID, ref int ApplicationID, ref int DriverID, int IssuedUsingLocalLicenseID,
          ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
    {
        bool isFound = false;
        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string Query = "SELECT * FROM InternationalLicenses WHERE IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID";

        SqlCommand command = new SqlCommand(Query, connection);
        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                InternationalLicenseID = (int)reader["InternationalLicenseID"];
                ApplicationID = (int)reader["ApplicationID"];
                DriverID = (int)reader["DriverID"];
                IssueDate = (DateTime)reader["IssueDate"];
                ExpirationDate = (DateTime)reader["ExpirationDate"];
                IsActive = (bool)reader["IsActive"];
                CreatedByUserID = (int)reader["CreatedByUserID"];

                isFound = true;
            }
            reader.Close();
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


    //static public bool FindInternationaLicneseByID( int InternationalLicenseID, ref int ApplicationID, ref int DriverID, int IssuedUsingLocalLicenseID,
    //  ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
    //{
    //    bool isFound = false;
    //    SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

    //    string Query = "SELECT * FROM InternationalLicenses WHERE IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID";

    //    SqlCommand command = new SqlCommand(Query, connection);
    //    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);

    //    try
    //    {
    //        connection.Open();
    //        SqlDataReader reader = command.ExecuteReader();
    //        if (reader.Read())
    //        {
    //            InternationalLicenseID = (int)reader["InternationalLicenseID"];
    //            ApplicationID = (int)reader["ApplicationID"];
    //            DriverID = (int)reader["DriverID"];
    //            IssueDate = (DateTime)reader["IssueDate"];
    //            ExpirationDate = (DateTime)reader["ExpirationDate"];
    //            IsActive = (bool)reader["IsActive"];
    //            CreatedByUserID = (int)reader["CreatedByUserID"];

    //            isFound = true;
    //        }
    //        reader.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.Message);
    //        isFound = false;
    //    }
    //    finally
    //    {
    //        connection.Close();
    //    }

    //    return isFound;

    //}


    static public DataTable GetAllInternationalLicenses()
    {
        DataTable dt = new DataTable();
        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string Query = "SELECT * FROM InternationalLicenses";

        SqlCommand command = new SqlCommand(Query, connection);

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

    static public DataTable GetAllInternationlLicensesByDriverID(int DriverID)
    {
        DataTable dt = new DataTable();
        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string Query = "SELECT * FROM InternationalLicenses where DriverID = @DriverID";

        SqlCommand command = new SqlCommand(Query, connection);

        command.Parameters.AddWithValue("DriverID", DriverID);

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


    public static DataTable Search(string searchValue, string searchType)
    {
        DataTable dt = new DataTable();
        string columnName = "";

        if (searchType == "Is Active" && (searchValue == "" || searchValue == "All"))
        {
            return GetAllInternationalLicenses(); // دالة تعرض كل البيانات
        }

        // تحويل النص الظاهر إلى اسم العمود الحقيقي
        switch (searchType)
        {
            case "L D L AppID":
                columnName = "IssuedUsingLocalLicenseID";
                break;

            case "I D L AppID":
                columnName = "InternationalLicenseID";
                break;

            case "Driver ID":
                columnName = "DriverID";
                break;

            case "Is Active":
                columnName = "IsActive";
                break;

            default:
                return dt; // لا يوجد نوع بحث معروف
        }

        string query = $"SELECT * FROM InternationalLicenses WHERE {columnName} ";
        using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
        using (SqlCommand command = new SqlCommand())
        {
            command.Connection = connection;

            // فلترة حسب نوع العمود
            if (columnName == "IsActive")
            {
                query += "= @value";
                command.CommandText = query;

                if (searchValue == "Yes" || searchValue == "1")
                    command.Parameters.AddWithValue("@value", 1);
                else if (searchValue == "No" || searchValue == "0")
                    command.Parameters.AddWithValue("@value", 0);
                else
                    return dt;
            }
            else if (columnName == "InternationalLicenseID" || columnName == "IssuedUsingLocalLicenseID" || columnName == "DriverID")
            {
                query += "= @value";
                command.CommandText = query;

                if (int.TryParse(searchValue, out int intVal))
                    command.Parameters.AddWithValue("@value", intVal);
                else
                    return dt;
            }
            else
            {
                query += "LIKE @value";
                command.CommandText = query;
                command.Parameters.AddWithValue("@value", "%" + searchValue.Trim() + "%");
            }

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Search Error: " + ex.Message);
            }
        }

        return dt;
    }

    public static DataTable GetDriverInternationalLicenses(int DriverID)
    {

        DataTable dt = new DataTable();
        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string query = @"
            SELECT    InternationalLicenseID, ApplicationID,
		                IssuedUsingLocalLicenseID , IssueDate, 
                        ExpirationDate, IsActive
		    from InternationalLicenses where DriverID=@DriverID
                order by ExpirationDate desc";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@DriverID", DriverID);

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
            // Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return dt;

    }
    
    public static int GetLastActiveInternationalLicenseByDriverID(int DriverID)
    {

        int result = -1;
        SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

        string Query = "SELECT top 1 InternationalLicenseID FROM InternationalLicenses where DriverID = @DriverID and GetDate() between IssueDate and ExpirationDate order by ExpirationDate desc; ";

        SqlCommand command = new SqlCommand(Query, connection);
        command.Parameters.AddWithValue("@DriverID", DriverID);

        try
        {
            connection.Open();

            object r = command.ExecuteScalar();

            if (r != null)
            {
                result = Convert.ToInt32(r);
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return result;

    }


}