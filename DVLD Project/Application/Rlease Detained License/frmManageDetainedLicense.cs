using DVLD_Buisness_Tier;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmManageDetainedLicense : Form
    {
        public frmManageDetainedLicense()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;


            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void LoadDataInfo()
        {
            DataTable allData = clsDetainLicense.GetAllDetainedLicenseInfo();

            guna2DataGridView1.DataSource = allData;
            DataTable filteredData;

            string searchType = comboBox1.Text;
            string searchValue = tbSearch.Text;

            if (searchType == "None" || string.IsNullOrWhiteSpace(searchValue))
            {
                filteredData = allData;
            }
            else
            {
                filteredData = clsDetainLicense.Search(searchValue, searchType);
                if (filteredData == null || filteredData.Rows.Count == 0)
                    filteredData = allData;
            }

            guna2DataGridView1.DataSource = filteredData;
            lblRecord.Text = "#Record : " + guna2DataGridView1.Rows.Count;
        }



        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageDetainedLicense_Load(object sender, EventArgs e)
        {
            LoadDataInfo();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense(-1);
            frm.ShowDialog();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            LoadDataInfo();
        }

        private void tbSearch_TextChange(object sender, EventArgs e)
        {
            LoadDataInfo();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataInfo();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[1].Value);

            clsLicense License = clsLicense.Find(LicenseID);
            clsDriver driver = clsDriver.Find(License.DriverID);

            frmShowPersonInfo frm = new frmShowPersonInfo(driver.PersonID);
            frm.ShowDialog();
        }

        private void ShowLicenseDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[1].Value);
            frmShowLicense frm = new frmShowLicense(LicenseID);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isReleased = Convert.ToBoolean(guna2DataGridView1.CurrentRow.Cells[3].Value);
            int LicenseID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[1].Value);
            if (isReleased)
            {
                releaseDetainedLicenseToolStripMenuItem.Enabled = false;

            }
            else
            {
                releaseDetainedLicenseToolStripMenuItem.Enabled = true;

                frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(LicenseID);
                frm.ShowDialog();

            }

            LoadDataInfo();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[1].Value);
            clsLicense license = clsLicense.Find(LicenseID);
            clsApplications application = clsApplications.Find(license.ApplicationID);

            clsLocalDrivingLicenseApplications clsLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplications.FindByApplicationID(application.ApplicationID);
            frmShowLicenseHistory frm = new frmShowLicenseHistory(clsLocalDrivingLicenseApplications.LocalDrivingLicenseID);
            frm.ShowDialog();
        }
    }
}
