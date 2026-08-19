using DVLD_Buisness_Tier;
using System;
using System.Data;

public class clsInternationalLicense : clsApplications
{
    public clsDriver DriverInfo;
    public int InternationalLicenseID { set; get; }
    public int DriverID { set; get; }
    public int IssuedUsingLocalLicenseID { set; get; }
    public DateTime IssueDate { set; get; }
    public DateTime ExpirationDate { set; get; }
    public bool IsActive { set; get; }

    public enum enMode { AddNew = 0, Update = 1 };
    public enMode Mode = enMode.AddNew;

    public clsInternationalLicense()
    {
        this.ApplicationTypeID = (int)clsApplications.enApplicationType.NewInternationalLicense;

        this.InternationalLicenseID = -1;
        this.DriverID = -1;
        this.IssuedUsingLocalLicenseID = -1;
        this.IssueDate = DateTime.Now;
        this.ExpirationDate = DateTime.Now;

        this.IsActive = true;


        Mode = enMode.AddNew;
    }
    public clsInternationalLicense(int ApplicationID, int ApplicantPersonID,
                DateTime ApplicationDate,
                 enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
                 float PaidFees, int CreatedByUserID,
                 int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
                DateTime IssueDate, DateTime ExpirationDate, bool IsActive)

    {
        //this is for the base clase
        base.ApplicationID = ApplicationID;
        base.ApplicationPersonID = ApplicantPersonID;
        base.ApplicationDate = ApplicationDate;
        base.ApplicationTypeID = (int)clsApplications.enApplicationType.NewInternationalLicense;
        base.ApplicationStatus = ApplicationStatus;
        base.LastStatusDate = LastStatusDate;
        base.PaidFee = PaidFees;
        base.CreatedByUserID = CreatedByUserID;

        this.InternationalLicenseID = InternationalLicenseID;
        this.ApplicationID = ApplicationID;
        this.DriverID = DriverID;
        this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
        this.IssueDate = IssueDate;
        this.ExpirationDate = ExpirationDate;
        this.IsActive = IsActive;
        this.CreatedByUserID = CreatedByUserID;

        this.DriverInfo = clsDriver.Find(this.DriverID);

        Mode = enMode.Update;
    }


    public static DataTable GetAllInternationalLicenses()
    {
        return InternationalLicenseData.GetAllInternationalLicenses();
    }


    public static clsInternationalLicense Find(int InternationalLicenseID)
    {
        int ApplicationID = -1;
        int DriverID = -1; int IssuedUsingLocalLicenseID = -1;
        DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
        bool IsActive = true; int CreatedByUserID = 1;

        if (InternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID, ref ApplicationID, ref DriverID,
            ref IssuedUsingLocalLicenseID,
        ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
        {
            //now we find the base application
            clsApplications Application = clsApplications.Find(ApplicationID);



            return new clsInternationalLicense(Application.ApplicationID,
                Application.ApplicationPersonID,
                                 Application.ApplicationDate,
                                (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate,
                                 Application.PaidFee, Application.CreatedByUserID,
                                 InternationalLicenseID, DriverID, IssuedUsingLocalLicenseID,
                                     IssueDate, ExpirationDate, IsActive);

        }

        else
            return null;

    }

    public static DataTable GetAllLicensesByDriverID(int DirverID)
    {
        return InternationalLicenseData.GetAllInternationlLicensesByDriverID(DirverID);
    }


    //public static clsInternationalLicense FindByLocalLicenseID(int localLicenseID)
    //{
    //    int internationalLicenseID = -1, applicationID = -1, driverID = -1;
    //    DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
    //    bool isActive = false;
    //    int createdByUserID = -1;

    //    if (InternationalLicenseData.FindByLocalLicenseID(ref internationalLicenseID, ref applicationID, ref driverID, localLicenseID, ref issueDate, ref expirationDate, ref isActive, ref createdByUserID))
    //    {
    //        return new clsInternationalLicense(internationalLicenseID, applicationID, driverID, localLicenseID, issueDate, expirationDate, isActive, createdByUserID);
    //    }
    //    else
    //    {
    //        return null;
    //    }

    //}


    public bool Add()
    {
        this.InternationalLicenseID = InternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);

        return (this.InternationalLicenseID > 0);
    }

    public bool Update()
    {
        return InternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
    }

    public bool Delete()
    {
        return InternationalLicenseData.DeleteInternationalLicense(this.InternationalLicenseID);
    }

    public static DataTable Search(string SearchValue, string SearchType)
    {
        return InternationalLicenseData.Search(SearchValue, SearchType);
    }


    public bool Save()
    {

        base.Mode = (clsApplications.enMode)Mode;
        if (!base.Save())
            return false;


        switch (Mode)
        {
            case enMode.AddNew:
                if (Add())
                {

                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:

                return Update();
        }

        return false;
    }

    public static DataTable GetDriverInternationalLicenses(int DriverID)
    {
        return InternationalLicenseData.GetDriverInternationalLicenses(DriverID);
    }

    public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
    {
        return InternationalLicenseData.GetLastActiveInternationalLicenseByDriverID(DriverID);
    }
}