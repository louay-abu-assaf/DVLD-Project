using DVLD_Buisness_Tier;
using System;
using System.Data;
using System.Windows.Forms;


namespace DVLD_Project
{
    public partial class frmManageUsers : Form
    {

        DataTable _DtUsers = new DataTable();
        DataTable _DTAllUsers = new DataTable();
        public frmManageUsers()
        {
            InitializeComponent();
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            _DTAllUsers = clsUser.GetAllUsers();
            dgvUsers.DataSource = _DTAllUsers;
            _DtUsers = _DTAllUsers.Clone();

            int Number = dgvUsers.Rows.Count;
            lblNumberOfRecord.Text = "#Record : " + Number.ToString();
        }

        private void frmShowUserList_Load(object sender, EventArgs e)
        {

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            tbSearch.Hide();
            LoadUserInfo();


        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmAddNewUser frm = new frmAddNewUser(-1);
            frm.ShowDialog();

            LoadUserInfo();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";




            switch (comboBox1.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "User Name":
                    FilterColumn = "UserName";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "is Active":
                    FilterColumn = "isActive";
                    break;
                default:
                    FilterColumn = "None";
                    break;

            }

            if (tbSearch.Text.Trim() == "" || FilterColumn == "None")
            {
                _DtUsers.DefaultView.RowFilter = "";
                lblNumberOfRecord.Text = "#Record : " + _DTAllUsers.Rows.Count;
                return;
            }

            if (FilterColumn != "UserName" && FilterColumn != "FullName")
            {
                _DTAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, tbSearch.Text.Trim());
            }
            else
            {
                _DTAllUsers.DefaultView.RowFilter = string.Format("[{0}] like {1}", FilterColumn, tbSearch.Text.Trim());

            }
            lblNumberOfRecord.Text = "#Record : " + _DTAllUsers.Rows.Count;

        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Person ID" || comboBox1.SelectedItem.ToString() == "User ID")
            {

                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            tbSearch.Visible = (comboBox1.Text != "None" && comboBox1.Text != "is Active");
            comboBox2.Visible = comboBox1.Text == "is Active";


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            string FilterColumn = "IsActive";
            string FilterValue = comboBox2.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }

            if (FilterValue == "All")
                _DTAllUsers.DefaultView.RowFilter = "";
            else
                //in this case we deal with numbers not string.
                _DTAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue.Trim());

            lblNumberOfRecord.Text = _DTAllUsers.Rows.Count.ToString();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = int.Parse(dgvUsers.SelectedRows[0].Cells[0].Value.ToString());

            if (MessageBox.Show("Are You Sure you Want to Delete The User ", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsUser.DeleteUser(UserID))
                {
                    MessageBox.Show("User Deleted Successfully", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserInfo();
                }
            }

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = int.Parse(dgvUsers.SelectedRows[0].Cells["PersonID"].Value.ToString());

            if (PersonID > 0)
            {

                frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
                frm.ShowDialog();

            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = int.Parse(dgvUsers.SelectedRows[0].Cells["PersonID"].Value.ToString());

            frmAddNewUser frm = new frmAddNewUser(PersonID);
            frm.ShowDialog();

            LoadUserInfo();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmChangePassword frm = new frmChangePassword(int.Parse(dgvUsers.SelectedRows[0].Cells["UserID"].Value.ToString()));
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAddNewUser frm = new frmAddNewUser(-1);
            frm.ShowDialog();

            LoadUserInfo();
        }
    }
}
