using DVLD_Buisness_Tier;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmmanagePeople : Form
    {

        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();

        //only select the columns that you want to show in the grid
        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                         "FirstName", "SecondName", "ThirdName", "LastName",
                                                         "GenderCaption", "DateOfBirth", "CountryName",
                                                         "Phone", "Email");

        public frmmanagePeople()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;


        }

        private int GetPeopleCount()
        {
            return clsPerson.GetPeopleCount();
        }




        private void managePeople_Load(object sender, System.EventArgs e)
        {
            dgvPeople.DataSource = _dtPeople;
            comboBox1.SelectedIndex = 0;
            label3.Text = "#Record :" + dgvPeople.Rows.Count.ToString();
            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 90;

                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 100;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 100;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 100;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 100;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 100;

                dgvPeople.Columns[6].HeaderText = "Gender";
                dgvPeople.Columns[6].Width = 80;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 120;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 100;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 150;
            }

        }
        private void _RefreshPeoplList()
        {
            _dtAllPeople = clsPerson.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GenderCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

            dgvPeople.DataSource = _dtPeople;
            label3.Text = "#Record :" + dgvPeople.Rows.Count.ToString();
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            frmAddNewPerson addPerson = new frmAddNewPerson(-1);
            addPerson.ShowDialog();

            _RefreshPeoplList();
        }

        private void bunifuButton1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                tbSearch.Hide();
                tbSearch.Enabled = false;

            }
            else
            {
                tbSearch.Show();
                tbSearch.Enabled = true;
            }
        }

        private void guna2TextBox1_TextChanged(object sender, System.EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (comboBox1.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gender":
                    FilterColumn = "GenderCaption";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (tbSearch.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                label3.Text = "#Record :" + dgvPeople.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID")
                //in this case we deal with integer not string.

                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, tbSearch.Text.Trim());
            else
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, tbSearch.Text.Trim());

            label3.Text = "#Record :" + dgvPeople.Rows.Count.ToString();
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Person ID" || comboBox1.SelectedItem.ToString() == "Phone" || comboBox1.SelectedItem.ToString() == "Gender")
            {

                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

            }

        }
        private void showDetailsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

            if (dgvPeople.SelectedRows.Count > 0)
            {
                int personID = int.Parse(dgvPeople.SelectedRows[0].Cells[0].Value.ToString());
                frmShowPersonInfo frminfo = new frmShowPersonInfo(personID);
                frminfo.ShowDialog();
                _RefreshPeoplList();

            }
            else
            {
                MessageBox.Show("Error Happen");
            }



        }

        private void cuiDataGridView1_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewPerson frm = new frmAddNewPerson(-1);
            frm.ShowDialog();
            _RefreshPeoplList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(dgvPeople.SelectedRows[0].Cells[0].Value.ToString());
            frmAddNewPerson frm = new frmAddNewPerson(ID);
            frm.ShowDialog();
            _RefreshPeoplList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(dgvPeople.SelectedRows[0].Cells[0].Value.ToString());
            if (MessageBox.Show("Are you sure you want to delete it?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                clsPerson person = clsPerson.Find(ID);

                if (clsPerson.DeletePerson(ID))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(person.ImagePath) && File.Exists(person.ImagePath))
                        {
                            File.Delete(person.ImagePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    MessageBox.Show("Person Deleted Successfully", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeoplList();
                }
                else
                {
                    MessageBox.Show("Person Was Not Delete because it has data instead to it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }



        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            frmAddNewPerson frm = new frmAddNewPerson(-1);
            frm.ShowDialog();

            _RefreshPeoplList();
        }
    }
}
