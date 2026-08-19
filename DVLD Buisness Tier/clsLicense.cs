using DVLD_DataAccess_Tier;
using System;
using System.Data;

namespace DVLD_Buisness_Tier
{
    public class clsLicense
    {
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float PaidFees { get; set; }
        public bool IsActive { get; set; }
        public clsLicense.enIssueReason IssueReason { get; set; }
        public int CreatedByUserID { get; set; }
        public clsDriver DriverInfo { get; set; }
        public clsDetainLicense DetainedLicenseInfo { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }

        public clsLicense()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClass = -1;
            IssueDate = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            Notes = string.Empty;
            IsActive = false;
            IssueReason = clsLicense.enIssueReason.FirstTime;
            CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }



        public clsLicense(int licenseID, int applicationID, int driverID, int licenseClass, DateTime issueDate, DateTime expirationDate, string notes, float paidFees, bool isActive, clsLicense.enIssueReason issueReason, int createdByUserID)
        {
            LicenseID = licenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            LicenseClass = licenseClass;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserID = createdByUserID;
            this.DriverInfo = clsDriver.Find(driverID);
            this.DetainedLicenseInfo = clsDetainLicense.Find(licenseID);
            this.LicenseClassInfo = clsLicenseClass.Find(this.LicenseClass);

            Mode = enMode.Update;
        }

        static public clsLicense Find(int LicenseID)
        {

            int driverID = -1, licenseClass = -1, issueReason = -1, createdByUserID = -1, ApplicaionID = -1;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            string notes = string.Empty;
            float paidFees = -1;
            bool isActive = false;

            if (LicenseData.GetLicenseInfoByID(LicenseID, ref ApplicaionID, ref driverID, ref licenseClass, ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))
            {
                return new clsLicense(LicenseID, ApplicaionID, driverID, licenseClass, issueDate, expirationDate, notes, paidFees, isActive,(clsLicense.enIssueReason)issueReason, createdByUserID);
            }
            else
            {
                return null;
            }

        }

        public bool IsDetained()
        {
            return clsDetainLicense.isDetained(this.LicenseID);
        }


        static public clsLicense Find(int DriverID, int LicenseClass)
        {

            int LicenseID = -1, issueReason = -1, createdByUserID = -1, ApplicaionID = -1;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            string notes = string.Empty;
            float paidFees = -1;
            bool isActive = false;

            if (LicenseData.GetLicenseInfoByDriverIDAndLicenseClass(ref LicenseID, ref ApplicaionID, DriverID, LicenseClass, ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))
            {
                return new clsLicense(LicenseID, ApplicaionID, DriverID, LicenseClass, issueDate, expirationDate, notes, paidFees, isActive, (clsLicense.enIssueReason)issueReason, createdByUserID);
            }
            else
            {
                return null;
            }

        }



        static public DataTable GetAllLocalLicenseForDriverByDriverID(int DriverID)
        {
            return LicenseData.GetAllLocalLicenseForDriverByDriverID(DriverID);

        }


        private bool AddNewLicense()
        {
            this.LicenseID = LicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (int)this.IssueReason, this.CreatedByUserID);

            return (this.LicenseID > 0);
        }

        private bool UpdateLicense()
        {
            return LicenseData.UpdateLicenseInfo(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClass, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive,(int) this.IssueReason, this.CreatedByUserID);
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {

            return LicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);

        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return UpdateLicense();
            }

            return false;
        }

        public Boolean IsLicenseExpired()
        {

            return (this.ExpirationDate < DateTime.Now);

        }


        public bool DeactivateCurrentLicense()
        {
            return (LicenseData.DeactivateLicense(this.LicenseID));
        }

        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }

        public int Detain(float FineFees, int CreatedByUserID)
        {
            clsDetainLicense detainedLicense = new clsDetainLicense();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToSingle(FineFees);
            detainedLicense.CreatedByUserID = CreatedByUserID;

            if (!detainedLicense.Save())
            {

                return -1;
            }

            return detainedLicense.DetainID;

        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, ref int ApplicationID)
        {

            //First Create Applicaiton 
            clsApplications Application = new clsApplications();

            Application.ApplicationPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicense;
            Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFee = clsApplicationType.Find((int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicense).ApplicationFee;
            Application.CreatedByUserID = ReleasedByUserID;

            if (!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = Application.ApplicationID;


            return this.DetainedLicenseInfo.ReleaseDetainedLicense(ReleasedByUserID, Application.ApplicationID);

        }

        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {

            //First Create Applicaiton 
            clsApplications Application = new clsApplications();

            Application.ApplicationPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplications.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFee = clsApplicationType.Find((int)clsApplications.enApplicationType.RenewDrivingLicense).ApplicationFee;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;

            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFee;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;


            if (!NewLicense.Save())
            {
                return null;
            }

            //we need to deactivate the old License.
            DeactivateCurrentLicense();

            return NewLicense;
        }

        public clsLicense Replace(enIssueReason IssueReason, int CreatedByUserID)
        {


            //First Create Applicaiton 
            clsApplications Application = new clsApplications();

            Application.ApplicationPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;

            Application.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ?
                (int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense :
                (int)clsApplications.enApplicationType.ReplaceLostDrivingLicense;

            Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFee = clsApplicationType.Find(Application.ApplicationTypeID).ApplicationFee;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = 0;// no fees for the license because it's a replacement.
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;



            if (!NewLicense.Save())
            {
                return null;
            }

            //we need to deactivate the old License.
            DeactivateCurrentLicense();

            return NewLicense;
        }




    }


}
