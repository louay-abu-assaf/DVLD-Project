using DVLD_DataAccess_Tier;
using System;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Buisness_Tier
{
    public class clsLocalDrivingLicenseApplications : clsApplications
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LocalDrivingLicenseID { get; set; }
        public int LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }

        public string PersonFullName
        {
            get
            {
                return PersonInfo.FullName;
            }

        }

        public clsLocalDrivingLicenseApplications()
        {
            this.LocalDrivingLicenseID = -1;
            this.ApplicationID = -1;

            this.Mode = enMode.AddNew;

        }


        private clsLocalDrivingLicenseApplications(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
                    DateTime ApplicationDate, int ApplicationTypeID,
                     enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
                     float PaidFees, int CreatedByUserID, int LicenseClassID)

        {
            this.LocalDrivingLicenseID = LocalDrivingLicenseApplicationID; ;
            this.ApplicationID = ApplicationID;
            this.ApplicationPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = (int)ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFee = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
            this.PersonInfo = clsPerson.Find(ApplicantPersonID);
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.CreatedByUserInfo  = clsUser.FindByPersonID(ApplicantPersonID);

            Mode = enMode.Update;
        }

        static public clsLocalDrivingLicenseApplications Find(int LocalDrivingLicenseID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            bool IsFound = LocalDrivingLicenseApplicationsData.GetLocalDrivingLicenseApplicationsByID
                (LocalDrivingLicenseID, ref ApplicationID, ref LicenseClassID);


            if (IsFound)
            {
                //now we find the base application
                clsApplications Application = clsApplications.Find(ApplicationID);

                //we return new object of that person with the right data
                return new clsLocalDrivingLicenseApplications(
                    LocalDrivingLicenseID, Application.ApplicationID,
                    Application.ApplicationPersonID,
                                     Application.ApplicationDate, Application.ApplicationTypeID,
                                    (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate,
                                     Application.PaidFee, Application.CreatedByUserID, LicenseClassID);
            }
            else
                return null;

        }


        static public clsLocalDrivingLicenseApplications FindByApplicationID(int ApplicationID)
        {
            // 
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;

            bool IsFound = LocalDrivingLicenseApplicationsData.GetLocalDrivingLicenseApplicationsByApplicationID
                (ref LocalDrivingLicenseApplicationID,ApplicationID,ref LicenseClassID);


            if (IsFound)
            {
                //now we find the base application
                clsApplications Application = clsApplications.Find(ApplicationID);

                //we return new object of that person with the right data
                return new clsLocalDrivingLicenseApplications(
                    LocalDrivingLicenseApplicationID, Application.ApplicationID,
                    Application.ApplicationPersonID,
                                     Application.ApplicationDate, Application.ApplicationTypeID,
                                    (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate,
                                     Application.PaidFee, Application.CreatedByUserID, LicenseClassID);
            }
            else
                return null;



        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 

            this.LocalDrivingLicenseID = LocalDrivingLicenseApplicationsData.AddNewLocalDrivingLicenseApplication
                (
                this.ApplicationID, this.LicenseClassID);

            return (this.LocalDrivingLicenseID != -1);
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 

            return LocalDrivingLicenseApplicationsData.UpdateLocalDrivingLicenseApplication
                (
                this.LocalDrivingLicenseID, this.ApplicationID, this.LicenseClassID);

        }

        public bool Save()
        {

            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplications.enMode)Mode;
            if (!base.Save())
                return false;



                //After we save the main application now we save the sub application.
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewLocalDrivingLicenseApplication())
                        {

                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case enMode.Update:

                        return _UpdateLocalDrivingLicenseApplication();

                }

            return false;
        }

        static public DataTable GetAllLocalDrivingLicenseApplicationsInfo()
        {
            return LocalDrivingLicenseApplicationsData.GetAllLocalDrivingLicenseApplicationsInfo();
        }



        static public bool isLocalDrivingLicenseApplicationExist(int PersonID, int LicenseClassID)
        {
            return LocalDrivingLicenseApplicationsData.isApplicationExistForPerson(PersonID, LicenseClassID);
        }

        static public DataTable Search(string SerachValue, string SearchType)
        {
            return LocalDrivingLicenseApplicationsData.Search(SerachValue, SearchType);
        }

        static public int GetPassTestCountByID(int LocalDrivingLicenseApplicationID)
        {
            return LocalDrivingLicenseApplicationsData.GetPassTestCountByID(LocalDrivingLicenseApplicationID);
        }

        static public string GetLicenseClassNameByID(int LocalDrivingLicenseApplicationID)
        {
            return LocalDrivingLicenseApplicationsData.GetLicenseClassNameByID(LocalDrivingLicenseApplicationID);
        }

        static public bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return LocalDrivingLicenseApplicationsData.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
        }


        public bool DoesPassTestType(clsTestType.enTestType TestTypeID)
        {
            return LocalDrivingLicenseApplicationsData.DoesPassTestType(this.LocalDrivingLicenseID, (int)TestTypeID);
        }

        public bool DoesPassPreviousTest(clsTestType.enTestType CurrentTestType)
        {

            switch (CurrentTestType)
            {
                case clsTestType.enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    return true;

                case clsTestType.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.

                    return this.DoesPassTestType(clsTestType.enTestType.VisionTest);


                case clsTestType.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    return this.DoesPassTestType(clsTestType.enTestType.WrittenTest);

                default:
                    return false;
            }
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return LocalDrivingLicenseApplicationsData.DoesPassTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesAttendTestType(clsTestType.enTestType TestTypeID)

        {
            return LocalDrivingLicenseApplicationsData.DoesAttendTestType(this.LocalDrivingLicenseID, (int)TestTypeID);
        }

        public byte TotalTrialsPerTest(clsTestType.enTestType TestTypeID)
        {
            return LocalDrivingLicenseApplicationsData.TotalTrialsPerTest(this.LocalDrivingLicenseID, (int)TestTypeID);
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)

        {
            return LocalDrivingLicenseApplicationsData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static bool AttendedTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)

        {
            return LocalDrivingLicenseApplicationsData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0;
        }

        public bool AttendedTest(clsTestType.enTestType TestTypeID)

        {
            return LocalDrivingLicenseApplicationsData.TotalTrialsPerTest(this.LocalDrivingLicenseID, (int)TestTypeID) > 0;
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)

        {

            return LocalDrivingLicenseApplicationsData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool IsThereAnActiveScheduledTest(clsTestType.enTestType TestTypeID)

        {

            return LocalDrivingLicenseApplicationsData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseID, (int)TestTypeID);
        }

        public clsTest GetLastTestPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(this.ApplicationPersonID, this.LicenseClassID, TestTypeID);
        }

        public byte GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LocalDrivingLicenseID);
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public bool PassedAllTests()
        {
            return clsTest.PassedAllTests(this.LocalDrivingLicenseID);
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return clsTest.PassedAllTests(LocalDrivingLicenseApplicationID);
        }

        public int IssueLicenseForTheFirtTime(string Notes, int CreatedByUserID)
        {
            int DriverID = -1;

            clsDriver Driver = clsDriver.FindWithPersonID(this.ApplicationPersonID);

            if (Driver == null)
            {
                //we check if the driver already there for this person.
                Driver = new clsDriver();

                Driver.PersonID = this.ApplicationPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                if (Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = Driver.DriverID;
            }
            //now we diver is there, so we add new license

            clsLicense License = new clsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClass = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            License.PaidFees = this.LicenseClassInfo.ClassFee;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                //now we should set the application status to complete.
                this.SetComplete();

                return License.LicenseID;
            }

            else
                return -1;
        }

        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1);
        }

        public int GetActiveLicenseID()
        {//this will get the license id that belongs to this application
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicationPersonID, this.LicenseClassID);
        }

    }
}
