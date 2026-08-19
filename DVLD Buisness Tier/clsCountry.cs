using DVLD_DataAccess_Tier;
using System.Data;

namespace DVLD_Buisness_Tier
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public clsCountry()
        {
            CountryID = -1;
            CountryName = null;
        }

        public clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }


        public static DataTable GetAllCountries()
        {
            return CountryData.GetAllCountries();
        }

        public static clsCountry Find(string countryName)
        {
            int ID = -1;

            if (CountryData.GetCountryInfoByName(ref ID, countryName))
            {
                return new clsCountry(ID, countryName);
            }
            else
            {
                return null;
            }
        }

        public static clsCountry Find(int CountryID)
        {
            string CountryName = "";

            if (CountryData.GetCountryInfoByID(CountryID, ref CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
    }
}
