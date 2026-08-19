using DVLD_DataAccess_Tier;
using System.Data;

namespace DVLD_Buisness_Tier
{
    public class clsApplicationType
    {

        public int ApplicationID { get; set; }
        public string ApplicationTitle { get; set; }
        public float ApplicationFee { get; set; }

        clsApplicationType()
        {
            ApplicationID = -1;
            ApplicationTitle = "";
            ApplicationFee = -1;
        }

        clsApplicationType(int applicationID, string applicationTitle, float applicationFee)
        {
            ApplicationID = applicationID;
            ApplicationTitle = applicationTitle;
            ApplicationFee = applicationFee;
        }


        static public clsApplicationType Find(int ApplicationID)
        {
            string Applicationtitle = "";
            float ApplicationFee = 0;

            if (ApplicationTypeData.GetApplicationTypeInfoByID(ApplicationID, ref Applicationtitle, ref ApplicationFee))
            {
                return new clsApplicationType(ApplicationID, Applicationtitle, ApplicationFee);
            }
            else
            {
                return null;
            }


        }


        static public clsApplicationType Find(string ApplicationTypeName)
        {
            int ApplicationID = -1;
            float ApplicationFee = 0;

            if (ApplicationTypeData.GetApplicationTypeInfoByName(ref ApplicationID, ApplicationTypeName, ref ApplicationFee))
            {
                return new clsApplicationType(ApplicationID, ApplicationTypeName, ApplicationFee);
            }
            else
            {
                return null;
            }


        }


        static public DataTable GetAllApplicationType()
        {
            return ApplicationTypeData.GetAllApplicationType();
        }

        public bool UpdateApplicationType()
        {
            return ApplicationTypeData.UpdateApplicationType(this.ApplicationID, this.ApplicationTitle, this.ApplicationFee);
        }


    }
}
