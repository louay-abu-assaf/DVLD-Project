using DVLD.Classes;
using DVLD_Buisness_Tier;
using DVLD_Project.Properties;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace DVLD_Project
{

    public partial class frmAddNewPerson : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        public enum enGender { Male = 0, Female = 1 };
        private enMode _Mode;
        private int _PersonID = -1;
        clsPerson _Person;



        public delegate void PersonCreatedEventHandler(object sender, int PersonID);

        public event PersonCreatedEventHandler onPersonCreated;



        public frmAddNewPerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;

            if (PersonID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
                label1.Text = "Update Person Info";
            }


        }

        private void AddNewPerson_OnPersonAdded(object sender, int PersonID)
        {
            onPersonCreated?.Invoke(sender, PersonID);
        }


        private void AddNewPerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void ctrAddNewUser1_Load(object sender, EventArgs e)
        {

        }

        ///////////////////////////
        ///


        private void _ResetDefaultValues()
        {
            //this will initialize the reset the default values
            LoadCountry();

            if (_Mode == enMode.AddNew)
            {

                _Person = new clsPerson();
            }
            else
            {

            }

            //set default image for the person.
            if (rbMale.Checked)
                pbperosnImage.Image = Resources.Male_512;
            else
                pbperosnImage.Image = Resources.Female_512;

            //hide/show the remove link incase there is no image for the person.
            llblRemove.Visible = (pbperosnImage.ImageLocation != null);

            //we set the max date to 18 years from today, and set the default value the same.
            guna2DateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            guna2DateTimePicker1.Value = guna2DateTimePicker1.MaxDate;

            //should not allow adding age more than 100 years
            guna2DateTimePicker1.MinDate = DateTime.Now.AddYears(-100);

            //this will set default country to Syria.
            guna2ComboBox1.SelectedIndex = guna2ComboBox1.FindString("Syria");

            tbFirstName.Text = "";
            tbSecondName.Text = "";
            tbThirdName.Text = "";
            tbLastName.Text = "";
            tbNationalNo.Text = "";
            rbMale.Checked = true;
            tbPhone.Text = "";
            tbEmail.Text = "";
            tbAddress.Text = "";


        }

        private void LoadCountry()
        {
            DataTable dt = clsCountry.GetAllCountries();

            foreach (DataRow dr in dt.Rows)
            {
                guna2ComboBox1.Items.Add(dr["CountryName"]);
            }
        }





        private void _LoadData()
        {

            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //the following code will not be executed if the person was not found
            lblID.Text = _PersonID.ToString();
            tbFirstName.Text = _Person.FirstName;
            tbSecondName.Text = _Person.SecondName;
            tbThirdName.Text = _Person.ThirdName;
            tbLastName.Text = _Person.LastName;
            tbNationalNo.Text = _Person.NationalNo;
            guna2DateTimePicker1.Value = _Person.Date;

            if (_Person.Gender == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            tbAddress.Text = _Person.Address;
            tbPhone.Text = _Person.Phone;
            tbEmail.Text = _Person.Email;
            guna2ComboBox1.SelectedIndex = guna2ComboBox1.FindString(_Person.CountryInfo.CountryName);


            //load person image incase it was set.
            if (_Person.ImagePath != "")
            {
                pbperosnImage.ImageLocation = _Person.ImagePath;

            }

            //hide/show the remove link incase there is no image for the person.
            llblRemove.Visible = (_Person.ImagePath != "");

        }

        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();

            if (parentForm != null)
            {
                parentForm.Close();
            }
        }




        private bool ValidateInputFields()
        {
            return string.IsNullOrWhiteSpace(tbFirstName.Text) ||
                   string.IsNullOrWhiteSpace(tbSecondName.Text) ||
                   string.IsNullOrWhiteSpace(tbThirdName.Text) ||
                   string.IsNullOrWhiteSpace(tbLastName.Text) ||
                   string.IsNullOrWhiteSpace(tbPhone.Text) ||
                   string.IsNullOrWhiteSpace(tbNationalNo.Text) ||
                   string.IsNullOrWhiteSpace(tbAddress.Text);
        }

        private bool IsNationalNumberExist(string nationalNo)
        {
            return clsPerson.IsNationalNumberExist(nationalNo);
        }


        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return true;
            }
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }




        private void FillPersonDataFromControls()
        {
            if (_Person == null)
            {
                _Person = new clsPerson();
            }

            _Person.FirstName = tbFirstName.Text.Trim();
            _Person.SecondName = tbSecondName.Text.Trim();
            _Person.ThirdName = tbThirdName.Text.Trim();
            _Person.LastName = tbLastName.Text.Trim();
            _Person.NationalNo = tbNationalNo.Text.Trim();
            _Person.Phone = tbPhone.Text.Trim();
            _Person.Email = tbEmail.Text.Trim();
            _Person.Address = tbAddress.Text.Trim();
            _Person.Date = guna2DateTimePicker1.Value;
            _Person.Gender = rbMale.Checked ? 0 : 1;

            clsCountry country = clsCountry.Find(guna2ComboBox1.Text);
            _Person.CountryID = country.CountryID;

            if (pbperosnImage.ImageLocation != null)
                _Person.ImagePath = pbperosnImage.ImageLocation;
            else
                _Person.ImagePath = "";
        }

        private bool _HandlePersonImage()
        {

            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.


            //_Person.ImagePath contains the old Image, we check if it changed then we copy the new image
            if (_Person.ImagePath != pbperosnImage.ImageLocation)
            {
                if (_Person.ImagePath != "" || _Person.ImagePath != null)
                {
                    //first we delete the old image from the folder in case there is any.

                    if (_Person.ImagePath == null)
                    {

                    }
                    else
                    {
                        try
                        {
                            File.Delete(_Person.ImagePath);
                        }
                        catch (IOException)
                        {
                            // We could not delete the file.
                            //log it later   
                        }
                    }


                }

                if (pbperosnImage.ImageLocation != null)
                {
                    //then we copy the new image to the image folder after we rename it
                    string SourceImageFile = pbperosnImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbperosnImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

            }
            return true;
        }





        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (ValidateInputFields())
            {
                MessageBox.Show("Fill all fields to add new person", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidEmail(tbEmail.Text))
            {
                MessageBox.Show("Invalid email format", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbEmail.Focus();
                return;
            }

            if (_Person.Mode == clsPerson.enMode.AddNew)
            {
                if (IsNationalNumberExist(tbNationalNo.Text))
                {
                    MessageBox.Show("This National Number is Already Exists", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbNationalNo.Focus();
                    return;
                }
            }



            if (!_HandlePersonImage())
                return;

            FillPersonDataFromControls();

            if (_Person.Save())
            {
                lblID.Text = _Person.PersonID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                label1.Text = "Update Person Info";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }


        private void guna2RadioButton2_CheckedChanged(object sender, EventArgs e)
        {



        }



        private void ctrAddNewUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();

        }


        private void tbEmail_Leave(object sender, EventArgs e)
        {
            if (!IsValidEmail(tbEmail.Text))
            {
                errorProvider1.SetError(tbEmail, "Invalid email format");
            }
            else
            {
                errorProvider1.SetError(tbEmail, "");
            }
        }

        private void tbPhone_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbNationalNo_Leave(object sender, EventArgs e)
        {

            if (_Person.Mode == clsPerson.enMode.Update)
            {
                return;
            }

            if (clsPerson.IsNationalNumberExist(tbNationalNo.Text))
            {
                errorProvider1.SetError(tbNationalNo, "This National Number is Already Exists");
            }
            else
            {
                errorProvider1.SetError(tbNationalNo, "");

            }
        }

        private void bunifuButton2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbperosnImage.ImageLocation == null)
                pbperosnImage.Image = Resources.Female_512;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbperosnImage.ImageLocation == null)
                pbperosnImage.Image = Resources.Male_512;
        }

        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {

            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbperosnImage.Load(selectedFilePath);
                llblRemove.Visible = true;
                // ...
            }
        }

        private void llblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbperosnImage.ImageLocation = null;



            if (rbMale.Checked)
                pbperosnImage.Image = Resources.Male_512;
            else
                pbperosnImage.Image = Resources.Female_512;

            llblRemove.Visible = false;

        }
    }
}
