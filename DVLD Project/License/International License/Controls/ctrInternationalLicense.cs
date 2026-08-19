using DVLD_Project.Properties;
using Sunny.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project
{

    public partial class ctrInternationalLicense : UserControl
    {        private int _InternationalLicenseID;
        private clsInternationalLicense _InternationalLicense;


        public int InternationalLicenseID
        {
            get { return _InternationalLicenseID; }
        }

        public ctrInternationalLicense()
        {
            InitializeComponent();
        }

        private void _LoadPersonImage()
        {
            if (_InternationalLicense.DriverInfo.PersonInfo.Gender == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            string ImagePath = _InternationalLicense.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        public void LoadInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            _InternationalLicense = clsInternationalLicense.Find(_InternationalLicenseID);
            if (_InternationalLicense == null)
            {
                MessageBox.Show("Could not find International License ID = " + _InternationalLicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }

            lblIntLicense.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";
            lblLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblName.Text = _InternationalLicense.DriverInfo.PersonInfo.FullName;
            lblNationNo.Text = _InternationalLicense.DriverInfo.PersonInfo.NationalNo;
            lblGender.Text = _InternationalLicense.DriverInfo.PersonInfo.Gender == 0 ? "Male" : "Female";
            lblDateOfBirth.Text = _InternationalLicense.DriverInfo.PersonInfo.Date.ToShortDateString();

            lblDriverID.Text = _InternationalLicense.DriverID.ToString();
            lblIssuedate.Text = _InternationalLicense.IssueDate.ToShortDateString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToShortDateString();

            _LoadPersonImage();



        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
