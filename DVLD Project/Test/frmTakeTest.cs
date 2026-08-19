using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using DVLD_Project.Properties;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmTakeTest : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode _Mode = enMode.AddNew;
        private int _TestID;
        private int _DiAppID;
        private int _TestTypeID;
        clsTest _Test;
        clsApplications _Applications;
        clsTestAppointment _TestAppointment;
        clsPerson _Person;
        clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplications;
        clsTestType _TestType;


        public frmTakeTest(int TestAppointmentID, int TestTypeID)
        {
            InitializeComponent();
            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            _Test = clsTest.Find(TestAppointmentID);
            _TestTypeID = TestTypeID;

            if (_Test == null)
            {
                _Test = new clsTest();
                _TestID = -1;

            }


            if (_TestID == -1)
            {
                _Mode = enMode.AddNew;
                _Test = new clsTest();
            }
            else
            {
                _Mode = enMode.Update;
                _Test = clsTest.Find(TestAppointmentID);
            }


        }

        private void LoadTestInfo()
        {
            switch (_TestTypeID)
            {
                case 1:
                    pbTestIcon.Image = Resources.Vision_512;
                    break;
                case 2:
                    pbTestIcon.Image = Resources.Written_Test_512;
                    break;
                case 3:
                    pbTestIcon.Image = Resources.driving_test_512;
                    break;

            }
        }

        private void LoadInfo()
        {
            LoadTestInfo();
            _DiAppID = _TestAppointment.LocalDrivingLicenseApplicationID;
            _TestTypeID =(int)_TestAppointment.TestTypeID;
            _TestType = clsTestType.Find((clsTestType.enTestType)_TestTypeID);
            _Applications = clsApplications.FindWithLocalDrivingLicense(_DiAppID);
            _Person = clsPerson.Find(_Applications.ApplicationPersonID);


            lblappID.Text = _DiAppID.ToString();
            lblDClass.Text = clsLocalDrivingLicenseApplications.GetLicenseClassNameByID(_DiAppID).ToString();
            lblFees.Text = _TestType.TestTypeFee.ToString();
            lblName.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
            lblTrial.Text = clsTestAppointment.GetTestTrial(_DiAppID, _TestTypeID).ToString();
            lblDate.Text = _TestAppointment.AppointmentDate.ToShortDateString().ToString();

            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblTestID.Text = "Not Taken yet";
            }
        }

        private void GetControlInfoToSave()
        {
            bool Pass = true ? guna2RadioButton1.Checked : false;
            string Notes = textBox1.Text;

            _Test.Note = Notes;
            _Test.TestResult = Pass;
            _Test.TestAppointmentID = _TestAppointment.TestAppointmentID;
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (MessageBox.Show("Are You sure you want to save? After that you cannot change the Pass/Fail results after you Save ?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_Test.Save())
                {
                    MessageBox.Show("Test Data Saved Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblTestID.Text = _Test.TestID.ToString();
                    if (Pass)
                    {
                        if (_TestAppointment.RetakeTestApplicationID != -1)
                        {
                            clsApplications.ChangeApplicationStatus(_TestAppointment.RetakeTestApplicationID, 3);
                        }
                    }


                }
                else
                {

                    MessageBox.Show("Failed To Save Test Data", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }



        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            GetControlInfoToSave();
            bunifuButton1.Visible = false;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }
    }
}
