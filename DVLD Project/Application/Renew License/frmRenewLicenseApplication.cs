using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmRenewLicenseApplication : Form
    {

        private int _NewLicenseID = -1;

        public frmRenewLicenseApplication()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(825, 900);
        }

        private void PerpareApplicationInfo()
        {

            DateTime CurrentDate = DateTime.Now;
           



            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = lblAppDate.Text;

            lblExpirationDate.Text = "???";
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplications.enApplicationType.RenewDrivingLicense).ApplicationFee.ToString();
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

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicense frm =
           new frmShowLicense(_NewLicenseID);
            frm.ShowDialog();
        }

        private void ctrFilterLicenseInfo1_OnFindlicenseComplete(int obj)
        {
            int SelectedLicenseID = obj;

            lblOldLicenseID.Text = SelectedLicenseID.ToString();

            linkLabel1.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)

            {
                return;
            }

            int DefaultValidityLength = ctrFilterLicenseInfo1.SelectedLicenseInfo.LicenseClassInfo.DefaultValidityLength;
            lblExpirationDate.Text =DateTime.Now.AddYears(DefaultValidityLength).ToShortDateString();
            lblLicenseFees.Text = ctrFilterLicenseInfo1.SelectedLicenseInfo.LicenseClassInfo.ClassFee.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();
            tbNotes.Text = ctrFilterLicenseInfo1.SelectedLicenseInfo.Notes;


            //check the license is not Expired.
            if (!ctrFilterLicenseInfo1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is not yet expiared, it will expire on: " + ctrFilterLicenseInfo1.SelectedLicenseInfo.ExpirationDate.ToShortDateString()
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bunifuButton1.Enabled = false;
                return;
            }

            //check the license is not Expired.
            if (!ctrFilterLicenseInfo1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bunifuButton1.Enabled = false;
                return;
            }



            bunifuButton1.Enabled = true;
        }

        private void frmRenewLicenseApplication_Load(object sender, EventArgs e)
        {
            PerpareApplicationInfo();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            clsLicense NewLicense =
                ctrFilterLicenseInfo1.SelectedLicenseInfo.RenewLicense(tbNotes.Text.Trim(),
                clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Renew the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblRLApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblRenewlicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("Licensed Renewed Successfully with ID=" + _NewLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            bunifuButton1.Enabled = false;
            ctrFilterLicenseInfo1.FilterEnabled = false;
            linkLabel2.Enabled = true;



        }
    }
}
