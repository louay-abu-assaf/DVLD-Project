using DVLD_Buisness_Tier;
using Sunny.UI;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class ctrlLocalDrivingLicenseApplicationInfo : UserControl
    {
        public clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplication;

        public int _LocalDrivingLicenseApplicationID = -1;

        private int _LicenseID = -1;

        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }

        public ctrlLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationByLocalDrivingAppID(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplications.Find(LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicenseApplication == null )
            {
                _ResetLocalDrivingLicenseApplicationInfo();


                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _FillLocalDrivingLicenseApplicationInfo();
        }


        public void LoadApplicationByAppID(int ApplicationID)
        {
            _LocalDrivingLicenseApplication  = clsLocalDrivingLicenseApplications.FindByApplicationID(ApplicationID);
            if( _LocalDrivingLicenseApplication == null )
            {
                _ResetLocalDrivingLicenseApplicationInfo();


                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillLocalDrivingLicenseApplicationInfo();
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            linkLabel2.Enabled = (_LicenseID != -1);
            lblLDAPPID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseID.ToString();
            lblLicenseType.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName.ToString();
            lblPassedTest.Text = _LocalDrivingLicenseApplication.GetPassedTestCount().ToString() + "/3";
            ctrlBasicApplicationInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID);
        }


        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseApplicationID = -1;
            ctrlBasicApplicationInfo1.ResetApplicationInfo();
            lblLDAPPID.Text = "[????]";
            lblLicenseType.Text = "[????]";


        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           

           
            frmShowPersonInfo frm = new frmShowPersonInfo(_LocalDrivingLicenseApplication.ApplicationPersonID);
            frm.ShowDialog();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
