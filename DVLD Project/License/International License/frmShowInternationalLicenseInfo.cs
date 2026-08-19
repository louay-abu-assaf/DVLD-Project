using DVLD_Buisness_Tier;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmShowInternationalLicenseInfo : Form
    {
        int _IntLicenseID = -1;
        clsInternationalLicense _InternationalLicense;

        public frmShowInternationalLicenseInfo(int internationalLicenseID)
        {
            InitializeComponent();
            _IntLicenseID = internationalLicenseID;

        }

        private void LoadIntLicenseInfo()
        {
          ctrInternationalLicense1.LoadInfo(_IntLicenseID);
        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            LoadIntLicenseInfo();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
