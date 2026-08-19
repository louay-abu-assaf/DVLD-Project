using DVLD_Buisness_Tier;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmAddNewUser : Form
    {

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private clsPerson _Person;
        private clsUser _User;
        int PersonID = -1;


        public frmAddNewUser(int personID)
        {
            InitializeComponent();
            PersonID = personID;

            if (PersonID == -1)
            {
                _Mode = enMode.AddNew;

            }
            else
            {
                _Mode = enMode.Update;
            }

            ctrSearchPersonInfo1.ctrPersonInfo1.PersonDataBack += Form2DataBack;


        }


        private void Form2DataBack(object sender, int PerssonID)
        {
            LoadPersonInfo(PersonID);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmAddNewPerson frm = new frmAddNewPerson(-1);

            frm.onPersonCreated += AddNewPersonDataBack;


            frm.ShowDialog();
        }

        private void AddNewPersonDataBack(object sender, int PersonID)
        {
            LoadPersonInfo(PersonID);
        }


        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (PersonID == -1)
            {
                MessageBox.Show("Person not found.");
                return;
            }

            clsCountry Country = clsCountry.Find(_Person.CountryID);
            ctrSearchPersonInfo1.LoadPersonInfo(PersonID);


        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string searchBy = ctrSearchPersonInfo1.SearchType.ToString();

            string searchText = ctrSearchPersonInfo1.SearchBox.Trim();


            DataTable dt = clsPerson.SearchPerson(searchBy, searchText);

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("sorry, Person not found ", "Not Found ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                ctrSearchPersonInfo1.ctrPersonInfo1.ShowLinkedLabel = true;
                MessageBox.Show("Person Found successfully", "Found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataRow dr = dt.Rows[0];

                _Person = new clsPerson();
                _Person.PersonID = Convert.ToInt32(dr["PersonID"]);
                _Person.FirstName = Convert.ToString(dr["FirstName"]);
                _Person.SecondName = Convert.ToString(dr["SecondName"]);
                _Person.ThirdName = Convert.ToString(dr["ThirdName"]);
                _Person.LastName = Convert.ToString(dr["LastName"]);
                _Person.NationalNo = Convert.ToString(dr["NationalNo"]);
                _Person.Date = Convert.ToDateTime(dr["DateOfBirth"]);
                _Person.Gender = Convert.ToByte(dr["Gender"]);
                _Person.Address = Convert.ToString(dr["Address"]);
                _Person.Phone = Convert.ToString(dr["Phone"]);
                _Person.Email = Convert.ToString(dr["Email"]);
                _Person.CountryID = Convert.ToInt32(dr["NationalityCountryID"]);
                _Person.ImagePath = Convert.ToString(dr["ImagePath"]);


            }
            clsCountry country = clsCountry.Find(_Person.CountryID);

            ctrSearchPersonInfo1.LoadPersonInfo(PersonID);

        }

        private void frmAddNewUser_Load(object sender, EventArgs e)
        {
            ctrSearchPersonInfo1.SearchType = 1;
            //comboBox1.SelectedIndex = 1;
            btnSave.Enabled = false;
            btnSave.Hide();

            if (_Mode == enMode.Update)
            {
                label1.Text = "Update User Info";
                ctrSearchPersonInfo1.groupBox.Enabled = false;


                LoadPersonInfo(PersonID);

            }

            if (_Mode == enMode.AddNew)
            {
                ctrSearchPersonInfo1.groupBox.Enabled = true;
                label1.Text = "Add New User";
                ctrSearchPersonInfo1.ctrPersonInfo1.ShowLinkedLabel = false;
            }

        }



        private void LoadUserInfoToFields(clsUser user)
        {


            tbUserName.Text = user.UserName;
            tbPassword.Text = user.Password;
            tbComfirm.Text = user.Password;
            checkBox1.Checked = user.IsActive;
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            PersonID = Convert.ToInt32(ctrSearchPersonInfo1.PersonId);

            if (_Mode == enMode.AddNew)
            {

                if (clsUser.isUserExist(PersonID))
                {
                    MessageBox.Show("Selected Person is Already Has a User, Choose anther one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }


            if (PersonID < 0)
            {
                MessageBox.Show("Please find a person first to continue.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsUser.isUserExist(PersonID))
            {
                _User = clsUser.FindByPersonID(PersonID);

                if (_User == null || _User.UserID == -1)
                {
                    MessageBox.Show("Failed to load existing user info.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _User.Mode = clsUser.enMode.Update;
                LoadUserInfoToFields(_User);
            }
            else
            {

                _User = new clsUser();
                _User.PersonID = PersonID;
                _User.Mode = clsUser.enMode.AddNew;
            }

            tabUser.SelectedIndex = 1;
            btnSave.Show();
            btnSave.Enabled = true;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ctrSearchPersonInfo1.comboBox.SelectedItem.ToString() == "Person ID" || ctrSearchPersonInfo1.comboBox.SelectedItem.ToString() == "Phone" || ctrSearchPersonInfo1.comboBox.SelectedItem.ToString() == "Gender")
            {

                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

            }

        }
        private bool TryGetValidPersonID(out int personID)
        {
            if (!int.TryParse(PersonID.ToString(), out personID) || personID <= 0)
            {
                MessageBox.Show("Please select a valid person first.", "Invalid Person", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(tbUserName.Text) ||
                string.IsNullOrWhiteSpace(tbPassword.Text) ||
                string.IsNullOrWhiteSpace(tbComfirm.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidatePasswordMatch()
        {
            if (tbPassword.Text != tbComfirm.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidateUserNameAvailability(string username)
        {
            if (_User == null || _User.UserID == -1)
            {

                if (clsUser.isUserNameTaken(username))
                {
                    MessageBox.Show("Username is already taken. Please choose another.", "Username Taken", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else if (_User.UserName != username && clsUser.isUserNameTaken(username))
            {

                MessageBox.Show("Username is already taken. Please choose another.", "Username Taken", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void PrepareUserObject(int personID, string username)
        {
            if (_User == null || _User.UserID == -1)
            {
                if (clsUser.isUserExist(personID))
                {
                    _User = clsUser.Find(personID);
                    _User.Mode = clsUser.enMode.Update;
                }
                else
                {
                    _User = new clsUser();
                    _User.Mode = clsUser.enMode.AddNew;
                }
            }
        }

        private void FillUserData(int personID)
        {
            _User.UserName = tbUserName.Text.Trim();
            _User.Password = tbPassword.Text;
            _User.IsActive = checkBox1.Checked;
            _User.PersonID = personID;
        }

        private bool SaveUser()
        {
            return _User.Save();
        }


        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (!TryGetValidPersonID(out int personID)) return;
            if (!ValidateInputs()) return;
            if (!ValidatePasswordMatch()) return;

            string username = tbUserName.Text.Trim();

            if (!ValidateUserNameAvailability(username)) return;

            PrepareUserObject(personID, username);
            FillUserData(personID);

            if (SaveUser())
            {
                MessageBox.Show(
                    _User.Mode == clsUser.enMode.AddNew ? "User added successfully!" : "User updated successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                label1.Text = "Update Person Info";


            }
            else
            {
                MessageBox.Show("Failed to save user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbUserName_Leave(object sender, EventArgs e)
        {


            if (clsUser.isUserNameTaken(tbUserName.Text))
            {
                errorProvider1.SetError(tbUserName, "User Name Taken");
            }
            else
            {
                errorProvider1.SetError(tbUserName, "");
            }
        }

        private void tbComfirm_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (tbPassword.Text != tbComfirm.Text)
            {
                errorProvider1.SetError(tbComfirm, "Password Not Match");
            }
            else
            {
                errorProvider1.SetError(tbComfirm, "");
            }
        }

        private void bunifuButton1_Click_1(object sender, EventArgs e)
        {
            tabUser.SelectedIndex = 0;
        }

        private void tbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbPassword.Text == null)
            {
                errorProvider1.SetError(tbPassword, "Password Can't be blank");
            }
            else
            {
                errorProvider1.SetError(tbPassword, "");
            }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrPersonInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
