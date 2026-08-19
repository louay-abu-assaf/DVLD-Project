using DVLD_DataAccess_Tier;
using System;
using System.Data;

namespace DVLD_Buisness_Tier
{
    public class clsDriver
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsPerson PersonInfo { get; set; }

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsDriver()
        {
            DriverID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;
            Mode = enMode.AddNew;
        }

        public clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            DriverID = driverID;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = createdDate;
            this.PersonInfo = clsPerson.Find(personID);
            Mode = enMode.Update;
        }


        public static DataTable GetAllDriverInfo()
        {
            return DriverData.GetAllDriverInfo();
        }

        private bool AddNewDriver()
        {
            this.DriverID = DriverData.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);

            return (this.DriverID > 0);
        }

        private bool UpdateDriverInfo()
        {
            return DriverData.UpdateDriverInfo(this.DriverID, this.PersonID, this.CreatedByUserID, this.CreatedDate);
        }




        public static clsDriver Find(int DriverID)
        {
            int personId = -1, numberOfActiveLicenses = -1, createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (DriverData.GetDriverInfoWithDriverID(DriverID, ref personId, ref createdByUserID, ref createdDate))
            {
                return new clsDriver(DriverID, personId, createdByUserID, createdDate);
            }

            else
            {
                return null;
            }
        }

        public static DataTable SearchDrivers(string SearchValue, string SearcgType)
        {
            return DriverData.SearchDrivers(SearchValue, SearcgType);
        }




        public static clsDriver FindWithPersonID(int PersonID)
        {
            int DriverID = -1, numberOfActiveLicenses = -1, createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (DriverData.GetDriverInfoWithPersonID(ref DriverID, PersonID, ref createdByUserID, ref createdDate))
            {
                return new clsDriver(DriverID, PersonID, createdByUserID, createdDate);
            }

            else
            {
                return null;
            }
        }


        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return UpdateDriverInfo();
            }

            return false;

        }

        public static DataTable GetLicenses(int DriverID)
        {
            return LicenseData.GetDriverLicenses(DriverID);
        }

        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }


    }
}
