using DVLD_Buisness_Tier;
using DVLD_Project.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmTestAppointments : Form
    {
        private int _DiAppID;
        clsApplications _Applications;
        clsApplicationType _ApplicationType;
        clsPerson _Person;
        clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplications;
        clsUserSession _User;
        clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;

        public frmTestAppointments(int DlAppID, clsTestType.enTestType TestType)
        {

            InitializeComponent();

            _DiAppID = DlAppID;
            _TestType = TestType;
        }

        private void LoadTestAppointmentInfo()
        {
            _Applications = clsApplications.FindWithLocalDrivingLicense(_DiAppID);

            if (_Applications != null)
            { 

                ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationByAppID(_Applications.ApplicationID);


            }


        }

        private void LoadTestTypeInfo()
        {
            switch (_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    lblTestName.Text = "Vision Test Appointments ";
                    pbTestIcon.Image = Resources.Vision_512;
                    break;
                case clsTestType.enTestType.WrittenTest:
                    lblTestName.Text = "Written Test Appointments ";
                    pbTestIcon.Image = Resources.Written_Test_512;

                    break;
                case clsTestType.enTestType.StreetTest:
                    lblTestName.Text = "Practical Test Appointments ";
                    pbTestIcon.Image = Resources.driving_test_512;

                    break;
                default:
                    lblTestName.Text = "Vision Test Appointments ";
                    break;
            }


        }


        private void LoadDrivingLicenseInfo()
        {
            _LocalDrivingLicenseApplications = clsLocalDrivingLicenseApplications.Find(_DiAppID);
   
        }

        private void LoadTestAppointmentDataInTable()
        {
            DataTable dt = clsTestAppointment.FindWithDiAppIDAndTestType(_DiAppID, (int)_TestType);
            guna2DataGridView1.DataSource = dt;

            lblRecord.Text = "#Record : " + guna2DataGridView1.Rows.Count.ToString();
        }


        private void LoadInfo()
        {
            LoadTestTypeInfo();
            LoadDrivingLicenseInfo();
            LoadTestAppointmentInfo();
            LoadTestAppointmentDataInTable();

        }

        private void frmVisiontestAppointments_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplications localDrivingLicenseApplication = clsLocalDrivingLicenseApplications.Find(_DiAppID);


            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            //---
            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestType);

            if (LastTest == null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_DiAppID, _TestType);
                frm1.ShowDialog();
                LoadInfo();
                return;
            }

           
            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake failed test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest
                (LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestType);
            frm2.ShowDialog();
            LoadInfo();

        }


        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //}
            int TestAppointmentID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);

            frmScheduleTest frm = new frmScheduleTest(_DiAppID, _TestType, TestAppointmentID);
            frm.ShowDialog();

            LoadTestAppointmentDataInTable();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);
            frmTakeTest frm = new frmTakeTest(TestAppointmentID, (int)_TestType);
            frm.ShowDialog();

            LoadTestAppointmentDataInTable();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool IsLocked = Convert.ToBoolean(guna2DataGridView1.CurrentRow.Cells[3].Value);

            if (IsLocked)
            {
                takeTestToolStripMenuItem.Enabled = false;
            }
        }
    }
}
