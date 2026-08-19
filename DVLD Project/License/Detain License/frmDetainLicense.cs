using DVLD.Classes;
using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using DVLD_Project.Properties;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmDetainLicense : Form
    {
        private int _DetainID = -1;
        private int _SelectedLicenseID = -1;

        public frmDetainLicense(int LicenseID)
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(825, 755);

        }



        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
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
            if (ctrFilterLicenseInfo1.SelectedLicenseInfo.IsDetained())
            {
                MessageBox.Show("Selected License i already detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tbFees.Focus();
            bunifuButton1.Enabled = true;

        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if((ctrFilterLicenseInfo1.SelectedLicenseInfo.DriverInfo.PersonID > 0))
            {
                frmShowLicenseHistory frm =
              new frmShowLicenseHistory(ctrFilterLicenseInfo1.SelectedLicenseInfo.DriverInfo.PersonID);
                frm.ShowDialog();
            }
            

        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.UserName;

        }



        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicense frm =
                       new frmShowLicense(_SelectedLicenseID);
            frm.ShowDialog();
        }


        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            _DetainID = ctrFilterLicenseInfo1.SelectedLicenseInfo.Detain(Convert.ToSingle(tbFees.Text), clsGlobal.CurrentUser.UserID);
            if (_DetainID == -1)
            {
                MessageBox.Show("Failed to Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblDetainID.Text = _DetainID.ToString();
            MessageBox.Show("License Detained Successfully with ID=" + _DetainID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            bunifuButton1.Enabled = false;
            ctrFilterLicenseInfo1.FilterEnabled = false;
            tbFees.Enabled = false;
            linkLabel1.Enabled = true;
        }
        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(tbFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(tbFees, null);

            }
            ;


            if (!clsValidatoin.IsNumber(tbFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(tbFees, null);
            }
            ;
        }
    }
}
