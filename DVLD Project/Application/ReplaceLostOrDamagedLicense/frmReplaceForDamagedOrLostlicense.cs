using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using System;
using System.Windows.Forms;
using static DVLD_Buisness_Tier.clsLicense;

namespace DVLD_Project
{
    public partial class frmReplaceForDamagedOrLostlicense : Form
    {


        private int _NewLicenseID = -1;

        public frmReplaceForDamagedOrLostlicense()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(820, 815);
        }

        private int _GetApplicationTypeID()
        {
            //this will decide which application type to use accirding 
            // to user selection.

            if (rbDamage.Checked)

                return (int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApplications.enApplicationType.ReplaceLostDrivingLicense;
        }

        private enIssueReason _GetIssueReason()
        {
            //this will decide which reason to issue a replacement for

            if (rbDamage.Checked)

                return enIssueReason.DamagedReplacement;
            else
                return enIssueReason.LostReplacement;
        }


        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
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

            //dont allow a replacement if is Active .
            if (!ctrFilterLicenseInfo1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bunifuButton1.Enabled = false;
                return;
            }

            bunifuButton1.Enabled = true;
        }

        private void frmReplaceForDamagedOrLostlicense_Load(object sender, EventArgs e)
        {

            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblUser.Text = clsGlobal.CurrentUser.UserName;

            rbDamage.Checked = true;

        }

        private void rbDamage_CheckedChanged(object sender, EventArgs e)
        {
            label2.Text = "Replacement for Damaged License";
            this.Text = label2.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).ApplicationFee.ToString();
        }

        private void rbLost_CheckedChanged(object sender, EventArgs e)
        {
            
            label2.Text = "Replacement for Lost License";
            this.Text = label2.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).ApplicationFee.ToString();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Issue a Replacement for the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            clsLicense NewLicense =
               ctrFilterLicenseInfo1.SelectedLicenseInfo.Replace(_GetIssueReason(),
               clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Issue a replacemnet for this  License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblRLApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;

            lblReplacedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("Licensed Replaced Successfully with ID=" + _NewLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            bunifuButton1.Enabled = false;
            
            ctrFilterLicenseInfo1.FilterEnabled = false;
            linkLabel2.Enabled = true;


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
    }
}
