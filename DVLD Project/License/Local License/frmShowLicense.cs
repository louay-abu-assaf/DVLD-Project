using DVLD_Buisness_Tier;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmShowLicense : Form
    {
        private int LicenseID;




        public frmShowLicense(int LicenseID)
        {
            InitializeComponent();
            this.LicenseID = LicenseID;
        }


        private void LoadLicenseInfo()
        {
            ctrLicenseInfo1.LoadInfo(LicenseID);

        }


        private void frmShowLicense_Load(object sender, EventArgs e)
        {
            LoadLicenseInfo();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
