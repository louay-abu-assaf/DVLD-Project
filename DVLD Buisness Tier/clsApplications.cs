using DVLD_DataAccess_Tier;
using System;

namespace DVLD_Buisness_Tier
{
    public class clsApplications
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };
        public enMode Mode = enMode.AddNew;


        public int ApplicationID { get; set; }
        public int ApplicationPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApplicantName
        {
            get { return clsPerson.Find(ApplicationPersonID).FullName; }
        }
        public clsApplicationType ApplicationTypeInfo;
        public int ApplicationTypeID { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public string StatusText
        {
            get
            {

                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }

        }
        public DateTime LastStatusDate { get; set; }
        public float PaidFee { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }
        public clsPerson PersonInfo { get; set; }
        public clsApplications()
        {
            Mode = enMode.AddNew;
        }



        public clsApplications(int applicationID, int applicationPersonID, DateTime applicationDate, int applicationTypeID, enApplicationStatus applicationStatus, DateTime lastStatusDate, float paidFee, int createdByUserID)
        {
            ApplicationID = applicationID;
            ApplicationPersonID = applicationPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFee = paidFee;
            CreatedByUserID = createdByUserID;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.CreatedByUserInfo = clsUser.Find(CreatedByUserID);
            this.PersonInfo = clsPerson.Find(ApplicationPersonID);

            Mode = enMode.Update;
        }

        public static clsApplications Find(int ApplicationID)
        {
            int applicationPersonID = -1, applicationTypeID = -1, applicationStatus = -1, createdByUserID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            float PaidFee = -1;



            if (ApplicationsData.GetApplicationInfoByID(ApplicationID, ref applicationPersonID, ref applicationDate, ref applicationTypeID, ref applicationStatus, ref lastStatusDate, ref PaidFee, ref createdByUserID))
            {
                return new clsApplications(ApplicationID, applicationPersonID, applicationDate, applicationTypeID, (enApplicationStatus)applicationStatus, lastStatusDate, PaidFee, createdByUserID);
            }
            else
            {
                return null;
            }

        }


        public static clsApplications FindWithLocalDrivingLicense(int DrivingLicenseApplicationID)
        {
            int ApplicationID = -1, applicationPersonID = -1, applicationTypeID = -1, applicationStatus = -1, createdByUserID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            float PaidFee = -1;



            if (ApplicationsData.GetApplicationInfoByLocalDrivingLicenseID(DrivingLicenseApplicationID, ref ApplicationID, ref applicationPersonID, ref applicationDate, ref applicationTypeID, ref applicationStatus, ref lastStatusDate, ref PaidFee, ref createdByUserID))
            {
                return new clsApplications(ApplicationID, applicationPersonID, applicationDate, applicationTypeID, (enApplicationStatus)applicationStatus, lastStatusDate, PaidFee, createdByUserID);
            }
            else
            {
                return null;
            }

        }


        public bool AddNewApplication()
        {
            this.ApplicationID = (ApplicationsData.AddNewApplication(this.ApplicationPersonID, this.ApplicationDate, this.ApplicationTypeID, (int)this.ApplicationStatus, this.LastStatusDate, this.PaidFee, this.CreatedByUserID));

            return (this.ApplicationID > 0);
        }

        public bool DeleteApplication(int ApplicationID)
        {
            return ApplicationsData.DeleteApplication(ApplicationID);
        }

        public bool UpdateApplication()
        {
            return ApplicationsData.UpdateApplication(this.ApplicationID, this.ApplicationPersonID, this.ApplicationTypeID, (int)this.ApplicationStatus, this.LastStatusDate, this.PaidFee, this.CreatedByUserID);
        }

        static public bool ChangeApplicationStatus(int ApplicationID, int Status)
        {
            return ApplicationsData.ChangeStatusApplication(ApplicationID, Status);
        }

        public bool Cancel()

        {
            return ApplicationsData.UpdateStatus(ApplicationID, 2);
        }

        public bool SetComplete()

        {
            return ApplicationsData.UpdateStatus(ApplicationID, 3);
        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            return ApplicationsData.IsApplicationExist(ApplicationID);
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return ApplicationsData.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }

        public bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
            return DoesPersonHaveActiveApplication(this.ApplicationPersonID, ApplicationTypeID);
        }

        public static int GetActiveApplicationID(int PersonID, enApplicationType ApplicationTypeID)
        {
            return ApplicationsData.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return ApplicationsData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        public int GetActiveApplicationID(enApplicationType ApplicationTypeID)
        {
            return GetActiveApplicationID(this.ApplicationPersonID, ApplicationTypeID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewApplication())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return UpdateApplication();
            }

            return false;
        }
    }

}


