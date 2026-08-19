using DVLD_DataAccess_Tier;
using System.Data;

namespace DVLD_Buisness_Tier
{
    public class clsLicenseClass
    {
        public int LicenseClassID { get; set; }
        public string ClassDescription { get; set; }
        public string ClassName { get; set; }
        public int MinimumAllowedAge { get; set; }
        public int DefaultValidityLength { get; set; }
        public float ClassFee { get; set; }

        clsLicenseClass() { }

        clsLicenseClass(int licenseClassID, string className, string classDescription, int minimumAllowedAge, int defaultValidityLength, float classFee)
        {
            LicenseClassID = licenseClassID;
            ClassDescription = classDescription;
            ClassName = className;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFee = classFee;
        }

        static public DataTable GetLicensesClassInfo()
        {
            return LicenseClassData.GetAllLicenseInfo();
        }

        static public float GetLicenseClassFee(string ClassName)
        {
            return LicenseClassData.GetLicenseClassFee(ClassName);
        }

        static public clsLicenseClass Find(string ClassName)
        {
            ClassName = ClassName.Trim();
            int DefaultValidityLength = -1, MinimumAllowedAge = -1, LicenseClassID = -1;
            string ClassDescription = "";
            float ClassFee = -1;

            if (LicenseClassData.GetLicenseClassinfoByName(ref LicenseClassID, ClassName, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFee))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFee);
            }
            else
            {
                return null;
            }

        }

        static public clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = "";
            int DefaultValidityLength = -1, MinimumAllowedAge = -1;
            string ClassDescription = "";
            float ClassFee = -1;


            if (LicenseClassData.GetLicenseClassinfoByID(LicenseClassID, ref ClassName, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFee))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFee);
            }
            else
            {
                return null;
            }

        }
    }
}
