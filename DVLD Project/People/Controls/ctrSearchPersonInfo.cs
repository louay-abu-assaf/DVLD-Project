using DVLD_Buisness_Tier;
using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class ctrSearchPersonInfo : UserControl
    {


        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private clsPerson _Person;
        private clsUser _User;
        int PersonID = -1;

        public string SearchBox
        {
            get { return tbSearch.Text; }
            set { tbSearch.Text = value; }
        }

        public object SearchType
        {
            get { return cbSearch.SelectedItem; }
            set { cbSearch.SelectedItem = value; }
        }

        public ctrPersonInfo ctrPersonInfo1
        {
            get { return this.PersonInfo; }

        }

        public Guna2TextBox Box
        {
            get { return this.tbSearch; }
        }

        public ComboBox comboBox
        {
            get { return this.cbSearch; }
        }

        public GroupBox groupBox
        {
            get { return this.groupBox1; }
        }

        public int PersonId
        {
            get { return ctrPersonInfo1.personID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return ctrPersonInfo1.SelectedPersonInfo; }
        }


        public ctrSearchPersonInfo()
        {
            InitializeComponent();

            PersonInfo.PersonDataBack += Form2DataBack;
        }

        private void Form2DataBack(object sender, int PerssonID)
        {
            LoadPersonInfo(PersonID);
        }

        private void ctrPersonInfo1_Load(object sender, EventArgs e)
        {

        }

        private void ctrSearchPersonInfo_Load(object sender, EventArgs e)
        {
            cbSearch.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSearch.SelectedIndex = 1;
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person == null)
            {
                MessageBox.Show("Person not found.");
                return;
            }

            clsCountry Country = clsCountry.Find(_Person.CountryID);
            PersonInfo.LoadPersonInfo(PersonID);


        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {


        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void AddNewPersonDataBack(object sender, int PersonID)
        {
            LoadPersonInfo(PersonID);
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (cbSearch.SelectedItem.ToString() == "Person ID" || cbSearch.SelectedItem.ToString() == "Phone")
            {

                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string searchBy = cbSearch.SelectedItem?.ToString();

            string searchText = tbSearch.Text.Trim();


            DataTable dt = clsPerson.SearchPerson(searchBy, searchText);

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("sorry, Person not found ", "Not Found ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                PersonInfo.ShowLinkedLabel = true;

                DataRow dr = dt.Rows[0];
                _Person = new clsPerson();
                _Person.PersonID = Convert.ToInt32(dr["PersonID"]);


                MessageBox.Show("Person Found successfully", "Found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _Person = clsPerson.Find(Convert.ToInt32(dr["PersonID"]));

                if (_Person != null)
                {
                    LoadPersonInfo(_Person.PersonID);

                }


            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            frmAddNewPerson frm = new frmAddNewPerson(-1);

            frm.onPersonCreated += AddNewPersonDataBack;


            frm.ShowDialog();
        }

        private void PersonInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
