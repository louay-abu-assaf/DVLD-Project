using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmReleaseDetainedLicense : Form
    {
        private int _SelectedLicenseID = -1;

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(825, 755);
        }
        
        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
            _SelectedLicenseID = LicenseID;

            ctrFilterLicenseInfo1.LoadLicenseInfo(_SelectedLicenseID);
            ctrFilterLicenseInfo1.FilterEnabled = false;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {

        }

        private void ctrFilterLicenseInfo1_OnFindlicenseComplete(int obj)
        {
            _SelectedLicenseID = obj;

            lblLicenseID.Text = _SelectedLicenseID.ToString();

            linkLabel1.Enabled = (_SelectedLicenseID != -1);

            if (_SelectedLicenseID == -1)

            {
                return;
            }

            //ToDo: make sure the license is not detained already.
            if (!ctrFilterLicenseInfo1.SelectedLicenseInfo.IsDetained())
            {
                MessageBox.Show("Selected License i is not detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicense).ApplicationFee.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

            lblDetainID.Text = ctrFilterLicenseInfo1.SelectedLicenseInfo.DetainedLicenseInfo.DetainID.ToString();
            lblLicenseID.Text = ctrFilterLicenseInfo1.SelectedLicenseInfo.LicenseID.ToString();

            lblCreatedByUser.Text = ctrFilterLicenseInfo1.SelectedLicenseInfo.DetainedLicenseInfo.CreatedByUserInfo.UserName;
            lblDetainDate.Text = ctrFilterLicenseInfo1.SelectedLicenseInfo.DetainedLicenseInfo.DetainDate.ToShortDateString();
            lblFineFees.Text = ctrFilterLicenseInfo1.SelectedLicenseInfo.DetainedLicenseInfo.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblFineFees.Text)).ToString();

            bunifuButton1.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseHistory frm =
            new frmShowLicenseHistory(ctrFilterLicenseInfo1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicense frm = new frmShowLicense(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this detained  license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int ApplicationID = -1;


            bool IsReleased = ctrFilterLicenseInfo1.SelectedLicenseInfo.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserID, ref ApplicationID); ;

            lblApplicationID.Text = ApplicationID.ToString();

            if (!IsReleased)
            {
                MessageBox.Show("Faild to to release the Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Detained License released Successfully ", "Detained License Released", MessageBoxButtons.OK, MessageBoxIcon.Information);

            bunifuButton1.Enabled = false;
            ctrFilterLicenseInfo1.FilterEnabled = false;
            linkLabel1.Enabled = true;
        }
    }
}
