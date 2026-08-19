using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using DVLD_Project.Properties;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Test.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;
        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;


        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;


        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {

                    case clsTestType.enTestType.VisionTest:
                        {
                            label1.Text = "Vision Test";
                            pbTestIcon.Image = Resources.Vision_512;
                            
                            break;
                        }

                    case clsTestType.enTestType.WrittenTest:
                        {
                            label1.Text = "Written Test";
                            pbTestIcon.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            label1.Text = "Street Test";
                            pbTestIcon.Image = Resources.driving_test_512;
                            break;


                        }
                }
            }
        }

        public void LoadInfo(int LocalDrivingLicenseApplicationID, int AppointmentID = -1)
        {
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = AppointmentID;

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplications.Find(LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bunifuButton1.Enabled = false;
                return;
            }

            if (_LocalDrivingLicenseApplication.DoesAttendTestType(TestTypeID))

                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;

              if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                lblRAppFees.Text = clsApplicationType.Find((int)clsApplications.enApplicationType.RetakeTest).ApplicationFee.ToString();
                gbRetakeTest.Enabled = true;
                label1.Text = "Schedule Retake Test";
                lblRetakeTestID.Text = "0";
            }
            else
            {
                gbRetakeTest.Enabled = false;
                label1.Text = "Schedule Test";
                lblRAppFees.Text = "0";
                lblRetakeTestID.Text = "N/A";
            }
            lblappID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseID.ToString();

            lblDClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.PersonFullName;


            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(TestTypeID).ToString();


            if (_Mode == enMode.AddNew)
            {
                lblFees.Text = clsTestType.Find(_TestTypeID).TestTypeFee.ToString();
                guna2DateTimePicker1.MinDate = DateTime.Now;
                lblRetakeTestID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }

            float fees = 0;
            float retakeFee = 0;

            float.TryParse(lblFees.Text, out fees);
            float.TryParse(lblRetakeTestID.Text, out retakeFee);

            lblTotalFees.Text = (fees + retakeFee).ToString();

            lblTotalFees.Text = (fees + retakeFee).ToString("0.00");


            if (!_HandleActiveTestAppointmentConstraint())
                return;
            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePrviousTestConstraint())
                return;

        }

        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && clsLocalDrivingLicenseApplications.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, _TestTypeID))
            {
                lblNote.Text = "Person Already have an active appointment for this test";
                bunifuButton1.Enabled = false;
                guna2DateTimePicker1.Enabled = false;
                return false;
            }

            return true;
        }
        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bunifuButton1.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointment.PaidFee.ToString();

            //we compare the current date with the appointment date to set the min date.
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                guna2DateTimePicker1.MinDate = DateTime.Now;
            else
                guna2DateTimePicker1.MinDate = _TestAppointment.AppointmentDate;

            guna2DateTimePicker1.Value = _TestAppointment.AppointmentDate;

            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRetakeTestID.Text = "0";
                lblRetakeTestID.Text = "N/A";
            }
            else
            {
                lblRAppFees.Text = _TestAppointment.RetakeTestApplicationInfo.PaidFee.ToString();
                gbRetakeTest.Enabled = true;
                label1.Text = "Schedule Retake Test";
                lblRetakeTestID.Text = _TestAppointment.RetakeTestApplicationID.ToString();

            }
            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {

            if (_TestAppointment.isLocked)
            {
                lblNote.Visible = true;
                lblNote.Text = "Person already sat for the test, appointment loacked.";
                guna2DateTimePicker1.Enabled = false;
                bunifuButton1.Enabled = false;
                return false;

            }
            else
                lblNote.Visible = false;

            return true;
        }
        private bool _HandlePrviousTestConstraint()
        {

            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    lblNote.Visible = false;

                    return true;

                case clsTestType.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblNote.Text = "Cannot Schedule, Vision Test should be passed first";
                        lblNote.Visible = true;
                        bunifuButton1.Enabled = false;
                        guna2DateTimePicker1.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblNote.Visible = false;
                        bunifuButton1.Enabled = true;
                        guna2DateTimePicker1.Enabled = true;
                    }


                    return true;

                case clsTestType.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblNote.Text = "Cannot Sechule, Written Test should be passed first";
                        lblNote.Visible = true;
                        bunifuButton1.Enabled = false;
                        guna2DateTimePicker1.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblNote.Visible = false;
                        bunifuButton1.Enabled = true;
                        guna2DateTimePicker1.Enabled = true;
                    }


                    return true;

            }
            return true;

        }
        private bool _HandleRetakeApplication()
        {
           
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
               
                clsApplications Application = new clsApplications();

                Application.ApplicationPersonID = _LocalDrivingLicenseApplication.ApplicationPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplications.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFee = clsApplicationType.Find((int)clsApplications.enApplicationType.RetakeTest).ApplicationFee;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;

            }
            return true;
        }

        private void ctrlScheduleTest_Load(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseID;
            _TestAppointment.AppointmentDate = guna2DateTimePicker1.Value;
            _TestAppointment.PaidFee = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
