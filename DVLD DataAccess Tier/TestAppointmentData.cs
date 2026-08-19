using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess_Tier
{
    public class TestAppointmentData
    {


        static public bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "SELECT * FROM TestAppointments where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    TestTypeID = (int)reader["TestTypeID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];
                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                    {
                        RetakeTestApplicationID = -1;
                    }
                    else
                    {
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                    }

                    isFound = true;

                }

            }
            catch (Exception ex)
            {
                isFound = false;
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();

            }

            return isFound;
        }


        static public DataTable GetTestAppointmentInfoByLocalDrivingLicenseID(int LocalDrivingLicenseApplicationID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();

                string Query = "SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked FROM TestAppointments  where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and re-throw it
                        Console.WriteLine(ex.ToString());
                        throw;
                    }
                }
            }

            return dt;
        }
        static public DataTable GetTestAppointmentInfoByDiAppIDAndTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();

                string Query = "SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked FROM TestAppointments \r\nwhere LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestAppointments.TestTypeID = @TestTypeID";

                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", @TestTypeID);

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and re-throw it
                        Console.WriteLine(ex.ToString());
                        throw;
                    }
                }
            }

            return dt;
        }
        //static public int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool isLocked)
        //{
        //    int newTestAppointmentID = 0;
        //    int? retakeTestAppId = null;

        //    using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
        //    {
        //        // أولاً: البحث عن آخر اختبار فاشل لنفس الشخص ولنفس نوع الاختبار
        //        string findFailedTestSql = @"
        //        SELECT TOP 1 TA.TestAppointmentID
        //        FROM TestAppointments TA
        //        JOIN Tests T ON TA.TestAppointmentID = T.TestAppointmentID
        //        WHERE TA.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseID 
        //          AND TA.TestTypeID = @TestTypeID 
        //          AND T.TestResult = 0
        //        ORDER BY TA.AppointmentDate DESC";
        //        using (SqlCommand findCmd = new SqlCommand(findFailedTestSql, connection))
        //        {
        //            findCmd.Parameters.AddWithValue("@LocalDrivingLicenseID", LocalDrivingLicenseApplicationID);
        //            findCmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

        //            connection.Open();
        //            object result = findCmd.ExecuteScalar();
        //            if (result != null && result != DBNull.Value)
        //                retakeTestAppId = Convert.ToInt32(result);

        //            connection.Close();
        //        }

        //        // ثانياً: إنشاء موعد الاختبار الجديد
        //        string insertSql = @"
        //         INSERT INTO TestAppointments 
        //             (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID)
        //         VALUES 
        //             (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestApplicationID);
        //         SELECT SCOPE_IDENTITY();";

        //        using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
        //        {
        //            insertCmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
        //            insertCmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
        //            insertCmd.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
        //            insertCmd.Parameters.AddWithValue("@PaidFees", PaidFees);
        //            insertCmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //            insertCmd.Parameters.AddWithValue("@IsLocked", isLocked);
        //            insertCmd.Parameters.AddWithValue("@RetakeTestApplicationID", (object)retakeTestAppId ?? DBNull.Value);

        //            connection.Open();
        //            newTestAppointmentID = Convert.ToInt32(insertCmd.ExecuteScalar());
        //            connection.Close();
        //        }
        //    }

        //    return newTestAppointmentID;
        //}


        static public int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool isLocked, int RetakeTestApplicationID)
        {
            int newTestAppointmentID = 0;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "insert into TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID,IsLocked,RetakeTestApplicationID) values (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, @CreatedByUserID,@IsLocked,@RetakeTestApplicationID); select SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", isLocked);
            if (RetakeTestApplicationID != -1)
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            }
            else
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

            }

            try
            {
                connection.Open();



                newTestAppointmentID = Convert.ToInt32(command.ExecuteScalar());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();

            }

            return newTestAppointmentID;
        }

        static public bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            bool isUpdated = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "update TestAppointments set TestTypeID = @TestTypeID, LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID, AppointmentDate = @AppointmentDate, PaidFees = @PaidFees, CreatedByUserID = @CreatedByUserID, IsLocked = @IsLocked , RetakeTestApplicationID = @RetakeTestApplicationID where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            if (RetakeTestApplicationID != -1)
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            }
            else
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

            }

            try
            {
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isUpdated = true;
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

            return isUpdated;
        }

        static public bool DeleteTestAppointment(int TestAppointmentID)
        {
            bool isDeleted = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "delete from TestAppointments where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isDeleted = true;
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

            return isDeleted;
        }


        static public int GetTestTrial(int localDrivingLicenseID, int TestTypeID)
        {
            int Result = 0;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select Count(*) from TestAppointments where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                Result = Convert.ToInt32(command.ExecuteScalar());


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }

            return Result;

        }




        static public bool IsHaveActiveAppointment(int appointmentID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "select isFound = 1 from TestAppointments where TestAppointmentID = @TestAppointmentID and IsLocked = 0";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", appointmentID);

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


        public static bool IsPassedTest(int localDrivingLicenseID, int testType)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"
          SELECT TOP 1 1 AS isFound
          FROM Tests
          INNER JOIN TestAppointments 
              ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
          INNER JOIN TestTypes
              ON TestTypes.TestTypeID = TestAppointments.TestTypeID
          WHERE 
              TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseID
              AND Tests.TestResult = 1
              AND TestTypes.TestTypeID = @TestType";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseID", localDrivingLicenseID);
                    command.Parameters.AddWithValue("@TestType", testType);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isFound = reader.HasRows;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        isFound = false;
                    }
                }
            }

            return isFound;
        }


        public static bool LockedTestAppointment(int TestAppointmentID)
        {
            int result = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString);

            string Query = "Update TestAppointments set isLocked = 1 where TestAppointmentID = @TestAppointmentID";

            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                result = cmd.ExecuteNonQuery();

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

        public static bool WasLastTestSuccessful(int localDrivingLicenseID, int testTypeID)
        {
            string query = @" SELECT isFound = 1 
                     FROM TestAppointments
                     inner join Tests
                     on TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                     WHERE TestAppointments.LocalDrivingLicenseApplicationID = @localDrivingLicenseID AND TestTypeID = @TestTypeID
                     and Tests.TestResult = 1
                     ORDER BY AppointmentDate DESC";

            using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@localDrivingLicenseID", localDrivingLicenseID);
                cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToBoolean(result); // نفترض أن TestResult هو bool: true = ناجح
                }

                return false; // لا يوجد اختبار سابق أو غير ناجح
            }
        }

        public static int GetLastTestAppointmentID(int DiAppID, int TestTypeID)
        {
            string query = @"SELECT TOP 1 TestAppointmentID
                     FROM TestAppointments
                     WHERE LocalDrivingLicenseApplicationID = @DiAppID AND TestTypeID = @TestTypeID
                     ORDER BY AppointmentDate DESC";

            using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DiAppID", DiAppID);
                cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static bool IsFailedTest(int TestAppointmentID)
        {
            string query = @"SELECT TestResult FROM Tests WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                return result != null && Convert.ToInt32(result) == 0;
            }
        }

        public static int? GetFailedTestAppintmentID(int DiAppID, int TestTypeID)
        {
            string query = @"SELECT TOP 1 TestAppointments.TestAppointmentID
    FROM TestAppointments
    INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
    WHERE Tests.TestResult = 0
      AND TestAppointments.LocalDrivingLicenseApplicationID = @DiAppID
      AND TestAppointments.TestTypeID = @TestTypeID
    ORDER BY TestAppointments.AppointmentDate DESC;
";

            using (SqlConnection conn = new SqlConnection(DataAccessSettings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DiAppID", DiAppID);
                cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                return result != null ? Convert.ToInt32(result) : (int?)null;
            }
        }




    }



}