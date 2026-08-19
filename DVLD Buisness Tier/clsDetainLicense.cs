using DVLD_DataAccess_Tier;
using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace DVLD_Buisness_Tier
{
    public class clsDetainLicense
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public float FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { set; get; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserID { get; set; }
        public int? ReleaseApplicationID { get; set; }
        public clsUser ReleasedByUserInfo { set; get; }


        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public clsDetainLicense()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.MinValue;
            FineFees = -1;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.MinValue;
            ReleasedByUserID = -1;
            ReleaseApplicationID = -1;

            Mode = enMode.AddNew;
        }


        public clsDetainLicense(int detainID, int licenseID, DateTime detainDate, float fineFees, int createdByUserID, bool isReleased, DateTime? releaseDate, int releasedByUserID, int? releaseApplicationID)
        {
            DetainID = detainID;
            LicenseID = licenseID;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserID;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserID;
            ReleaseApplicationID = releaseApplicationID;
            this.CreatedByUserInfo = clsUser.Find(this.CreatedByUserID);


            this.ReleasedByUserInfo = clsUser.Find((int) this.ReleasedByUserID);
            

            Mode = enMode.Update;
        }

        private bool AddNewDetainLicense()
        {
            this.DetainID = DetainLicenseData.AddDetainLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID, this.IsReleased, this.ReleaseDate.Value, this.ReleasedByUserID.Value, this.ReleaseApplicationID.Value);

            return (this.DetainID > 0);
        }

        private bool UpdateDetainLicense()
        {
            return DetainLicenseData.UpdateDetainLicense(this.DetainID, this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID, this.IsReleased, this.ReleaseDate.Value, this.ReleasedByUserID.Value, this.ReleaseApplicationID.Value);
        }

        public static DataTable GetAllDetainedLicenseInfo()
        {
            return DetainLicenseData.GetAllDetainedLicense();
        }

        public static clsDetainLicense Find(int LicenseID)
        {
            int detainID = -1, createdByUserID = -1;
            DateTime dateTime = DateTime.Now;
            float fineFees = -1;
            bool isReleased = false;
            DateTime? releaseDate = DateTime.Now;
            int releasedByUserID = -1;
            int? releaseApplicationID = -1;

            if (DetainLicenseData.GetDetainedLicenseByID(ref detainID, LicenseID, ref dateTime, ref fineFees, ref createdByUserID, ref isReleased, ref releaseDate, ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainLicense(detainID, LicenseID, dateTime, fineFees, createdByUserID, isReleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            else
            {
                return null;

            }
        }

        public static bool isDetained(int LicenseID)
        {
            return DetainLicenseData.isDetained(LicenseID);
        }



        static public DataTable Search(string SearchValue, string SearchType)
        {
            return DetainLicenseData.Search(SearchType, SearchValue);
        }
        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewDetainLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return UpdateDetainLicense();
            }

            return false;
        }


        public static bool IsLicenseDetained(int LicenseID)
        {
            return DetainLicenseData.IsLicenseDetained(LicenseID);
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return DetainLicenseData.ReleaseDetainedLicense(this.DetainID,
                   ReleasedByUserID, ReleaseApplicationID);
        }


    }
}
