using DVLD_Buisness_Tier;
using Guna.UI2.WinForms;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using static DVLD_Project.frmShowLicense;

namespace DVLD_Project
{
    public partial class ctrFilterLicenseInfo : UserControl
    {
        clsLicense _License;
    

        public event Action<int> OnFindlicenseComplete;


        public ctrFilterLicenseInfo()
        {
            InitializeComponent();
        }


        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                groupBox1.Enabled = _FilterEnabled;
            }
        }

        private int _LicenseID = -1;

        public int LicenseID
        {
            get { return ctrLicenseInfo1.LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        { get { return ctrLicenseInfo1.SelectedLicenseInfo; } }



        public void LoadLicenseInfo(int LicenseID)
        {


            tbSearch.Text = LicenseID.ToString();
            ctrLicenseInfo1.LoadInfo(LicenseID);
            _LicenseID = ctrLicenseInfo1.LicenseID;
            if (OnFindlicenseComplete != null && FilterEnabled)
                // Raise the event with a parameter
                OnFindlicenseComplete(_LicenseID);


        }


        protected virtual void CompleteFindLicense(int LicenseID)
        {
            Action<int> Handler = OnFindlicenseComplete;
            if (Handler != null)
            {
                Handler(LicenseID);
            }
        }

        public void RaiseOnFindLicenseComplete(int licenseID)
        {
            OnFindlicenseComplete?.Invoke(licenseID);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some filed are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbSearch.Focus();
                return;

            }
            _LicenseID = int.Parse(tbSearch.Text);
            LoadLicenseInfo(_LicenseID);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ctrLicenseInfo1_Load(object sender, EventArgs e)
        {

        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbSearch.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbSearch, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(tbSearch, null);
            }
        }
    }
}
