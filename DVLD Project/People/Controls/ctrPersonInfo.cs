using DVLD_Buisness_Tier;
using DVLD_Project.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class ctrPersonInfo : UserControl
    {

        public delegate void PersonCreatedEventHandler(object sender, int PersonID);

        public event PersonCreatedEventHandler PersonDataBack;

        private clsPerson _Person;

        private int _PersonID = -1;

        public int personID
        {
            get { return _PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public ctrPersonInfo()
        {
            InitializeComponent();

        }


        public bool ShowLinkedLabel
        {
            get { return linkLabel1.Visible; }
            set { linkLabel1.Visible = value; }
        }


        private void DelegateDataBack(object sender, int personID)
        {
            // Exception here SomeTime 
            PersonDataBack?.Invoke(sender, personID);
        }

        private void ctrPersonInfo_Load(object sender, EventArgs e)
        {


        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
        }

        private void _LoadPersonImage()
        {
            if (_Person.Gender
                == 0)
                pictureBox1.Image = Resources.Male_512;
            else
                pictureBox1.Image = Resources.Female_512;

            string ImagePath = _Person.ImagePath;
            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pictureBox1.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }


        private void _FillPersonInfo()
        {
            linkLabel1.Enabled = true;
            _PersonID = _Person.PersonID;
            lblID.Text = _Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblName.Text = _Person.FullName;
            lblGender.Text = _Person.Gender == 0 ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblDate.Text = _Person.Date.ToShortDateString();
            lblCountry.Text = clsCountry.Find(_Person.CountryID).CountryName;
            lblAddress.Text = _Person.Address;

            _LoadPersonImage();


        }

        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDate.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pictureBox1.Image = Resources.Male_512;

        }


        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No. = " + NationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }



        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddNewPerson frm = new frmAddNewPerson(_PersonID);
            frm.ShowDialog();


            LoadPersonInfo(_PersonID);

        }
    }
}
