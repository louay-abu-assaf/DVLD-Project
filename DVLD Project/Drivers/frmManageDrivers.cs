using DVLD_Buisness_Tier;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmManageDrivers : Form
    {

        private DataTable _dtAllDrivers;


        public frmManageDrivers()
        {
            InitializeComponent();

        }

       

        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            _dtAllDrivers = clsDriver.GetAllDriverInfo();
            guna2DataGridView1.DataSource = _dtAllDrivers;
            lblRecord.Text = guna2DataGridView1.Rows.Count.ToString();
            tbSearch.Visible = false;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbSearch_TextChange(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (comboBox1.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No":
                    FilterColumn = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (tbSearch.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllDrivers.DefaultView.RowFilter = "";
                lblRecord.Text = guna2DataGridView1.Rows.Count.ToString();
                return;
            }


            if (FilterColumn != "FullName" && FilterColumn != "NationalNo")
                //in this case we deal with numbers not string.
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, tbSearch.Text.Trim());
            else
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, tbSearch.Text.Trim());

            lblRecord.Text = _dtAllDrivers.Rows.Count.ToString();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbSearch.Visible = (comboBox1.Text != "None");


            if (comboBox1.Text == "None")
            {
                tbSearch.Enabled = false;
            }
            else
                tbSearch.Enabled = true;

            tbSearch.Text = "";
            tbSearch.Focus();
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id or user id is selected.
            if (comboBox1.Text == "Driver ID" || comboBox1.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }

}
