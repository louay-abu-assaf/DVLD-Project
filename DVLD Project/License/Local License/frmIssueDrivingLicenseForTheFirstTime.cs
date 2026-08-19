using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmIssueDrivingLicenseForTheFirstTime : Form
    {

        private int _DiAppID;
        clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplications;


        public frmIssueDrivingLicenseForTheFirstTime(int DlAppID)
        {
            InitializeComponent();
            _DiAppID = DlAppID;

        }

        

        private void LoadInfo()
        {

            _LocalDrivingLicenseApplications = clsLocalDrivingLicenseApplications.Find(_DiAppID);   

            if(_LocalDrivingLicenseApplications == null)
            {
                MessageBox.Show("no Application with ID " + _LocalDrivingLicenseApplications.ApplicationID);
                this.Close();
                return;
            }

            if( !_LocalDrivingLicenseApplications.PassedAllTests())
            {
                MessageBox.Show("Person Should Passed All test First " + _LocalDrivingLicenseApplications.ApplicationID);
                this.Close();
                return;

            }


            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationByLocalDrivingAppID(this._DiAppID);  

        }


        private void frmIssueDrivingLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            string Note = tbNote.Text;

            int LicenseID = _LocalDrivingLicenseApplications.IssueLicenseForTheFirtTime(Note, clsGlobal.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show("License Issued Successfully with ID " + LicenseID, "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                MessageBox.Show("Error Happen " , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
    }
}
