using DVLD_DataAccess_Tier;
using System;
using System.Data;

namespace DVLD_Buisness_Tier
{

    public class clsTestAppointment
    {


        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int TestAppointmentID { get; set; }
        public clsTestType.enTestType TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFee { get; set; }
        public int CreatedByUserID { get; set; }
        public bool isLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }
        public clsApplications RetakeTestApplicationInfo { get; set; }    

        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.MinValue;
            this.PaidFee = -1;
            this.CreatedByUserID = -1;
            this.isLocked = false;
            this.RetakeTestApplicationID = -1;

            Mode = enMode.AddNew;

        }

        public clsTestAppointment(int testAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFee, int CreatedByUserID, bool isLocked, int RetakeTestApplicationID)
        {
            this.TestAppointmentID = testAppointmentID;
            this.TestTypeID = (clsTestType.enTestType) TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFee = PaidFee;
            this.CreatedByUserID = CreatedByUserID;
            this.isLocked = isLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            RetakeTestApplicationInfo = clsApplications.Find(RetakeTestApplicationID);


            Mode = enMode.Update;
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = -1, LocalDrivingLicenseApplicationID = -1, CreatedByUserID = -1;
            DateTime AppointmentDate = DateTime.MinValue;
            float PaidFee = -1;
            bool isLocked = false;
            int RetakeTestApplicationID = -1;

            if (TestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID, ref AppointmentDate, ref PaidFee, ref CreatedByUserID, ref isLocked, ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFee, CreatedByUserID, isLocked, RetakeTestApplicationID);
            }
            else
            {
                return null;
            }

        }


        public static DataTable FindWithLocalDrivingLicense(int LocalDrivingLicenseApplicationID)
        {
            return TestAppointmentData.GetTestAppointmentInfoByLocalDrivingLicenseID(LocalDrivingLicenseApplicationID);
        }

        public static DataTable FindWithDiAppIDAndTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestAppointmentData.GetTestAppointmentInfoByDiAppIDAndTestType(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public bool AddNew()
        {
            this.TestAppointmentID = TestAppointmentData.AddNewTestAppointment((int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFee, this.CreatedByUserID, this.isLocked, this.RetakeTestApplicationID);
            return (this.TestAppointmentID > 0);
        }

        public bool UpdateTest()
        {
            return TestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, (int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFee, this.CreatedByUserID, this.isLocked, this.RetakeTestApplicationID);
        }

        public void DeleteTest(int TestAppointmentID)
        {
            TestAppointmentData.DeleteTestAppointment(TestAppointmentID);
        }

        public static int GetTestTrial(int LocalDrivingLicenseID, int TestTypeID)
        {
            return TestAppointmentData.GetTestTrial(LocalDrivingLicenseID, TestTypeID);
        }

        public static bool IsHaveActiveAppointment(int TestAppointmentID)
        {
            return TestAppointmentData.IsHaveActiveAppointment(TestAppointmentID);
        }

        public static bool LockedTestAppointment(int TestAppointmentID)
        {
            return TestAppointmentData.LockedTestAppointment(TestAppointmentID);
        }

        public static bool IsPassedTest(int LocalDrivingLicenseID, int TestType)
        {
            return TestAppointmentData.IsPassedTest(LocalDrivingLicenseID, TestType);
        }

        public static bool WasLastTestSuccessful(int LocalDrivingLicenseID, int TestType)
        {
            return TestAppointmentData.WasLastTestSuccessful(LocalDrivingLicenseID, TestType);
        }

        public static int GetLastTestAppointmentID(int DrivingLicenseID, int TestTypeID)
        {
            return TestAppointmentData.GetLastTestAppointmentID(DrivingLicenseID, TestTypeID);
        }

        public static clsTestAppointment GetLastTestAppointmentInfo(int DrivingLicenseID, int TestTypeID)
        {
            clsTestAppointment clsTestAppointment;

            int TestID = GetLastTestAppointmentID(DrivingLicenseID, TestTypeID);
            if (TestID > 0)
            {
                clsTestAppointment = clsTestAppointment.Find(TestID);
                return new clsTestAppointment(clsTestAppointment.TestAppointmentID, TestTypeID, clsTestAppointment.LocalDrivingLicenseApplicationID, clsTestAppointment.AppointmentDate, clsTestAppointment.PaidFee, clsTestAppointment.CreatedByUserID, clsTestAppointment.isLocked, clsTestAppointment.RetakeTestApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static bool IsFailedTest(int TestAppointmentID)
        {
            return TestAppointmentData.IsFailedTest(TestAppointmentID);
        }

        public static int? GetFailedTestAppointmentID(int DiAppID, int TestTypeID)
        {
            return TestAppointmentData.GetFailedTestAppintmentID(DiAppID, TestTypeID);
        }

        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNew())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return UpdateTest();
            }

            return false;
        }




    }
}
