using DVLD_Buisness_Tier;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmShowLicenseHistory : Form
    {
        private int _PersonID = -1;


        public frmShowLicenseHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
           
        }


        private void frmShowLicenseHistory_Load(object sender, EventArgs e)
        {
            if (_PersonID != -1)
            {
                ctrSearchPersonInfo1.LoadPersonInfo(_PersonID);
                ctrSearchPersonInfo1.groupBox.Enabled = false;
                ctrDrivingLicenses1.LoadInfoByPersonID(_PersonID);
                
            }
            else
            {
                ctrSearchPersonInfo1.Enabled = true;
            }
        }
    }
}
