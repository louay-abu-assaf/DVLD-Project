using DVLD_DataAccess_Tier;
using System.Data;

namespace DVLD_Buisness_Tier
{
    public class clsTestType
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public enTestType TestTypeID { get; set; }
        public string TestTypeName { get; set; }
        public string TestTypeDescription { get; set; }
        public float TestTypeFee { get; set; }

        public clsTestType() { }

        public clsTestType(enTestType testTypeID, string testTypeName, string testTypeDescription, float testTypeFee)
        {
            TestTypeID = testTypeID;
            TestTypeName = testTypeName;
            TestTypeDescription = testTypeDescription;
            TestTypeFee = testTypeFee;
        }


        public bool UpdateTestTypeInfo()
        {
            return TestTypeData.UpdateTestType((int)this.TestTypeID, this.TestTypeName, this.TestTypeDescription, this.TestTypeFee);
        }

        public static DataTable GetAllTestTypesInfo()
        {
            return TestTypeData.GetAllTestTypeInfo();
        }


        static public clsTestType Find(enTestType TestTypeID)
        {
            string TestTypeName = null, TestTypeDescription = null;
            float TestTypeFee = -1;
            if (TestTypeData.GetTestTypeInfoByID((int)TestTypeID, ref TestTypeName, ref TestTypeDescription, ref TestTypeFee))
            {
                return new clsTestType(TestTypeID, TestTypeName, TestTypeDescription, TestTypeFee);

            }
            else
            {
                return null;
            }
        }


    }
}
