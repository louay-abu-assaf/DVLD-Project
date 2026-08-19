using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using Sunny.UI;
using System;
using System.Data;
using System.Web.Hosting;
using System.Windows.Forms;


namespace DVLD_Project
{
    public partial class frmAddAndUpdateNewLocalDrivingLicenseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode _Mode = enMode.AddNew;


        clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplication;
        clsApplications _Applications;
        int _ApplicationID = -1;
        int _DiAppID = -1;
        int LocalDrivingLicenseID = -1;
        private int _SelectedPersonID = -1;
        bool ApplicationDone = false, LicenseDone = false;
        clsPerson _Person;
        clsApplicationType _ApplicationType;

        clsUserSession session;

        public frmAddAndUpdateNewLocalDrivingLicenseApplication(int DiAppID)
        {
            InitializeComponent();
            _DiAppID = DiAppID;

            if (_DiAppID == -1)
            {
                _Mode = enMode.AddNew;
                _Applications = new clsApplications();
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplications();
            }
            else
            {
                _Mode = enMode.Update;
                _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplications.Find(_DiAppID);
                _Applications = clsApplications.Find(_LocalDrivingLicenseApplication.ApplicationID);
            }


        }
        private void _FillLicenseClassesInComoboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetLicensesClassInfo();

            foreach (DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }


        private void _ResetDefualtValues()
        {
            //this will initialize the reset the default values
            _FillLicenseClassesInComoboBox();


            if (_Mode == enMode.AddNew)
            {

                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplications();
                
                

                cbLicenseClass.SelectedIndex = 2;
                lblFee.Text = clsApplicationType.Find((int)clsApplications.enApplicationType.NewDrivingLicense).ApplicationFee.ToString();
                lblDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                
                bunifuButton3.Enabled = true;


            }

        }

        private void _LoadData()
        {

            ctrSearchPersonInfo1.groupBox.Enabled = false;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplications.Find(_DiAppID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with ID = " + _DiAppID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            ctrSearchPersonInfo1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicationPersonID);
            lblID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseID.ToString();
            lblDate.Text = _LocalDrivingLicenseApplication.ApplicationDate.ToShortDateString();
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblFee.Text = _LocalDrivingLicenseApplication.PaidFee.ToString();
            lblCreatedBy.Text = clsUser.Find(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;

        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadLicenseClassInfo()
        {
            DataTable dt = clsLicenseClass.GetLicensesClassInfo();

            foreach (DataRow dr in dt.Rows)
            {
                cbLicenseClass.Items.Add(dr[1].ToString());
            }
        }

        private void LabelMode()
        {
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
            }
        }

        private void PrepareApplicationInfo()
        {

            lblDate.Text = DateTime.Now.ToShortDateString();
            session = clsUserSession.GetInstance();
            lblCreatedBy.Text = session.UserName;
            clsApplicationType Type = clsApplicationType.Find((int)clsApplications.enApplicationType.NewDrivingLicense);

            lblFee.Text = Type.ApplicationFee.ToString();


        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                bunifuButton3.Enabled = true;
                
                return;
            }


            //incase of add new mode.
            if (ctrSearchPersonInfo1.PersonId != -1)
            {

                bunifuButton3.Enabled = true;
                tabUser.SelectedIndex = 1;
              

            }

            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void frmAddAndUpdateNewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            cbLicenseClass.DropDownStyle = ComboBoxStyle.DropDownList;

            _ResetDefualtValues();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }
        }



        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            tabUser.SelectedIndex = 0;
        }



        private void AddApplication()
        {
            if (_Applications != null)
            {

            }
            else
            {

                _Applications.ApplicationPersonID = _Person.PersonID;
                _Applications.ApplicationDate = Convert.ToDateTime(lblDate.Text);
                _Applications.ApplicationTypeID = 1;
                _Applications.ApplicationStatus = clsApplications.enApplicationStatus.New;
                _Applications.LastStatusDate = Convert.ToDateTime(lblDate.Text);
                _Applications.PaidFee = lblFee.Text.ToInt();
                _Applications.CreatedByUserID = session.UserId;


            }


            if (_Applications.Save())
            {
                _Mode = enMode.Update;
                _ApplicationID = _Applications.ApplicationID;
                lblID.Text = _ApplicationID.ToString();
                LabelMode();
            }


            if (_Applications.ApplicationPersonID > 0)
            {
                ApplicationDone = true;
            }
        }

        private void AddLocalLicense(int licenseClassID)
        {

            if (_LocalDrivingLicenseApplication != null)
            {
                _LocalDrivingLicenseApplication.LicenseClassID = licenseClassID;

                if (_LocalDrivingLicenseApplication.UpdateApplication())
                {
                    MessageBox.Show("Local Driving License Updated Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            else
            {

                _LocalDrivingLicenseApplication.ApplicationID = _ApplicationID;
                _LocalDrivingLicenseApplication.LicenseClassID = licenseClassID;
                _LocalDrivingLicenseApplication.AddNewApplication();


            }

            if (_LocalDrivingLicenseApplication.LocalDrivingLicenseID > 0)
            {
                LicenseDone = true;
            }
        }

        private void AddNewLocalDrivingLicenseApplication()
        {
            int personID = _Person.PersonID;
            string LicenseClassName = cbLicenseClass.Text;

            clsLicenseClass licenseClass = clsLicenseClass.Find(LicenseClassName);


            if (licenseClass != null)
            {

                if (clsLocalDrivingLicenseApplications.isLocalDrivingLicenseApplicationExist(personID, licenseClass.LicenseClassID))
                {
                    MessageBox.Show("Choose another License Class, the Selected Person Already have an active application for the selected class  ", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                AddApplication();
                AddLocalLicense(licenseClass.LicenseClassID);
            }


        }



        private void bunifuButton3_Click_1(object sender, EventArgs e)
        {

            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;


            int ActiveApplicationID = clsApplications.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplications.enApplicationType.NewDrivingLicense, LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }


            //check if user already have issued license of the same driving  class.
            if (clsLicense.IsLicenseExistByPersonID(ctrSearchPersonInfo1.PersonId, LicenseClassID))
            {

                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalDrivingLicenseApplication.ApplicationPersonID = ctrSearchPersonInfo1.PersonId;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = 1;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplications.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFee = Convert.ToSingle(lblFee.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;
            


            if (_LocalDrivingLicenseApplication.Save())
            {
                lblID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }
    }
}
