using DVLD_Buisness_Tier;
using System;
using System.Data;
using System.Windows.Forms;


namespace DVLD_Project
{
    public partial class frmManageInternationalLicenseApplication : Form
    {
        public frmManageInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        //private void LoadDataInfo()
        //{
        //    guna2DataGridView1.DataSource  = clsInternationalLicense.GetAllInternationalLicenses();
        //    lblRecord.Text = "#Record : " + guna2DataGridView1.Rows.Count.ToString();

        //}

        private void LoadAllInternationalDrivingLicenseApplications()
        {
            DataTable allData = clsInternationalLicense.GetAllInternationalLicenses();

            DataTable filteredData;

            string searchType = comboBox1.Text;
            string searchValue = cbStatus.Visible ? cbStatus.Text : tbSearch.Text;


            if (cbStatus.Visible)
            {
                if (cbStatus.Text == "All")
                {
                    searchValue = ""; // نرسل قيمة فارغة لعرض الكل
                }
                else if (cbStatus.Text == "Yes")
                {
                    searchValue = "1";
                }
                else if (cbStatus.Text == "No")
                {
                    searchValue = "0";
                }
            }

            if (searchType == "None" || string.IsNullOrWhiteSpace(searchValue))
            {
                filteredData = allData;
            }
            else
            {
                filteredData = clsInternationalLicense.Search(searchValue, searchType);
                if (filteredData == null || filteredData.Rows.Count == 0)
                    filteredData = allData;
            }

            guna2DataGridView1.DataSource = filteredData;
            lblRecord.Text = "#Record : " + guna2DataGridView1.Rows.Count;
        }



        private void frmManageInternationalLicense_Load(object sender, EventArgs e)
        {
            LoadAllInternationalDrivingLicenseApplications();

            comboBox1.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmIssueInternationalLicense frm = new frmIssueInternationalLicense();
            frm.ShowDialog();
            LoadAllInternationalDrivingLicenseApplications();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[1].Value);
            clsLocalDrivingLicenseApplications clsLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplications.FindByApplicationID(AppID);

            frmShowLicenseHistory frm = new frmShowLicenseHistory(clsLocalDrivingLicenseApplications.LocalDrivingLicenseID);
            frm.ShowDialog();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[2].Value);
            clsDriver driver = clsDriver.Find(DriverID);



            frmShowPersonInfo frm = new frmShowPersonInfo(driver.PersonID);
            frm.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);

            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(ID);
            frm.ShowDialog();
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            LoadAllInternationalDrivingLicenseApplications();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                tbSearch.Visible = false;
                cbStatus.Visible = false;
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                cbStatus.Visible = true;
                tbSearch.Visible = false;
            }
            else
            {
                tbSearch.Visible = true;
                cbStatus.Visible = false;
            }
        }

        private void tbSearch_TextChange(object sender, EventArgs e)
        {
            LoadAllInternationalDrivingLicenseApplications();
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllInternationalDrivingLicenseApplications();
        }
    }
}
