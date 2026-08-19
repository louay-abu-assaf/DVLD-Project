using DVLD_Buisness_Tier;
using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DVLD_Project
{
    public partial class frmManageLocalDrivingLicenseApplication : Form
    {
        clsApplications _Applications;
        clsPerson _Person;

        DataTable _dtAllLocalDrivingLicenseApplications = new DataTable();

        public frmManageLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            tbSearch.Visible = false;
        }


        private void LoadDataInfo()
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplications.GetAllLocalDrivingLicenseApplicationsInfo();
            guna2DataGridView1.DataSource = _dtAllLocalDrivingLicenseApplications;
            

            int Number = guna2DataGridView1.Rows.Count;
            lblRecord.Text = "#Record : " + Number.ToString();
        }

        private void frmManageLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {

            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplications.GetAllLocalDrivingLicenseApplicationsInfo();
            comboBox1.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            tbSearch.Visible = false;

            LoadDataInfo();

        }

        private void tbSearch_TextChange(object sender, EventArgs e)
        {
           
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "Status";
            string FilterValue = cbStatus.Text;

            if (FilterValue == "None")
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = $"[{FilterColumn}] = '{FilterValue}'";

            lblRecord.Text = _dtAllLocalDrivingLicenseApplications.DefaultView.Count.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbSearch.Visible = (comboBox1.Text != "None" && comboBox1.Text != "Status");
            cbStatus.Visible = comboBox1.Text == "Status";

            if (tbSearch.Visible)
            {
                tbSearch.Text = "";
                tbSearch.Focus();
            }

            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRecord.Text = "#Record : " + guna2DataGridView1.Rows.Count.ToString();


        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            string selected = comboBox1.SelectedItem.ToString();

            if (selected == "L D L AppID" && !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            if (selected == "Full Name" && !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmAddAndUpdateNewLocalDrivingLicenseApplication frm = new frmAddAndUpdateNewLocalDrivingLicenseApplication(-1);
            frm.ShowDialog();
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int appID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);

            if (clsApplications.ChangeApplicationStatus(appID, 2))
            {
                MessageBox.Show($"Application With ID {appID} was canceled successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to cancel the application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void OpenTestAppointment()
        {
            int LocalDrivingLicenseID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);
            int TestType = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[5].Value) + 1;
            frmTestAppointments frm = new frmTestAppointments(LocalDrivingLicenseID, (clsTestType.enTestType)TestType);

            frm.ShowDialog();
        }

        private void sechduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTestAppointment();
            LoadDataInfo();

            
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);

            if (MessageBox.Show("Are You Sure Do You Want To Delete this Local Driving License Application ", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {

                if (clsLocalDrivingLicenseApplications.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseID))
                {
                    MessageBox.Show("Local Driving License Application Deletes Successfully ", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Request To delete has been Canceled ", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)guna2DataGridView1.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplications LocalDrivingLicenseApplication =
                    clsLocalDrivingLicenseApplications.Find
                                                    (LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)guna2DataGridView1.CurrentRow.Cells[5].Value;

            bool LicenseExists = LocalDrivingLicenseApplication.IsLicenseIssued();

            //Enabled only if person passed all tests and Does not have license. 
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;

            showLicenseToolStripMenuItem.Enabled = LicenseExists;
            editApplicationToolStripMenuItem.Enabled = !LicenseExists && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enApplicationStatus.New);
            sechduleTestsToolStripMenuItem.Enabled = !LicenseExists;

            //Enable/Disable Cancel Menue Item
            //We only canel the applications with status=new.
            cancelApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enApplicationStatus.New);

            //Enable/Disable Delete Menue Item
            //We only allow delete incase the application status is new not complete or Cancelled.
            deleteApplicationToolStripMenuItem.Enabled =
                (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enApplicationStatus.New);



            //Enable Disable Schedule menue and it's sub menue
            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest); ;
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);

            sechduleTestsToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enApplicationStatus.New);

            if (sechduleTestsToolStripMenuItem.Enabled)
            {
                //To Allow Schdule vision test, Person must not passed the same test before.
                sechduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                //To Allow Schdule written test, Person must pass the vision test and must not passed the same test before.
                sechdulWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                //To Allow Schdule steet test, Person must pass the vision * written tests, and must not passed the same test before.
                sechduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;

            }
        }


        private void sechdulWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTestAppointment();
            LoadDataInfo();
        }

        private void sechduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTestAppointment();
            LoadDataInfo();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DiAppID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);
            int TestType = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[5].Value);

            frmIssueDrivingLicenseForTheFirstTime frm = new frmIssueDrivingLicenseForTheFirstTime(DiAppID);
            frm.ShowDialog();

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)guna2DataGridView1.CurrentRow.Cells[0].Value;

            int LicenseID = clsLocalDrivingLicenseApplications.Find(
                           LocalDrivingLicenseApplicationID).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                frmShowLicense frm = new frmShowLicense(LicenseID);
                frm.ShowDialog();

            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DiAppID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);
            clsLocalDrivingLicenseApplications clsLocalDriving = clsLocalDrivingLicenseApplications.Find(DiAppID);
            frmShowLicenseHistory frm = new frmShowLicenseHistory(clsLocalDriving.ApplicationPersonID);

            frm.ShowDialog();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DiAppID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);
            frmShowApplicationInfo frm = new frmShowApplicationInfo(DiAppID);
            frm.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DiAppID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);
            frmAddAndUpdateNewLocalDrivingLicenseApplication frm = new frmAddAndUpdateNewLocalDrivingLicenseApplication(DiAppID);
            frm.ShowDialog();


        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (comboBox1.Text)
            {
                case "L D L AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if(tbSearch.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecord.Text = "#Record : " + _dtAllLocalDrivingLicenseApplications.DefaultView.Count.ToString();
                return;
            }

            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                //in this case we deal with integer not string.
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, tbSearch.Text.Trim());
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, tbSearch.Text.Trim());

            lblRecord.Text = "#Record : " + _dtAllLocalDrivingLicenseApplications.DefaultView.Count.ToString();
        }
    }
}