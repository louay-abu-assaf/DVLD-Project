using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using DVLD_Project.User;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class MainScreen : Form
    {

        clsUserSession session = clsUserSession.GetInstance();

        public MainScreen()
        {
            InitializeComponent();

        }

        private void uSersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageUsers frm = new frmManageUsers();
            frm.ShowDialog();

        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmmanagePeople managePeople = new frmmanagePeople();
            managePeople.ShowDialog();
        }

        private void MainScreen_
            (object sender, EventArgs e)
        {

        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            LoginScreen frm = new LoginScreen();
            this.Close();
            frm.ShowDialog();
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {

        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUserInfo frm = new frmShowUserInfo(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();

        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmChangePassword frm = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void manageApplicationTyoeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationType frm = new frmManageApplicationType();

            frm.ShowDialog();

        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddAndUpdateNewLocalDrivingLicenseApplication frm = new frmAddAndUpdateNewLocalDrivingLicenseApplication(-1);
            frm.ShowDialog();
        }

        private void localDrivingApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLocalDrivingLicenseApplication frm = new frmManageLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDrivers frm = new frmManageDrivers();
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueInternationalLicense frmInternationalLicense = new frmIssueInternationalLicense();
            frmInternationalLicense.ShowDialog();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageInternationalLicenseApplication frm = new frmManageInternationalLicenseApplication();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLicenseApplication frm = new frmRenewLicenseApplication();
            frm.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLocalDrivingLicenseApplication frm = new frmManageLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplaceForDamagedOrLostlicense frm = new frmReplaceForDamagedOrLostlicense();
            frm.ShowDialog();
        }

        private void manageTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestType frm = new frmManageTestType();
            frm.ShowDialog();
        }

        private void detaineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense(-1);
            frm.ShowDialog();
        }

        private void releaseDetainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void manageDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicense frm = new frmManageDetainedLicense();
            frm.ShowDialog();
        }
    }
}
