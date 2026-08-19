using DVLD_DataAccess_Tier;
using System;
using System.Data;

namespace DVLD_Buisness_Tier
{
    public class clsPerson
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }
        }
        public DateTime Date { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CountryID { get; set; }
        public string ImagePath { get; set; }

        public clsCountry CountryInfo;

        public clsPerson()
        {
            PersonID = -1;
            NationalNo = null;
            FirstName = null;
            SecondName = null;
            ThirdName = null;
            LastName = null;
            Date = DateTime.Now;
            Gender = -1;
            Address = null;
            Phone = null;
            Email = null;
            CountryID = -1;
            ImagePath = null;


            Mode = enMode.AddNew;
        }

        public clsPerson(int personID, string nationalNo, string firstName, string secondName, string thirdName, string lastName, DateTime date, int gender, string address, string phone, string email, int countryID, string imagePath)
        {
            PersonID = personID;
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            Date = date;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            CountryID = countryID;
            ImagePath = imagePath;
            this.CountryInfo = clsCountry.Find(countryID);

            Mode = enMode.Update;
        }



        public static DataTable GetAllPeople()
        {
            return PersonData.GetPeople();
        }

        //public static int AddNewPerson(string NationalNo,string FirstName,string SecondName,string thirdName,string LastName,DateTime Date,int Gender,string Address ,string Phone , string Email,int CountryID,string ImagePath)
        //{
        //    return PersonData.AddNewPerson(NationalNo, FirstName, SecondName, thirdName, LastName, Date, Gender, Address, Phone, Email, CountryID, ImagePath);
        //}

        public static bool DeletePerson(int PersonID)
        {
            return PersonData.DeletePerson(PersonID);
        }

        public bool UpdatePersonInfo()
        {
            return PersonData.UpdatePersonInfo(this.PersonID, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.Date, this.Gender, this.Address, this.Phone, this.Email, this.CountryID, this.ImagePath);
        }


        //public static bool UpdatePersonInfo(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName, DateTime Date, int Gender, string Address, string Phone, string Email, int CountryID,string ImagePath)
        //{
        //    return PersonData.UpdatePersonInfo(PersonID, FirstName, SecondName, ThirdName, LastName, Date, Gender, Address, Phone, Email, CountryID, ImagePath);
        //}


        public static bool IsNationalNumberExist(string NationalNo)
        {
            return PersonData.IsNationalNoExist(NationalNo);
        }

        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            int Gender = 0;
            int CountryID = -1;

            DateTime Date = DateTime.Now;


            if (PersonData.GetPersonInfoByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref Date, ref Gender, ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, Date, Gender, Address, Phone, Email, CountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static clsPerson Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int PersonID = -1, NationalityCountryID = -1;
            int Gender = 0;

            bool IsFound = PersonData.GetPersonInfoByNationalNo
                                (
                                    NationalNo, ref PersonID, ref FirstName, ref SecondName,
                                    ref ThirdName, ref LastName, ref DateOfBirth,
                                    ref Gender, ref Address, ref Phone, ref Email,
                                    ref NationalityCountryID, ref ImagePath
                                );

            if (IsFound)

                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                          NationalNo, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            else
                return null;
        }

        public static clsPerson Find(string FirstName, string SecondName, string ThirdName, string LastName)
        {
            int PersonID = -1;
            string NationalNo = "", Address = "", Phone = "", Email = "", ImagePath = "";
            int Gender = -1, CountryID = -1;

            DateTime Date = DateTime.Now;
            if (PersonData.GetPersonInfoByName(ref PersonID, FirstName, SecondName, ThirdName, LastName, ref NationalNo, ref Date, ref Gender, ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, Date, Gender, Address, Phone, Email, CountryID, ImagePath);
            }
            else
            {
                return new clsPerson();
            }
        }

        public static int GetPeopleCount()
        {
            return PersonData.GetPeopleCount();
        }

        public static DataTable SearchPeople(string SearchType, string SearchValue)
        {
            return PersonData.SearchPeople(SearchType, SearchValue);
        }

        public static DataTable SearchPerson(string SearchType, string SearchValue)
        {
            return PersonData.SearchPerson(SearchType, SearchValue);
        }

        private bool _AddNewPerson()
        {
            this.PersonID = PersonData.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.Date, this.Gender, this.Address, this.Phone, this.Email, this.CountryID, this.ImagePath);

            return (this.PersonID > 0);
        }

        private bool _UpdatePersonInfo()
        {
            return PersonData.UpdatePersonInfo(this.PersonID, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.Date, this.Gender, this.Address, this.Phone, this.Email, this.CountryID, this.ImagePath);
        }

        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePersonInfo();
            }

            return false;
        }


        public static bool isPersonExist(int ID)
        {
            return PersonData.IsPersonExist(ID);
        }

        public static bool isPersonExist(string NationlNo)
        {
            return PersonData.IsPersonExist(NationlNo);
        }

    }
}
