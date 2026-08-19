using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmIssueInternationalLicense : Form
    {
        private int _InternationalLicenseID = -1;
       

        public frmIssueInternationalLicense()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(825, 800);

        }

        private void frmInternationalLicense_Load(object sender, EventArgs e)
        {
            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = lblAppDate.Text;
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();//add one year.
            lblFees.Text = clsApplicationType.Find((int)clsApplications.enApplicationType.NewInternationalLicense).ApplicationFee.ToString();
            lblUser.Text = clsGlobal.CurrentUser.UserName;
        }



        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmShowLicenseHistory frm =
               new frmShowLicenseHistory(ctrFilterLicenseInfo1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();


        }

        private void ctrFilterLicenseInfo1_OnFindlicenseComplete(int obj)
        {
            int SelectedLicenseID = obj;

            lblLocalLicenseID.Text = SelectedLicenseID.ToString();

            linkLabel1.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)

            {
                return;
            }




            //check the license class, person could not issue international license without having
            //normal license of class 3.

            if (ctrFilterLicenseInfo1.SelectedLicenseInfo.LicenseClass != 3)
            {
                MessageBox.Show("Selected License should be Class 3, select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check if person already have an active international license
            int ActiveInternaionalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrFilterLicenseInfo1.SelectedLicenseInfo.DriverID);

            if (ActiveInternaionalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternaionalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                linkLabel1.Enabled = true;
                _InternationalLicenseID = ActiveInternaionalLicenseID;
                bunifuButton1.Enabled = false;
                return;
            }

            bunifuButton1.Enabled = true;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsInternationalLicense InternationalLicense = new clsInternationalLicense();
            //those are the information for the base application, because it inhirts from application, they are part of the sub class.
            
            InternationalLicense.ApplicationPersonID = ctrFilterLicenseInfo1.SelectedLicenseInfo.DriverInfo.PersonID;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.PaidFee = clsApplicationType.Find((int)clsApplications.enApplicationType.NewInternationalLicense).ApplicationFee;
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;


            InternationalLicense.DriverID = ctrFilterLicenseInfo1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrFilterLicenseInfo1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);

            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblILApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            lblILLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + InternationalLicense.InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            bunifuButton1.Enabled = false;
            ctrFilterLicenseInfo1.FilterEnabled = false;
            linkLabel2.Enabled = true;


        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm =
                         new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void ctrFilterLicenseInfo1_Load(object sender, EventArgs e)
        {
          

        }
    }
}
