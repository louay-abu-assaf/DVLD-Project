using DVLD_Buisness_Tier;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmShowApplicationInfo : Form
    {

        private int _DiAppID;
        clsApplications _Applications;



        public frmShowApplicationInfo(int DiAppID)
        {
            InitializeComponent();
            _DiAppID = DiAppID;
        }

        private void LoadApplicationInfo()
        {
            _Applications = clsApplications.FindWithLocalDrivingLicense(_DiAppID);

            if (_Applications != null)
            {

                ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationByLocalDrivingAppID(_DiAppID);
            }


        }

       

        private void LoadInfo()
        {

 
            LoadApplicationInfo();

        }



        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowApplicationInfo_Load(object sender, EventArgs e)
        {
            LoadInfo();

        }
    }
}
