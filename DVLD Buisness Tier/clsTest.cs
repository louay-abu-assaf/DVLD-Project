using DVLD_DataAccess_Tier;

namespace DVLD_Buisness_Tier
{
    public class clsTest
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enum enTestType {VisionTest = 1 , WrittenTest = 2,StreetTest = 3};
        public enMode Mode;

        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Note { get; set; }
        public int CreatedByUserID { get; set; }
        public clsTestAppointment TestAppointmentInfo { get; set; }


        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Note = string.Empty;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        public clsTest(int TestID, int TestAppointmentID, bool TestResult, string Note, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestResult = TestResult;
            this.Note = Note;
            this.CreatedByUserID = CreatedByUserID;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);

            Mode = enMode.Update;
        }

        public bool AddNewTest()
        {
            this.TestID = TestData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Note, this.CreatedByUserID);
            if (this.TestID > 0)
            {
                clsTestAppointment.LockedTestAppointment(this.TestAppointmentID);
            }
            return (this.TestID > 0);
        }

        public bool UpdateTest()
        {
            return TestData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Note, this.CreatedByUserID);
        }

        public static clsTest Find(int TestAppointmentID)
        {

            int TestID = -1, CreatedByUserID = -1;
            bool TestResult = false;
            string Note = string.Empty;


            if (TestData.GetTestInfoByTestAppointmentID(ref TestID, TestAppointmentID, ref TestResult, ref Note, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Note, CreatedByUserID);
            }
            else
            {
                return null;
            }

        }

        public static clsTest FindLastTestPerPersonAndLicenseClass
           (int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {
            int TestID = -1;
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = ""; int CreatedByUserID = -1;

            if (TestData.GetLastTestByPersonAndTestTypeAndLicenseClass
                (PersonID, LicenseClassID, (int)TestTypeID, ref TestID,
            ref TestAppointmentID, ref TestResult,
            ref Notes, ref CreatedByUserID))

                return new clsTest(TestID,
                        TestAppointmentID, TestResult,
                        Notes, CreatedByUserID);
            else
                return null;

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewTest())
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

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return TestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
           
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }



    }
}
